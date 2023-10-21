using SimpleFTP;
using System.Text;

var client = new Client(8888);

var message = new StringBuilder();

Task.Run(client.AcceptMessage);
await client.SendMessage();