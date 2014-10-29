using System;
using System.Linq;
using EncryptionLibrary.Data;
using Renci.SshNet;

namespace SftpChecker
{
    public class SftpChecker
    {
        private readonly AuthFile _authFile;
        private readonly ConnectionFile _connectionFile;


        public SftpChecker(AuthFile authFile, ConnectionFile connectionFile)
        {
            _authFile = authFile;
            _connectionFile = connectionFile;
        }

        private ConnectionInfo BuildConnectionInfo()
        {
            KeyboardInteractiveAuthenticationMethod keyboardAuth;
            ConnectionInfo connectionInfo;

            using (var connectionFileAccessor = new ConnectionFileAccessor(_connectionFile, _authFile))
            {
                var passwordAuth = new PasswordAuthenticationMethod(
                    connectionFileAccessor.GetUsername(),
                    connectionFileAccessor.GetPassword()
                    );
                keyboardAuth = new KeyboardInteractiveAuthenticationMethod(
                    connectionFileAccessor.GetUsername()
                    );

                connectionInfo = new ConnectionInfo(
                    connectionFileAccessor.GetHostname(),
                    connectionFileAccessor.GetPort(),
                    connectionFileAccessor.GetUsername(),
                    keyboardAuth, passwordAuth
                    );
            }

            keyboardAuth.AuthenticationPrompt += (sender, args) =>
            {
                foreach (
                    var prompt in args.Prompts.Where(
                            prompt => prompt.Request.StartsWith("Password:", StringComparison.InvariantCultureIgnoreCase)
                        )
                    )
                {
                    using (var connectionFileAccessor = new ConnectionFileAccessor(_connectionFile, _authFile))
                    {
                        prompt.Response = connectionFileAccessor.GetPassword();
                    }
                }
            };

            return connectionInfo;
        }

        public bool TestSftpConnection()
        {
            var connectionInfo = BuildConnectionInfo();
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
    }
}
