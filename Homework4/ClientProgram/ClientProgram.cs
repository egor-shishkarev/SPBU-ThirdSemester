using System.Net.Sockets;

string host = "127.0.0.1";
int port = 8888;
using TcpClient client = new();
StreamReader? reader = null;
StreamWriter? writer = null;

try
{
    client.Connect(host, port);
    reader = new StreamReader(client.GetStream());
    writer = new StreamWriter(client.GetStream());
    if (writer == null || reader == null) return;
    Task.Run(() => ReceiveMessageAsync(reader));
    await SendMessageAsync(writer);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
writer?.Close();
reader?.Close();

async Task SendMessageAsync(StreamWriter writer)
{
    while (true)
    {
        string? message = Console.ReadLine();
        if (string.IsNullOrEmpty(message) || message == "\n") continue;
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}

async Task ReceiveMessageAsync(StreamReader reader)
{
    while (true)
    {
        try
        {
            string? message = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) continue;
            Console.WriteLine(message);
        }
        catch
        {
            break;
        }
    }
}

