using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthSharp.Models
{
    public class GatewayModel
    {
        public int ID { get; set; }

        public string GatewayName { get; set; }

        public long LastSystemUptime { get; set; }
        
        public long LastWifidogUptime { get; set; }

        public int LastFreeMemory { get; set; }

        public float LastSystemLoad { get; set; }

        public DateTime LastHeartBeat { get; set; }
    }
}