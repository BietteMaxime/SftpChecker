using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Renci.SshNet;
using SftpChecker.Data;
using NDesk.Options;
using SftpChecker.Helpers;

namespace SftpChecker
{
    class Program
    {
        static T LoadFile<T>(string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var fileStream = new FileStream(filename, FileMode.Open))
            {
                return (T) xmlSerializer.Deserialize(fileStream);
            }
        }

        static void SaveObject<T>(T objectToSerialize, string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(filename))
            {
                xmlSerializer.Serialize(streamWriter, objectToSerialize);
            }
        }

        static ConnectionInfo BuildConnectionInfo(AuthFile authFile, ConnectionFile connectionFile)
        {
            var cryptKey = authFile.GetCryptKey();
            var authKey = authFile.GetAuthKey();

            var passwordAuth = new PasswordAuthenticationMethod(connectionFile.GetUsername(cryptKey, authKey),
                connectionFile.GetPassword(cryptKey, authKey));
            var keyboardAuth = new KeyboardInteractiveAuthenticationMethod(connectionFile.GetUsername(cryptKey, authKey));
            
            keyboardAuth.AuthenticationPrompt += (sender, args) =>
            {
                foreach (var prompt in args.Prompts)
                {
                    if (prompt.Request.StartsWith("Password:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        prompt.Response = connectionFile.GetPassword(cryptKey, authKey);
                    }
                }
            };

            var connectionInfo = new ConnectionInfo(
                connectionFile.GetHostname(cryptKey, authKey),
                connectionFile.GetPort(cryptKey, authKey),
                connectionFile.GetUsername(cryptKey, authKey),
                keyboardAuth, passwordAuth
                );

            return connectionInfo;
        }

        static bool TestSftpConnection(ConnectionInfo connectionInfo)
        {
            using (var sftpClient = new SftpClient(connectionInfo))
            {
                try
                {
                    sftpClient.Connect();
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }

        static int Main(string[] args)
        {
            var help = false;
            var version = false;

            var testSftp = false;
            string authFilename = null;
            string connectionFilename = null;
            var sendEmail = false;

            var getNewKey = false;

            /*//DEBUG CODE
              var getAuthFile = false;*/

            var setAuthFile = false;
            string cryptKey = null;
            string authKey = null;

            var setConnectionFile = false;
            string host = null;
            string port = null;
            string username = null;
            string password = null;

            var p = new OptionSet
            {
                {
                    "testSftp",
                    "initiate a simple authentification test to the SFTP server specified in the ConnectionFile. (AuthFile and ConnectionFile required)",
                    v => { testSftp = true; }
                },
                {
                    "authFile:",
                    "path the authFile.",
                    v => { authFilename = v; }
                },
                {
                    "connectionFile:",
                    "path the connectionFile.",
                    v => { connectionFilename = v; }
                },
                {
                    "newKey",
                    "generate a new key and output it.",
                    v => { getNewKey = true; }
                },
                {
                    "setAuthFile",
                    "generate a new AuthFile (require AuthFile, cryptKey, authKey).",
                    v => { setAuthFile = true; }
                },
                /*//DEBUG CODE
                {
                    "getAuthFile",
                    "read the AuthFile and print the cleartext values.",
                    (v) => { getAuthFile = true; }
                },*/
                {
                    "cryptKey:",
                    "cryptKey value (for setAuthFile only).",
                    v => { cryptKey = v; }
                },
                {
                    "authKey:",
                    "authKey value (for setAuthFile only).",
                    v => { authKey = v; }
                },
                {
                    "setConnectionFile",
                    "generate a new ConnectionFile (require AuthFile, ConnectionFile, Host, Port, Username, Password).",
                    v => { setConnectionFile = true;}
                },
                {
                    "host:",
                    "host value (for setConnectionFile only).",
                    v => { host = v; }
                },
                {
                    "port:",
                    "port value (for setConnectionFile only).",
                    v => { port = v; }
                },
                {
                    "username:",
                    "username value (for setConnectionFile only).",
                    v => { username = v; }
                },
                {
                    "password:",
                    "password value (for setConnectionFile only).",
                    v => { password = v; }
                },
                {
                    "sendEmail",
                    "send notification of failure by email. (Only during testSftp)",
                    v => { sendEmail = true;}
                },
                {
                    "h|?|help", "show this message and exit.",
                    v => help = v != null
                },
                {
                    "V|version", "output version information and exit.",
                    v => version = v != null
                },
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("Error: ");
                Console.WriteLine(e.Message);
                return -1;
            }

            if (help)
                p.WriteOptionDescriptions (Console.Out);
            if (version)
                Console.WriteLine ("SFTP checker version 1.0. Maxime Biette");

            if (testSftp)
            {
                Debug.Assert(authFilename != null, "authFilename != null");
                var authFile = LoadFile<AuthFile>(authFilename);

                Debug.Assert(connectionFilename != null, "connectionFilename != null");
                var connectionFile = LoadFile<ConnectionFile>(connectionFilename);

                var connectionInfo = BuildConnectionInfo(authFile, connectionFile);
                var successSftp = TestSftpConnection(connectionInfo);
                if (sendEmail && !successSftp)
                {
                    //Write email sending stuff here.
                }
            }
            if (getNewKey)
            {
                var newKey = AesThenHmac.NewKey();
                foreach( var bit in newKey)
                {
                    Console.Out.Write("{0},",bit);
                }
                Console.Out.Write("{0}",Convert.ToBase64String(newKey));
                return 0;
            }
            if (setAuthFile)
            {
                var authFile = new AuthFile();
                Debug.Assert(cryptKey != null, "cryptKey != null");
                authFile.SetCryptKey(cryptKey);
                Debug.Assert(authKey != null, "authKey != null");
                authFile.SetAuthKey(authKey);

                Debug.Assert(authFilename != null, "authFilename != null");
                SaveObject(authFile, authFilename);

                return 0;
            }
            /*//DEBUG CODE
            if (getAuthFile)
            {
                Debug.Assert(authFilename != null, "authFilename != null");
                var authFile = LoadFile<AuthFile>(authFilename);
                Console.Out.WriteLine("CryptKey: {0}",Convert.ToBase64String(authFile.GetCryptKey()));
                Console.Out.WriteLine("AuthKey: {0}", Convert.ToBase64String(authFile.GetAuthKey()));
                return 0;
            }*/
            if (setConnectionFile)
            {
                Debug.Assert(authFilename != null, "authFilename != null");
                var authFile = LoadFile<AuthFile>(authFilename);
                var connCryptKey = authFile.GetCryptKey();
                var connAuthKey = authFile.GetAuthKey();

                var connectionFile = new ConnectionFile();
                Debug.Assert(host != null, "host != null");
                connectionFile.SetHostname(host, connCryptKey, connAuthKey);
                Debug.Assert(port != null, "port != null");
                connectionFile.SetPort(port, connCryptKey, connAuthKey);
                Debug.Assert(username != null, "username != null");
                connectionFile.SetUsername(username, connCryptKey, connAuthKey);
                Debug.Assert(password != null, "password != null");
                connectionFile.SetPassword(password, connCryptKey, connAuthKey);

                Debug.Assert(connectionFilename != null, "connectionFilename != null");
                SaveObject(connectionFile, connectionFilename);

                return 0;
            }
            return -1;
        }
        
    }
}
