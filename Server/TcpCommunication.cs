using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class TcpCommunication
    {
        public class Client
        {
            private const int BufferSize = 1024;
            private readonly string _serverIpAddress;
            private int _serverPort;
        
            private TcpClient _client;
            private NetworkStream _stream;
            private byte[] _buffer;

            public Client(string ip, int port)
            {
                _serverIpAddress = ip;
                _serverPort = port;
            }
            
            public void Connect()
            {
                try
                {
                    _client = new TcpClient();
                    _client.Connect(IPAddress.Parse(_serverIpAddress), _serverPort);
                    _stream = _client.GetStream();
                    _buffer = new byte[BufferSize];
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to connect to the server.", ex);
                }
            }
            
            public void SendMessage(string s)
            {
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(s);
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send pairs to the server.", ex);
                }
            }
            
            public void SendPos(Position pos1, Position pos2)
            {
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(pos1.ToString() + " " + pos2.ToString());
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send pairs to the client.", ex);
                }
            }
            
            public string ReceiveMessage()
            {
                try
                {
                    int bytesRead = _stream.Read(_buffer, 0, BufferSize);
                    string message = Encoding.ASCII.GetString(_buffer, 0, bytesRead);
                    return message;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to receive pairs from the server.", ex);
                }
            }

            public (Position, Position) RecievePos()
            {
                try
                {
                    int bytesRead = _stream.Read(_buffer, 0, BufferSize);
                    var message = Encoding.ASCII.GetString(_buffer, 0, bytesRead).Split(" ");
                    return (new Position(int.Parse(message[0]), int.Parse(message[1])),
                        new Position(int.Parse(message[2]), int.Parse(message[3])));
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to receive pairs from the server.", ex);
                }
            }
            
            public void Close()
            {
                try
                {
                    _stream.Close();
                    _client.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to close the client connection.", ex);
                }
            }
        }
        
        
        public class Server
        {
            private const int BufferSize = 1024;
            private int _serverPort;
        
            private TcpListener _listener;
            private TcpClient _client;
            private NetworkStream _stream;
            private byte[] _buffer;
            
            public Server(int port)
            {
                _serverPort = port;
            }
            
            public void Start()
            {
                try
                {
                    _listener = new TcpListener(IPAddress.Any, _serverPort);
                    _listener.Start();
                    _client = _listener.AcceptTcpClient();
                    _stream = _client.GetStream();
                    _buffer = new byte[BufferSize];
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to start the server.", ex);
                }
            }
            
            public string ReceiveMessage()
            {
                try
                {
                    int bytesRead = _stream.Read(_buffer, 0, BufferSize);
                    string message = Encoding.ASCII.GetString(_buffer, 0, bytesRead);
                    return message;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to receive pairs from the client.", ex);
                }
            }
            
            public (Position, Position) RecievePos()
            {
                try
                {
                    int bytesRead = _stream.Read(_buffer, 0, BufferSize);
                    var message = Encoding.ASCII.GetString(_buffer, 0, bytesRead).Split(" ");
                    return (new Position(int.Parse(message[0]), int.Parse(message[1])),
                        new Position(int.Parse(message[2]), int.Parse(message[3])));
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to receive pairs from the server.", ex);
                }
            }
            
            public void SendMessage(string s)
            {
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(s);
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send pairs to the client.", ex);
                }
            }

            public void SendPos(Position pos1, Position pos2)
            {
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(pos1.ToString() + " " + pos2.ToString());
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send pairs to the client.", ex);
                }
            }
            
            public void Stop()
            {
                try
                {
                    _stream.Close();
                    _client.Close();
                    _listener.Stop();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to stop the server.", ex);
                }
            }
        }
    }

}