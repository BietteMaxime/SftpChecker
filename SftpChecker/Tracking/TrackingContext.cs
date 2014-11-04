using System.Data.Entity;

namespace SftpChecker.Tracking
{
    public class TrackingContext: DbContext
    {
        public DbSet<Check> Checks { get; set; }
    }
}
