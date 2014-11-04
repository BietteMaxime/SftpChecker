using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpChecker.Tracking
{
    public class TrackingAccessor: IDisposable
    {
        private string Process { get; set; }
        private TrackingContext TrackingContext { get; set; }

        public TrackingAccessor(string process)
        {
            Process = process;
            TrackingContext = new TrackingContext();
        }

        public void AddCheck(bool isSuccess, string message)
        {
           
            TrackingContext.Checks.Add(new Check {Process = Process, IsSuccess = isSuccess, Message = message});
            TrackingContext.SaveChanges();
            
        }

        public Check GetLastCheck()
        {
            return TrackingContext.Checks.Last();
        }

        public void Dispose()
        {
            TrackingContext.Dispose();
        }
    }
}
