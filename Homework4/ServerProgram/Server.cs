using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleFTP;

public class Server
{
    private TcpListener listener;

    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task Start()
    {
        try
        {
            listener.Start();
            Console.WriteLine("Server is working. Waiting for clients...");
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected");
                Task.Run(() => ProcessAsync(client));
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task ProcessAsync(TcpClient client)
    {
        var stream = client.GetStream();
        while (true)
        {
            var buffer = new List<byte>();
            var byteReaded = 10;
            while ((byteReaded = stream.ReadByte()) != '\n')
            {
                buffer.Add((byte)byteReaded);
            }
            var message = Encoding.UTF8.GetString(buffer.ToArray());
            buffer.Clear();

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"Received data from {client.Client.RemoteEndPoint}: {message}");
            }

            var parsedData = message.Split();
            if (parsedData.Length != 2)
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes("-1 \n"));
                await stream.FlushAsync();
                continue;
            }
            switch (parsedData[0]) 
            {
                case "1":
                {
                    await ListResponse(parsedData[1], stream);
                    break;
                }
                case "2":
                {
                    await GetReponse(parsedData[1], stream);
                    break;
                }
                default:
                {
                    await stream.WriteAsync(Encoding.UTF8.GetBytes("-1 \n"));
                    await stream.FlushAsync();
                    break;
                }
            }
        }
    }

    private async Task ListResponse(string path, NetworkStream stream)
    {
        if (!Directory.Exists(path))
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes("-1 \n"));
            await stream.FlushAsync();
            return;
        }
        var listOfFiles = Directory.GetFiles(path);

        var listOfDirectories = Directory.GetDirectories(path);

        var count = listOfFiles.Length + listOfDirectories.Length;

        var response = new StringBuilder();
        response.Append(count.ToString());

        foreach (var directory in listOfDirectories)
        {
            response.Append(" " + directory + " true");
        }

        foreach (var file in listOfFiles)
        {
            response.Append(" " + file + " false");
        }

        response.Append(" \n");
        await stream.WriteAsync(Encoding.UTF8.GetBytes(response.ToString()));
        await stream.FlushAsync();
    }

    private async Task GetReponse(string path, NetworkStream stream)
    {
        if (!File.Exists(path))
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes("-1 \n"));
            await stream.FlushAsync();
            return;
        }
        
        var fileInfo = new FileInfo(path);
        await stream.WriteAsync(Encoding.UTF8.GetBytes($"{fileInfo.Length} "));
        await stream.FlushAsync();

        var content = File.ReadAllText(path);
        await stream.WriteAsync(Encoding.UTF8.GetBytes(content));
        await stream.FlushAsync();

    }
}
