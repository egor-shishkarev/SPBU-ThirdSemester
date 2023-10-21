using System.Net.Sockets;

namespace SimpleFTP;

public class Client
{
    private int port;

    private NetworkStream stream;

    private StreamReader reader;

    private StreamWriter writer;

    private TcpClient tcpClient;

    public Client(int port)
    {
        this.port = port;

        tcpClient = new TcpClient();

        tcpClient.Connect("localhost", 8888);

        Console.WriteLine($"Connection has been established with 127.0.0.1 with port {port}");

        stream = tcpClient.GetStream();

        reader = new StreamReader(stream);

        writer = new StreamWriter(stream);
    }

    public async Task SendMessage()
    {
        while (true)
        {
            string? message = Console.ReadLine();
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
            Console.WriteLine(">");
        }
    }

    public async Task AcceptMessage()
    {
        while (true)
        {
            Console.WriteLine(">");
            string? message = await reader.ReadLineAsync();
            Console.WriteLine(message);
        }
    }

    public void Disconnect()
    {
        tcpClient.Close();
    }
}