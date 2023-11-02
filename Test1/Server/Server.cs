using System.Net;
using System.Net.Sockets;

namespace Chat;

public class Server
{
    private int port;

    private TcpListener listener;

    private NetworkStream stream;

    private StreamReader reader;

    private StreamWriter writer;

    private TcpClient client;

    public Server(int port)
    {
        this.port = port;
        listener = new TcpListener(IPAddress.Any, port);

    }

    public async Task Start()
    {
        try
        {
            listener.Start();
            Console.WriteLine("Server is working, waiting for clients...");
            client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Connected to the client");
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            while (true)
            {
                Task.Run(async () => Console.WriteLine($"Client: {await ReceiveMessage()}"));
                Task.Run(() => SendMessage(Console.ReadLine()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    public async Task<string> ReceiveMessage()
    {
        var message = await reader.ReadLineAsync();
        if (message == "exit")
        {
            Disconnect();
            Environment.Exit(0);
        }
        return message;
    }

    public async Task SendMessage(string message)
    {
        if (message == "exit")
        {
            Disconnect();
            Environment.Exit(0);
        }
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }

    private void Disconnect()
    {
        writer.Close();
        reader.Close();
        client.Close();
    }
}