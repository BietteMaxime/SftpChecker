using System;
using System.Text;
using SftpChecker.Helpers;

namespace SftpChecker.Data
{
    public class AuthFile
    {
        private static readonly byte[] AuthfileInternalCryptKey =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        };

        private static readonly byte[] AuthfileInternalAuthKey =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        };

        public string CryptKey { get; set; }
        public string AuthKey { get; set; }

        private static byte[] GetKey(string value)
        {
            var key = AesThenHmac.SimpleDecrypt(value, AuthfileInternalCryptKey, AuthfileInternalAuthKey);
            return Convert.FromBase64String(key);
        }

        private void SetKey(string key, out string value)
        {
            value = AesThenHmac.SimpleEncrypt(key, AuthfileInternalCryptKey, AuthfileInternalAuthKey);
        }

        public byte[] GetCryptKey()
        {
            return GetKey(CryptKey);
        }

        public void SetCryptKey(string key)
        {
            string cryptKey;
            SetKey(key, out cryptKey);
            CryptKey = cryptKey;
        }

        public byte[] GetAuthKey()
        {
            return GetKey(AuthKey);
        }

        public void SetAuthKey(string key)
        {
            string authKey;
            SetKey(key, out authKey);
            AuthKey = authKey;
        }
        
    }
}
