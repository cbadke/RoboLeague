namespace RunsGames

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open System.Net
open System.Threading
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.Diagnostics
open Microsoft.WindowsAzure.ServiceRuntime

type WorkerRole() =
    inherit RoleEntryPoint() 

    // This is a sample worker implementation. Replace with your logic.

    let log message (kind : string) = Trace.TraceInformation(message, kind)
    let HttpPort = 9355
    let TcpPort = 9354
    let _serverFqdn = System.Environment.MachineName
    let ServiceNamespace = "roboleague"
    let queueName = "games"

    let generateConnectionString() =
        let connectionStringBuilder = new ServiceBusConnectionStringBuilder (ManagementPort = HttpPort, RuntimePort = TcpPort)
        connectionStringBuilder.Endpoints.Add((new System.UriBuilder (Scheme = "sb", Host = _serverFqdn, Path = ServiceNamespace)).Uri) |> ignore
        connectionStringBuilder.StsEndpoints.Add((new System.UriBuilder (Scheme = "https", Host = _serverFqdn, Port = HttpPort, Path = ServiceNamespace)).Uri) |> ignore
        connectionStringBuilder.ToString()
      
    override wr.Run() =

        log "RunsGames entry point called" "Information"
        let messageFactory = MessagingFactory.CreateFromConnectionString(generateConnectionString());
        let queueClient = messageFactory.CreateQueueClient(queueName)

        while(true) do 
            log "Working" "Information"
            // Receive the message from the queue 
            let receivedMessage = queueClient.Receive(); 
            if (receivedMessage <> null)  then
                log ("Message received: " + receivedMessage.GetBody<String>()) "Information"
                receivedMessage.Complete()
            Thread.Sleep(5000)
            


    override wr.OnStart() = 

        // Set the maximum number of concurrent connections 
        ServicePointManager.DefaultConnectionLimit <- 12
       
        // For information on handling configuration changes
        // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

        let namespaceManager = NamespaceManager.CreateFromConnectionString(generateConnectionString());
        if (not(namespaceManager.QueueExists(queueName))) then
            namespaceManager.CreateQueue(queueName) |> ignore

        base.OnStart()
