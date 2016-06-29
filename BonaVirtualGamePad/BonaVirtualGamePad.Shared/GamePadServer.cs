﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class GamePadServer : GamePadNode
    {
        public IPEndPoint ListeningEndpoint { get; set; }
        public UdpClient UdpListener { get; set; }

        public PlayerClient[] Clients;          // Array of all currently connected players

        public GamePadServer(int capacity = Defaults.DEFAULT_SERVER_CAPACITY, int listeningPort = Defaults.DEFAULT_SERVER_PORT)
        {
            ListeningEndpoint = new IPEndPoint(IPAddress.Any, listeningPort);
            UdpListener = new UdpClient(ListeningEndpoint);

            Clients = new PlayerClient[capacity];
        }

        public void SendNetworkDiscoveryResponse(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            var package = CreateNetworkDiscoveryResponse(localEndpoint, this);
            SendPackage(package, UdpListener, remoteEndpoint);
        }

        public int GetPlayerCount()
        {
            var result = 0;

            for (int i = 0; i < Clients.Length; i++) {
                if (Clients[i] != null) {
                    result++;
                }
            }

            return result;
        }

        public int GetPlayerCapacity()
        {
            return Clients.Length;
        }
    }
}
