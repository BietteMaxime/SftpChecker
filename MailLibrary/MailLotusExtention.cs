using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domino;

namespace MailLibrary
{
    public static class MailLotusExtention
    {
        public static NotesDocument ConvertToNoteDocument(this Mail mail, NotesDatabase notesDatabase)
        {
            NotesDocument notesDocument = notesDatabase.CreateDocument();

            //Set document type 
            notesDocument.ReplaceItemValue("Form", "Memo");

            //Set notes memo fields (To: CC: Bcc: Subject etc)
            notesDocument.ReplaceItemValue("SendTo", mail.To.ToArray());
            notesDocument.ReplaceItemValue("CopyTo", mail.Cc.ToArray());
            notesDocument.ReplaceItemValue("Subject", mail.Subject);

            //Set notes body as rich text
            notesDocument.CreateRichTextItem("Body").AppendText(mail.Body);
            
            return notesDocument;
        }
    }
}
