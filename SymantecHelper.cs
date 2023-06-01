using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CPPMHelper
{
    internal class SymantecHelper
    {
        private const string _doScanPath = @"C:\Program Files (x86)\Symantec\Symantec Endpoint Protection\DoScan.exe";
        private const string _localLogPath = @"C:\ProgramData\Symantec\Symantec Endpoint Protection\CurrentVersion\Data\Logs\AV\";
        private const string _dateFormat = "MMddyyyy";
        private const string _fileSuffix = ".log";
        private const string _successCode = "0:0:0:0:0";
        private const string _scanResultCode = "2";

        public SymantecHelper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VolumeName"> Must be in this format D:</param>
        /// <returns>Bool -> True Means that nothing is found</returns>
        internal bool ExecuteScan(string VolumeName)
        {
            string logfile = GetCurrentLogFileName();
            string[] start = GetLogContent(logfile);

            StartSymantecScan(VolumeName);

            string[] stop = GetLogContent(logfile);
            string[] NewLines = GetDiffrenceBetweenLog(start, stop);

            return CheckIfScanDetectedNothing(NewLines);
        }

        private string GetCurrentLogFileName()
        {
            string current = $"{DateTime.Now.ToString(_dateFormat)}{_fileSuffix}";

            return $"{_localLogPath}{current}";
        }

        private string[] GetLogContent(string Path)
        {
            string Result = string.Empty;
            using(FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] Data = new byte[fs.Length];
                fs.Read(Data, 0, Data.Length);

                Result = Encoding.UTF8.GetString(Data);
            }
            return Result.Split(Environment.NewLine);
        }

        private string[] GetDiffrenceBetweenLog(string[] StartLog, string[] EndLog)
        {
            string[] Result = EndLog[(StartLog.Length - 1)..];
            Result = Result.Where(single => !single.Equals(string.Empty)).ToArray();
            return Result;
        } 

        private bool CheckIfScanDetectedNothing(string[]NewLines)
        {
            bool Result = true;
            foreach(string NewLine in NewLines)
            {
                string[] SingleLine = NewLine.Split(",");
                //Check if Log line is the start or the result Line
                if (SingleLine[1].Equals(_scanResultCode))
                {
                    string[] scanresult = SingleLine[17].Split(":");
                    if (!scanresult[0].Equals("0"))
                    {
                        Result = false;
                        break;
                    }
                }
                
            }

            return Result;
        }

        private void StartSymantecScan(string VolumeName)
        {
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = _doScanPath,
                    Arguments = $"/ScanDir {VolumeName}",
                }
            };
            proc.Start();
            proc.WaitForExit();
        }
    }
}
