using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPMHelper.Models.Result.Models
{
    internal class SymantecCheckResult : IResult
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeStamp { get; set; }
        public string User { get; set; }
        public string Hostname { get; set; }
    }
}
