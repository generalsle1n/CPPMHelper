
# CPPMHelper

This tool is an command line program that provides simple functions to work with with Clearpass Policy Manager

## Authors

- [@generalSle1n](https://www.github.com/generalsle1n)


## Acknowledgements

 - [.Net Comamndline Api](https://github.com/dotnet/command-line-api)


## Tech Stack

This tool is Build on .Net 6 so it can be runned on Windows


## Documentation

The following Comamnds are available
### --AdminCheck
This command check if the executing user is an local administrator
#### Avaliable Paramters
- --GroupSID 
- - Here you can enter an custom SID, the default SID is: S-1-5-32-544)

### --USBCheck
This command scans all external attached storage and scan it with an antivirus engine (At the moment there is only support for Symantec Endpoint Protection)

#### Avaliable Paramters

### -DisableGlobalVPN
This command prevents the user from starting an connection with the sonicwall vpn client GlobalVPN

#### Avaliable Paramters
