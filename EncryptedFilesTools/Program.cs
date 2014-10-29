using System;
using System.Diagnostics;
using EncryptionLibrary.Data;
using EncryptionLibrary.Helpers;
using NDesk.Options;

namespace EncryptedFilesTools
{
    class Program
    {
        static int Main(string[] args)
        {
            var help = false;
            var version = false;

            string authFilename = null;
            string connectionFilename = null;

            var getNewKey = false;

            var getAuthFile = false;

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
                {
                    "getAuthFile",
                    "read the AuthFile and print the cleartext values.",
                    v => { getAuthFile = true; }
                },
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
                p.WriteOptionDescriptions(Console.Out);
            if (version)
                Console.WriteLine("SFTP checker version 1.0. Maxime Biette");

            if (getNewKey)
            {
                var newKey = AesThenHmac.NewKey();
                foreach (var bit in newKey)
                {
                    Console.Out.Write("{0},", bit);
                }
                Console.Out.Write("{0}", Convert.ToBase64String(newKey));
                return 0;
            }
            if (setAuthFile)
            {
                var authFile = new AuthFile();
                using (var authFileAccessor = new AuthFileAccessor(authFile))
                {
                    Debug.Assert(cryptKey != null, "cryptKey != null");
                    authFileAccessor.SetCryptKey(cryptKey);
                    Debug.Assert(authKey != null, "authKey != null");
                    authFileAccessor.SetAuthKey(authKey);
                }
                Debug.Assert(authFilename != null, "authFilename != null");
                FileHandler.SaveObject(authFile, authFilename);

                return 0;
            }
            if (getAuthFile)
            {
                Debug.Assert(authFilename != null, "authFilename != null");
                var authFile = FileHandler.LoadFile<AuthFile>(authFilename);
                using (var authFileAccessor = new AuthFileAccessor(authFile))
                {
                    Console.Out.WriteLine("CryptKey: {0}", Convert.ToBase64String(authFileAccessor.GetCryptKey()));
                    Console.Out.WriteLine("AuthKey: {0}", Convert.ToBase64String(authFileAccessor.GetAuthKey()));
                }
                return 0;
            }
            if (setConnectionFile)
            {
                Debug.Assert(authFilename != null, "authFilename != null");
                var authFile = FileHandler.LoadFile<AuthFile>(authFilename);

                var connectionFile = new ConnectionFile();
                using (var connectionFileAccessor = new ConnectionFileAccessor(connectionFile, authFile))
                {
                    Debug.Assert(host != null, "host != null");
                    connectionFileAccessor.SetHostname(host);
                    Debug.Assert(port != null, "port != null");
                    connectionFileAccessor.SetPort(port);
                    Debug.Assert(username != null, "username != null");
                    connectionFileAccessor.SetUsername(username);
                    Debug.Assert(password != null, "password != null");
                    connectionFileAccessor.SetPassword(password);
                }
                Debug.Assert(connectionFilename != null, "connectionFilename != null");
                FileHandler.SaveObject(connectionFile, connectionFilename);

                return 0;
            }
            return -1;
        }

    }
}
