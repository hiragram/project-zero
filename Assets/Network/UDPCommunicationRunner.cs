using System;

class UDPCommunicationRunner {
    static void Main(string[] args) {
        var communicator = new UDPCommunication("192.168.10.122", 35001, "192.168.10.115", 35002);
        communicator.StartReceiving((text) => {
            Console.WriteLine(text);
        });

        // var interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

        // foreach(var nic in interfaces) {
        //     // Console.WriteLine("{0}: {1}", nic.Name, );
        //     var props = nic.GetIPProperties();

        //     Console.WriteLine(nic.Name);
        //     foreach(var address in props.UnicastAddresses) {
        //         Console.WriteLine(address.Address.ToString());
        //     }
        // }
    }

}