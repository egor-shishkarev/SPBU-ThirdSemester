namespace Chat.Tests;

public class ChatTests
{
    private Server server;

    private Client client;

    [SetUp]
    public void Setup()
    {
        server = new Server(8888);
        server.Start();
        client = new Client(8888, "localhost");
    }

    [Test]
    public void SendMessageToServerShouldWorkTest()
    {
        var message = "";
        Task.Run(async () => {
            await client.SendMessage("Hello");
            message = await server.ReceiveMessage();
            
        });

        Task.WaitAll();

        Assert.That(message, Is.EqualTo("Hello"));
    }
}