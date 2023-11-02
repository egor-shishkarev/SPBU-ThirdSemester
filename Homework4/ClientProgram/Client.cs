using System.Net.Sockets;
using System.Text;

namespace SimpleFTP;

public class Client
{
    private TcpClient? client;

    private NetworkStream? stream;

    public Client(string host, int port)
    {
        try
        {
            client = new TcpClient(host, port);
            Console.WriteLine("You connected to server");
            stream = client.GetStream();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public Task<string> Request(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }
        var parsedData = message.Split();

        switch(parsedData[0])
        {
            case "1":
                {
                    return ListRequest(message);
                }
            case "2":
                {
                    return GetRequest(message);
                }
            default:
                {
                    return null;
                }
        }
    }

    private async Task<string> ListRequest(string message)
    {
        await stream.WriteAsync(Encoding.UTF8.GetBytes(message + '\n'));
        await stream.FlushAsync();

        var buffer = new List<byte>();
        var byteReaded = 10;
        while ((byteReaded = stream.ReadByte()) != '\n')
        {
            buffer.Add((byte)byteReaded);
        }

        var response = Encoding.UTF8.GetString(buffer.ToArray());
        return response;
    }

    private async Task<string> GetRequest(string message)
    {
        await stream.WriteAsync(Encoding.UTF8.GetBytes(message + '\n'));
        await stream.FlushAsync();

        var sizeBuffer = new byte[10];
        await stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length);
        Int32.TryParse(Encoding.UTF8.GetString(sizeBuffer),out int size);
        if (size == -1)
        {
            return "-1";
        }

        var data = new byte[size];
        long bytes = await stream.ReadAsync(data);
        var response = $"{size} {Encoding.UTF8.GetString(data, 0, (int)bytes)}";
        return response;
    }

}