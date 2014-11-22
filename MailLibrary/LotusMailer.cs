using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domino;

namespace MailLibrary
{
    public class LotusMailer: IDisposable
    {
        private NotesSession _notesSession;
        private NotesDatabase _notesDataBase;

        public LotusMailer(string serverName, string mailFile, string password)
        {
            _notesSession = new NotesSession();
            _notesSession.Initialize(password);
            _notesDataBase = _notesSession.GetDatabase(serverName, mailFile, false);
        }

        public void Send(Mail mail)
        {
            mail.ConvertToNoteDocument(_notesDataBase).Send(false);
        }

        public void Dispose()
        {
            _notesDataBase = null;
            _notesSession = null;
        }

    }
}
