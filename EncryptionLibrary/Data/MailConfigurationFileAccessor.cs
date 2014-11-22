using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionLibrary.Data
{
    public class MailConfigurationFileAccessor: AuthEncryptedFileAccessor<MailConfigurationFile>
    {
        // Constructor
        public MailConfigurationFileAccessor(AuthFile authFile,MailConfigurationFile mailConfigurationFile)
            : base(authFile, mailConfigurationFile)
        {
        }

        // Getters/Setters
        public string GetServerName()
        {
            return GetValue(_encryptedFile.ServerName);
        }
        public void SetServerName(string serverName)
        {
            string encryptedServerName;
            SetValue(serverName, out encryptedServerName);
            _encryptedFile.ServerName = encryptedServerName;
        }

        public string GetMailFile()
        {
            return GetValue(_encryptedFile.MailFile);
        }
        public void SetMailFile(string mailFile)
        {
            string encryptedMailFile;
            SetValue(mailFile, out encryptedMailFile);
            _encryptedFile.MailFile = encryptedMailFile;
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
    }
}
