using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public enum Protocol
    {
        Disconnected=0,
        Connected=1,
        PlayerMoved,
        PlayerShot,
        Respawn,
        Death,
        Item
    }
}
