using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPSTeamSuitUI.Models
{
    public class ResultSummary
    { 
        public List<ResultModel> ResultViewModel { get; set; } 
    //    public List<ResultModel> ResultViewModel { get; set; }  
    }

    public class Data
    {
        public int MajorId { get; set; }

        public List<int> Minors { get; set; }

        public List<string> MinorsSequence { get; set; }
        public int rssi { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
 
        public string recordType { get; set; }
        public string os { get; set; }
        public string manufacturer { get; set; }
        public float recordingDuration { get; set; }
        public string recorderVersion { get; set; }
        public string devicemodel { get; set; }
        public string osVersion { get; set; }
        public string notes { get; set; }
    }

    public class NewResultModel
    {
        public string UUid { get; set; }

        public string minorList { get; set; } 
        public string major { get; set; } 
     //   public string major { get; set; } 
 
    }

    public class ResultModel
    {
        public string UUid { get; set; }

        public List<Data> Data { get; set; } 
    }
}
