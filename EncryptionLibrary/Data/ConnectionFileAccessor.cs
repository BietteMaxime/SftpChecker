using System;
using EncryptionLibrary.Helpers;

namespace EncryptionLibrary.Data
{
    public class ConnectionFileAccessor: IDisposable
    {
        // Internal reference to the encrypted file.
        private readonly ConnectionFile _connectionFile;
        private readonly AuthFile _authFile;

        // Constructor
        public ConnectionFileAccessor(ConnectionFile connectionFile, AuthFile authFile)
        {
            _connectionFile = connectionFile;
            _authFile = authFile;
        }

        // Encryption/Decryption logic
        private string GetValue(string encryptedValue)
        {
            using (var authFileAccessor = new AuthFileAccessor(_authFile))
            {
                return AesThenHmac.SimpleDecrypt(encryptedValue, authFileAccessor.GetCryptKey(),
                    authFileAccessor.GetAuthKey());
            }
        }

        // Getters/Setters
        private void SetValue(string secret, out string encryptedValue)
        {
            using (var authFileAccessor = new AuthFileAccessor(_authFile))
            {
                encryptedValue = AesThenHmac.SimpleEncrypt(secret, authFileAccessor.GetCryptKey(),
                    authFileAccessor.GetAuthKey());
            }
        }

        public string GetHostname()
        {
            return GetValue(_connectionFile.Hostname);
        }
        public void SetHostname(string hostname)
        {
            string encryptedHostname;
            SetValue(hostname, out encryptedHostname);
            _connectionFile.Hostname = encryptedHostname;
        }

        public int GetPort()
        {
            return Convert.ToInt32(GetValue(_connectionFile.Port));
        }
        public void SetPort(string port)
        {
            string encryptedPort;
            SetValue(port, out encryptedPort);
            _connectionFile.Port = encryptedPort;
        }

        public string GetUsername()
        {
            return GetValue(_connectionFile.Username);
        }
        public void SetUsername(string username)
        {
            string encryptedUsername;
            SetValue(username, out encryptedUsername);
            _connectionFile.Username = encryptedUsername;
        }

        public string GetPassword()
        {
            return GetValue(_connectionFile.Password);
        }
        public void SetPassword(string password)
        {
            string encryptedPassword;
            SetValue(password, out encryptedPassword);
            _connectionFile.Password = encryptedPassword;
        }

        public void Dispose()
        {
        }
    }
}
