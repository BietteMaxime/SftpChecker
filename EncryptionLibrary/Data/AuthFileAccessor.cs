using System;
using EncryptionLibrary.Helpers;

namespace EncryptionLibrary.Data
{
    public class AuthFileAccessor: IDisposable
    {
        // Internal keys. (built in as the secret is not really here)
        private static readonly byte[] AuthfileInternalCryptKey = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private static readonly byte[] AuthfileInternalAuthKey = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // Internal reference to the encrypted file.
        private readonly AuthFile _authFile;

        // Constructor
        public AuthFileAccessor(AuthFile authFile)
        {
            _authFile = authFile;
        }

        // Encryption/Decryption logic
        private static byte[] GetKey(string value)
        {
            var key = AesThenHmac.SimpleDecrypt(value, AuthfileInternalCryptKey, AuthfileInternalAuthKey);
            return Convert.FromBase64String(key);
        }

        private static void SetKey(string key, out string value)
        {
            value = AesThenHmac.SimpleEncrypt(key, AuthfileInternalCryptKey, AuthfileInternalAuthKey);
        }

        // Getters/Setters
        public byte[] GetCryptKey()
        {
            return GetKey(_authFile.CryptKey);
        }

        public void SetCryptKey(string key)
        {
            string cryptKey;
            SetKey(key, out cryptKey);
            _authFile.CryptKey = cryptKey;
        }

        public byte[] GetAuthKey()
        {
            return GetKey(_authFile.AuthKey);
        }

        public void SetAuthKey(string key)
        {
            string authKey;
            SetKey(key, out authKey);
            _authFile.AuthKey = authKey;
        }

        public void Dispose()
        {
        }
    }
}
