using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleFTP;

public class Server
{
    private int port;

    private readonly TcpListener tcpListener;

    private List<Client> clients;
    public Server(int port)
    {
        this.port = port;

        tcpListener = new TcpListener(IPAddress.Any, this.port);

        clients = new List<Client>();
    }

    protected internal void RemoveConnection(string id)
    {
        Client? client = clients.FirstOrDefault(c => c.Id == id);
        if (client != null) clients.Remove(client);
        client?.Disconnect();
    }

    public async Task Start()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Server is working. Waiting for clients...");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                Client newClient = new Client(tcpClient, this);

                Console.WriteLine($"Connection has been established with client {tcpClient.Client.RemoteEndPoint}");

                clients.Add(newClient);

                Task.Run(newClient.ProcessAsync);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    protected internal async Task SendMessage(string message, string id)
    {
        var client = clients.Find(c => c.Id == id);
        if (client != null)
        {
            await client.writer.WriteLineAsync(message);
            await client.writer.FlushAsync();
        }
    }

    protected internal async Task SendResponse(string message, string id)
    {
        Console.WriteLine($"Received message: {message}");
        var client = clients.Find(c => c.Id == id);
        if (client != null)
        {
            var writer = client.writer;
            var parsedData = message.Split();
            if (parsedData.Length != 2)
            {
                await writer.WriteLineAsync("Bad request!");
                await writer.FlushAsync();
            }
            else if (parsedData[0] == "1")
            {
                var path = parsedData[1];
                if (!Directory.Exists(path))
                {
                    await writer.WriteAsync("-1 \n");
                    await writer.FlushAsync();
                }
                else
                {
                    var listOfFiles = Directory.GetFiles(path);

                    var listOfDirectories = Directory.GetDirectories(path);

                    await writer.WriteAsync($"{listOfFiles.Length + listOfDirectories.Length} ");

                    foreach (var element in listOfFiles)
                    {
                        await writer.WriteAsync(element.Replace("\\", "/") + " false ");
                    }

                    foreach (var element in listOfDirectories)
                    {
                        await writer.WriteAsync(element.Replace("\\", "/") + " true ");
                    }

                    await writer.WriteAsync("\n");
                    await writer.FlushAsync();
                }
            }
            else if (parsedData[0] == "2")
            {
                if (!File.Exists(parsedData[1]))
                {
                    await writer.WriteLineAsync("-1");
                    await writer.FlushAsync();
                }
                else
                {
                    var bytes = File.ReadAllText(parsedData[1]);
                    await writer.WriteAsync(new FileInfo(parsedData[1]).Length.ToString() + " " + bytes + "\n");
                    await writer.FlushAsync();
                }
            }
            else
            {
                await writer.WriteLineAsync("-1");
            }
        }
    }

    private void Disconnect()
    {
        foreach (var client in clients) 
        {
            client.Disconnect();
        }
    }
}

public class Client
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();

    protected internal StreamReader reader;

    protected internal StreamWriter writer;

    protected internal TcpClient tcpClient;

    private Server server;

    public Client(TcpClient tcpClient, Server server)
    {

        this.tcpClient = tcpClient;

        this.server = server;

        var stream = tcpClient.GetStream();

        reader = new StreamReader(stream);

        writer = new StreamWriter(stream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            await server.SendMessage("You connected to server", this.Id);

            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (message == null) continue;
                    await server.SendResponse(message, Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"The client {tcpClient.Client.RemoteEndPoint} disconnected from the server.");
                    break;
                }
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            server.RemoveConnection(Id);
        }
    }

    public void Disconnect()
    {
        tcpClient.Close();
        writer.Close();
        reader.Close();
    }
}
