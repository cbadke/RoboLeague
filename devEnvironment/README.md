# Dev Environment Setup

Run `setup.ps1` to configure your dev environment.

You must run this script as a local administrator. If your machine is part of a
workgroup, your user must be a local user. If your machine is part of a
domain, your user must be a domain user. If you are using Windows 10 with an
AzureAD or LiveID user then your machine is most likely on a workgroup.

These scripts will:

1. Configure a local Service Bus farm and namespace 'Roboleague'
