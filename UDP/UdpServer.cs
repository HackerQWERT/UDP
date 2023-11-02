using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpServer
{
    private UdpClient udpClient;
    private IPEndPoint iPEndPoint;

    CancellationTokenSource receiveMessageCancellationTokenSource;

    public UdpServer(int port)
    {
        udpClient = new UdpClient(port);
        iPEndPoint = new IPEndPoint(IPAddress.Any, port);
        receiveMessageCancellationTokenSource = new CancellationTokenSource();
    }

    public async void Start()
    {
        while (true)
        {
            try
            {
                UdpReceiveResult receiveResult = await udpClient.ReceiveAsync();
                string receiveMessage = Encoding.UTF8.GetString(receiveResult.Buffer);
                Console.WriteLine($"Server Received message: {receiveMessage}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public async Task SendAsync(string message, IPEndPoint remoteEP)
    {
        // Echo the message back to the client
        byte[] sendBytes = Encoding.UTF8.GetBytes(message);
        await udpClient.SendAsync(sendBytes, sendBytes.Length, remoteEP);
    }

    public void Stop()
    {
        udpClient.Close();
    }
}