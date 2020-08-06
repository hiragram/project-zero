using System;
using System.Net;
using System.Net.Sockets;

public class UDPCommunication {

    private string hostName;
    private int remotePort;

    private int localPort;

    public UDPCommunication(string hostName, int remotePort, int localPort) {
        this.hostName = hostName;
        this.remotePort = remotePort;
        this.localPort = localPort;
    }

    public UdpClient MakeReceiverClient() {
        var address = IPAddress.Parse("127.0.0.1");
        var endpoint = new IPEndPoint(address, localPort);
        var client = new UdpClient(endpoint);

        return client;
    }
}
