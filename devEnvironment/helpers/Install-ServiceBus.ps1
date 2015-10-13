param(
  [String]$RunAsAccount,
  [String]$SBFarmConnString = 'Data Source=.;Initial Catalog=SbManagementDB;Integrated Security=True'
)

if (-not $RunAsAccount) {
  $RunAsAccount = $(whoami)
}
$RunAsAccountPassword = Read-Host -assecurestring "Enter password for $RunAsAccount"

$SBCertificateAutoGenerationKey = ConvertTo-SecureString -AsPlainText -Force -String 'devpassword'
$namespace = "roboleague"

try {
  $t = Get-SBFarm -SBFarmDBConnectionString $SBFarmConnString
  Write-Host "Farm already configured at $SBFarmConnString"
} catch {
  New-SBFarm -SBFarmDBConnectionString $SBFarmConnString `
             -CertificateAutoGenerationKey $SBCertificateAutoGenerationKey
}

try {
  Start-SBHost
  Write-Host "Host already connected to farm at $SBFarmConnString"
} catch {
  Add-SBHost -SBFarmDBConnectionString $SBFarmConnString `
             -EnableFirewallRules $true `
             -CertificateAutoGenerationKey $SBCertificateAutoGenerationKey `
             -RunAsPassword $RunAsAccountPassword
}

try {
  $t = Get-SBNameSpace -Name $namespace
  Write-Host "Namespace [$namespace] already exists"
} catch {
  $user = $(whoami)
  if ($user.StartsWith($([Environment]::MachineName))) {
    $user = [Environment]::UserName
  }
  New-SBNamespace -Name $namespace `
                  -AddressingScheme 'Path' `
                  -ManageUsers $([Environment]::UserName)
}
