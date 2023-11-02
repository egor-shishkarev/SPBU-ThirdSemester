using SimpleFTP;

try
{
    var client = new Client("127.0.0.1", 8888);
    while (true)
    {
        var message = Console.ReadLine();
        var request = client.Request(message);
        if (request == null)
        {
            Console.WriteLine("Bad request");
            continue;
        }
        Console.WriteLine(request.Result);
    }
}

catch (Exception e)
{
    Console.WriteLine(e.Message);
}


