using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPMHelper.Models.Result.Models
{
    internal interface IResult
    {
        bool Success { get; set; }
        int ErrorCode { get; set; }
        string ErrorMessage { get; set; }
        DateTime TimeStamp { get; set; }
        string User { get; set; }
        string Hostname { get; set; }

        public void InsertDefaults()
        {
            TimeStamp = DateTime.Now;
            User = Environment.UserName;
            Hostname = Environment.MachineName;
        }
    }
}
