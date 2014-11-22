using System.Data.Entity;

namespace SftpChecker.Tracking
{
    public class TrackingContext: DbContext
    {
        //The list of all the checks performed and their result.
        public DbSet<Check> Checks { get; set; }
        //The mailing list to send errors and summary of the checks.
        public DbSet<MailAddress> MailingList { get; set; }
    }
}
