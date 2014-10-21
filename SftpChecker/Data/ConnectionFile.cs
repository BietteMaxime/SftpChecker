using System;
using SftpChecker.Helpers;

namespace SftpChecker.Data
{
    public class ConnectionFile
    {
        public string EncryptedHostname { get; set; }
        public string EncryptedPort { get; set; }
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }

        private static string GetValue(string encryptedValue, byte[] cryptKey, byte[] authKey)
        {
            return AesThenHmac.SimpleDecrypt(encryptedValue, cryptKey, authKey);
        }

        private void SetValue(string secret, byte[] cryptKey, byte[] authKey, out string encryptedValue)
        {
            encryptedValue = AesThenHmac.SimpleEncrypt(secret, cryptKey, authKey);
        }

        public string GetHostname(byte[] cryptKey, byte[] authKey)
        {
            return GetValue(EncryptedHostname, cryptKey, authKey);
        }
        public void SetHostname(string hostname, byte[] cryptKey, byte[] authKey)
        {
            string encryptedHostname;
            SetValue(hostname, cryptKey, authKey, out encryptedHostname);
            EncryptedHostname = encryptedHostname;
        }

        public int GetPort(byte[] cryptKey, byte[] authKey)
        {
            return Convert.ToInt32(GetValue(EncryptedPort, cryptKey, authKey));
        }
        public void SetPort(string port, byte[] cryptKey, byte[] authKey)
        {
            string encryptedPort;
            SetValue(port, cryptKey, authKey, out encryptedPort);
            EncryptedPort = encryptedPort;
        }

        public string GetUsername(byte[] cryptKey, byte[] authKey)
        {
            return GetValue(EncryptedUsername, cryptKey, authKey);
        }
        public void SetUsername(string username, byte[] cryptKey, byte[] authKey)
        {
            string encryptedUsername;
            SetValue(username, cryptKey, authKey, out encryptedUsername);
            EncryptedUsername = encryptedUsername;
        }

        public string GetPassword(byte[] cryptKey, byte[] authKey)
        {
            return GetValue(EncryptedPassword, cryptKey, authKey);
        }
        public void SetPassword(string password, byte[] cryptKey, byte[] authKey)
        {
            string encryptedPassword;
            SetValue(password, cryptKey, authKey, out encryptedPassword);
            EncryptedPassword = encryptedPassword;
        }
    }
}
