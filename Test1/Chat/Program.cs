/*Реализовать консольный сетевой чат.

Это должно быть одно приложение, которое в зависимости от опций командной строки запускается либо как клиент, либо как сервер
Если указан только порт, приложение запускается как сервер и слушает порт
Если указан IP-адрес и порт, приложение запускается как клиент и коннектится к указанному серверу
Как только соединение установлено, и клиент, и сервер могут писать друг другу сообщения в консоли, независимо друг от друга
Когда кто-либо набирает в консоли “exit”, соединение закрывается и приложения заканчивают работу.
Нужны юнит-тесты*/

// "C:\Users\Егор\source\repos\SPBU-ThirdSemester\Test1\Chat\bin\Debug\net7.0\Chat.exe"
using Chat;


if (args.Length < 1)
{
    Console.WriteLine("Недостаточно параметров");
    return -1;
}

if (args.Length == 1)
{
    if (Int32.TryParse(args[0], out int port))
    {
        if (0 < port && port < 63536)
        {
            Server server = new Server(port);
            await server.Start();
        }
    }
}


if (args.Length == 2)
{
    if (Int32.TryParse(args[0], out int port))
    {
        if (0 < port && port < 63536)
        {
            Client client = new Client(port, args[1]);
            await client.Start();
        }
    }
}



return 0;