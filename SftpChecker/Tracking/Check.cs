using System;

namespace SftpChecker.Tracking
{
    public class Check
    {
        public int Id { get; set; }
        public string Process { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}