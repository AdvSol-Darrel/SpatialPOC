using System;
using System.Collections.Generic;

#nullable disable

namespace SpatialPOC.Models
{
    public partial class JobRunDetail
    {
        public long? Jobid { get; set; }
        public long Runid { get; set; }
        public int? JobPid { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Command { get; set; }
        public string Status { get; set; }
        public string ReturnMessage { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
