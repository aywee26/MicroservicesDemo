using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private const string _exchange = "trigger";
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: _exchange,
                type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += (sender, args) 
                => Console.WriteLine("RabbitMQ connection shutdown.");
            Console.WriteLine("Connected to Message Bus.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);
        if (_connection.IsOpen)
        {
            Console.WriteLine("RabbitMQ connection is open, sending a message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("RabbitMQ connection is not open.");
        }
    }

    public void Dispose()
    {
        Console.WriteLine("Disposing Message Bus");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            exchange: _exchange,
            routingKey: "",
            basicProperties: null,
            body: body);
        Console.WriteLine($"Message is sent: {message}");

    }
}
