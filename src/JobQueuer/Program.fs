// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open System.Diagnostics

let log message (kind : string) = Trace.TraceInformation(message, kind)

[<EntryPoint>]
let main argv = 
    let HttpPort = 9355
    let TcpPort = 9354
    let _serverFqdn = System.Environment.MachineName
    let ServiceNamespace = "roboleague"
    let connectionStringBuilder = new ServiceBusConnectionStringBuilder (ManagementPort = HttpPort, RuntimePort = TcpPort)

    connectionStringBuilder.Endpoints.Add((new System.UriBuilder (Scheme = "sb", Host = _serverFqdn, Path = ServiceNamespace)).Uri) |> ignore
    connectionStringBuilder.StsEndpoints.Add((new System.UriBuilder (Scheme = "https", Host = _serverFqdn, Port = HttpPort, Path = ServiceNamespace)).Uri) |> ignore

    let queueName = "games"
    let messageFactory = MessagingFactory.CreateFromConnectionString(connectionStringBuilder.ToString());
    let namespaceManager = NamespaceManager.CreateFromConnectionString(connectionStringBuilder.ToString());
 
    if (not(namespaceManager.QueueExists(queueName))) then
        namespaceManager.CreateQueue(queueName) |> ignore

    let queueClient = messageFactory.CreateQueueClient(queueName)
    let payload = "Hello world"
    let sendMessage = new BrokeredMessage(payload)
    queueClient.Send(sendMessage);   
    log ("Sending Message " + payload) "Information"

    0 // return an integer exit code
