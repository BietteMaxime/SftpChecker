using System;

namespace EncryptionLibrary.Data
{
    public class ConnectionFileAccessor: AuthEncryptedFileAccessor<ConnectionFile>
    {
        // Constructor
        public ConnectionFileAccessor(AuthFile authFile, ConnectionFile connectionFile)
            : base(authFile,connectionFile)
        {
        }

        // Getters/Setters
        public string GetHostname()
        {
            return GetValue(_encryptedFile.Hostname);
        }
        public void SetHostname(string hostname)
        {
            string encryptedHostname;
            SetValue(hostname, out encryptedHostname);
            _encryptedFile.Hostname = encryptedHostname;
        }

        public int GetPort()
        {
            return Convert.ToInt32(GetValue(_encryptedFile.Port));
        }
        public void SetPort(string port)
        {
            string encryptedPort;
            SetValue(port, out encryptedPort);
            _encryptedFile.Port = encryptedPort;
        }

        public string GetUsername()
        {
            return GetValue(_encryptedFile.Username);
        }
        public void SetUsername(string username)
        {
            string encryptedUsername;
            SetValue(username, out encryptedUsername);
            _encryptedFile.Username = encryptedUsername;
        }

        public string GetPassword()
        {
            return GetValue(_encryptedFile.Password);
        }
        public void SetPassword(string password)
        {
            string encryptedPassword;
            SetValue(password, out encryptedPassword);
            _encryptedFile.Password = encryptedPassword;
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
