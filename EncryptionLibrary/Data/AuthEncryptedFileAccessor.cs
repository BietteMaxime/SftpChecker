using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptionLibrary.Helpers;

namespace EncryptionLibrary.Data
{
    public abstract class AuthEncryptedFileAccessor<T>: IDisposable
    {
        protected readonly T _encryptedFile;
        private readonly AuthFile _authFile;

        public AuthEncryptedFileAccessor(AuthFile authFile, T encryptedFile)
        {
            _encryptedFile = encryptedFile;
            _authFile = authFile;
        }

        // Encryption/Decryption logic
        protected string GetValue(string encryptedValue)
        {
            using (var authFileAccessor = new AuthFileAccessor(_authFile))
            {
                return AesThenHmac.SimpleDecrypt(encryptedValue, authFileAccessor.GetCryptKey(),
                    authFileAccessor.GetAuthKey());
            }
        }

        protected void SetValue(string secret, out string encryptedValue)
        {
            using (var authFileAccessor = new AuthFileAccessor(_authFile))
            {
                encryptedValue = AesThenHmac.SimpleEncrypt(secret, authFileAccessor.GetCryptKey(),
                    authFileAccessor.GetAuthKey());
            }
        }

        public void Dispose()
        {
        }
    }
}
