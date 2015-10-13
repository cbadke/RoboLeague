#Service Bus 1.1 doesn't work properly if VS2015 is installed. This is
#caused by a DLL that isn't shipped with Service Bus.
#For more info:
#http://stackoverflow.com/questions/28333034/how-do-i-report-a-bug-in-windows-server-service-bus/30413095#30413095
#https://github.com/matthewcanty/Microsoft.Cloud.Common.AzureStorage.FAKE.dll

function Find-Exe {
  param(
    [String]$Path,
    [String]$File
  )

  Get-ChildItem -Recurse -Path $Path -Filter $File -ErrorAction "SilentlyContinue" | Select-Object -First 1 -ExpandProperty FullName
}

pushd $PSScriptRoot

$snExe = Find-Exe "C:\Program Files (x86)\" "sn.exe"
$gacUtilExe = Find-Exe "C:\Program Files (x86)\" "gacutil.exe"

& $gacUtilExe /u Microsoft.Cloud.Common.AzureStorage
& $snExe -Vu Microsoft.Cloud.Common.AzureStorage.dll

popd
