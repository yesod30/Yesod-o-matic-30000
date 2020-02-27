using System;
using System.Collections.Generic;
using System.Text;

namespace Yesod_o_matic_30000
{
    public class ShipData
    {
        public ShipData(string name, string type, string tier)
        {
            Name = name;
            Tier = tier;
            Type = type;
        }

        public string Name { get; }
        public string Tier { get; }
        public string Type { get; }
    }
}
