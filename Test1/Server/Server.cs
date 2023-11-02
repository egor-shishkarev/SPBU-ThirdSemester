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
            while (true)
            {
                client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Connected to the client");
                stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                Task.Run(SendMessage);
                await ReceiveMessage();
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

    private async Task ReceiveMessage()
    {
        while (true)
        {
            var message = await reader.ReadLineAsync();
            if (message == "exit")
            {
                //Disconnect();
                Environment.Exit(0);
            }
            Console.WriteLine($"{message}");
        }
    }

    private async Task SendMessage()
    {
        while (true)
        {
            var message = Console.ReadLine();
            if (message == "exit")
            {
                //Disconnect();
                Environment.Exit(0);
            }
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }
    }

    private void Disconnect()
    {
        writer.Close();
        reader.Close();
        client.Close();
    }
}