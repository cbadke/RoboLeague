$ErrorActionPreference = "Stop"

pushd $PSScriptRoot

#Configure Service Bus
.\helpers\Install-FakeDll.ps1
.\helpers\Install-ServiceBus.ps1

popd

