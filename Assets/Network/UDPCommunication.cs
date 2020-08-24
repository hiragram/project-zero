using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPCommunication {

    private string hostName;
    private int remotePort;

    private UdpClient receiverClient;

    public UDPCommunication(string hostName, int remotePort, string localAddress, int localPort) {
        this.hostName = hostName;
        this.remotePort = remotePort;

        this.receiverClient = MakeReceiverClient(localAddress, localPort);
    }

    private UdpClient MakeReceiverClient(string localAddress, int localPort) {
        var address = IPAddress.Parse(localAddress);
        var endpoint = new IPEndPoint(address, localPort);
        var client = new UdpClient(endpoint);

        return client;
    }

    public void StartReceiving(Action<string> callback) {
        IPEndPoint remote = null;

        string receivedText;
        do {
            var receivedBytes = receiverClient.Receive(ref remote);
            receivedText = Encoding.UTF8.GetString(receivedBytes);
            callback(receivedText);
        } while(receivedText != "exit");

        receiverClient.Close();
    }
}
