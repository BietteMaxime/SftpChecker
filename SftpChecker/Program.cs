using System;
using System.Diagnostics;
using EncryptionLibrary.Data;
using EncryptionLibrary.Helpers;
using NDesk.Options;

namespace SftpChecker
{
    class Program
    {
        static int Main(string[] args)
        {
            var help = false;
            var version = false;

            var testSftp = false;
            string authFilename = null;
            string connectionFilename = null;
            var sendEmail = false;

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
                var authFile = FileHandler.LoadFile<AuthFile>(authFilename);

                Debug.Assert(connectionFilename != null, "connectionFilename != null");
                var connectionFile = FileHandler.LoadFile<ConnectionFile>(connectionFilename);

                var sftpChecker = new Checker(authFile, connectionFile);

                var successSftp = sftpChecker.TestSftpConnection();

                if (sendEmail)
                {
                    //Write email sending stuff here.
                }
            }
            return 0;
        }
        
    }
}
