using CPPMHelper;
using CPPMHelper.Models.Result.Models;
using System.CommandLine;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

const string _adminSID = "S-1-5-32-544";

RootCommand RootCommand = new RootCommand();

Command AdminCheck = new Command("--AdminCheck", "Check if current user is admin");
Option<string> AdminGroupSid = new Option<string>("--GroupSID", "Enter the SID for the Admin Check");
AdminGroupSid.SetDefaultValue(_adminSID);

AdminCheck.Add(AdminGroupSid);
AdminCheck.SetHandler((varibaleAdminSID) =>
{
    IResult Result = new AdminCheckResult();
    using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
    {
        foreach (Claim SingleClaim in identity.Claims)
        {
            if (SingleClaim.Value.Equals(varibaleAdminSID))
            {
                Result.InsertDefaults();
                Result.Success = false;
                Result.ErrorCode = (int)AdminCheckResultCode.IsAdmin;
                Result.ErrorMessage = AdminCheckResultCode.IsAdmin.ToString();
            }
        }
    }
    
    if (Result.User == null)
    {
        Result.InsertDefaults();
        Result.Success = true;
        Result.ErrorCode = (int)AdminCheckResultCode.IsNotAdmin;
        Result.ErrorMessage = AdminCheckResultCode.IsNotAdmin.ToString();
    }

    Console.WriteLine(JsonSerializer.Serialize(Result));
}, AdminGroupSid);
RootCommand.Add(AdminCheck);


Command UsbDetect = new Command("--USBCheck", "Start with the usb mass Storage Check");
UsbDetect.SetHandler(() =>
{
    List<DriveInfo> AllDrives = DriveInfo.GetDrives().Where(
        SingleDrive => SingleDrive.DriveType != DriveType.Network
        && !SingleDrive.Name.Equals("C:\\")
        ).ToList();
    SymantecHelper Symantec = new SymantecHelper();

    bool Result;
    IResult ResultOutput = new SymantecCheckResult();
    ResultOutput.InsertDefaults();
    if(AllDrives.Count > 0)
    {
        foreach (DriveInfo Drive in AllDrives)
        {
            Result = Symantec.ExecuteScan(Drive.Name.Replace(@"\", ""));
            if (Result == false)
            {
                //Deteced Something
                ResultOutput.Success = Result;
                ResultOutput.ErrorMessage = $"Symantec found something on: {Drive.Name}";
                ResultOutput.ErrorCode = 1;
                break;
            }
            else
            {
                //Everything is fine
                ResultOutput.Success = Result;
                ResultOutput.ErrorMessage = "Nothing found";
                ResultOutput.ErrorCode = 0;
            }
        }
    }
    else
    {
        ResultOutput.Success = true;
        ResultOutput.ErrorMessage = "No Devices found";
        ResultOutput.ErrorCode = 0;
    }
    
    Console.Write(JsonSerializer.Serialize(ResultOutput));
});
RootCommand.Add(UsbDetect);

Command GlobalVPNDisable = new Command("--DisableGlobalVPN", "Disable the GlobalVPN Client");

GlobalVPNDisable.SetHandler(() =>
{
    GlobalVPNHelper GVH = new GlobalVPNHelper();
    GVH.DisableClient();
});

RootCommand.Add(GlobalVPNDisable);

RootCommand.Invoke(args);