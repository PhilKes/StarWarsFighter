﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace StarWarsFighter
{
    public delegate void ConnectionEvent(object sender, TcpClient user);
    public delegate void DataReceivedEvent(TcpClient sender, byte[] data);
}
