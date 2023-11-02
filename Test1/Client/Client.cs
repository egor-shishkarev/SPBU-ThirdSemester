using System.Net.Sockets;
using System.Reflection;

namespace Chat;

public class Client
{
    private int port;

    private string host;

    private TcpClient client;

    private NetworkStream stream;

    private StreamReader reader;

    private StreamWriter writer;

    public Client(int port, string host)
    {
        this.port = port;
        this.host = host;
    }

    public async Task Start()
    {
        try
        {
            client = new TcpClient(host, port);
            stream = client.GetStream();
            Console.WriteLine("Connected to the server");
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            Task.Run(SendMessage);
            await ReceiveMessage();
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
        reader.Close();
        writer.Close();
        client.Close();
    }
}