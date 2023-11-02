namespace SimpleFTP.Tests;

public class SimpleFTPTests
{
    private Client client;

    private Server server;

    [SetUp]
    public void SetUp()
    {
        server = new Server(8888);
        server.Start();
        client = new Client("localhost", 8888);
    }

    [Test]
    public void ServerShouldRequestExpectedToUnknownRequest()
    {
        var request = "How are you?";
        var response = client.Request(request);

        Assert.That(response, Is.EqualTo(null));
    }

    [Test]
    public void ServerShouldRequestExpectedToListTest()
    {
        var request = "1 ../";
        var response = client.Request(request).Result;

        Assert.That(response, Is.EqualTo("1 ../net7.0 true"));
    }

    [Test] 
    public void ServerShouldRequestExpectedToGetTest()
    {
        var path = "../../../../Client.Tests/TestFiles/File.txt";
        var response = client.Request("2 " + path).Result;
        var fileContent = $"{File.ReadAllBytes(path).Length} {File.ReadAllText(path)}";

        Assert.That(response, Is.EqualTo(fileContent));
    }
}