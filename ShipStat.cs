using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Yesod_o_matic_30000
{
    public class ShipStat
    {

        public ShipStat(string ship, string tier, string shipClass, string players, string totalBattles, string averageWr, string averageXp,string averageDamage, string averageKills, string averagePlanes,
            string averageBaseCap, string averageBaseDef, string averageSR, string averagePotential, string averageSpotting)
        {
            Ship = ship;
            Tier = int.Parse(tier);
            Class = shipClass;
            Players = int.Parse(players);
            TotalBattles = int.Parse(totalBattles);
            AverageWr = Math.Round((double.Parse(averageWr, CultureInfo.InvariantCulture)) * 100, 2 );
            AverageXP = int.Parse(averageXp);
            AverageDamage = int.Parse(averageDamage);
            AverageKills = double.Parse(averageKills, CultureInfo.InvariantCulture);
            AveragePlanes = double.Parse(averagePlanes, CultureInfo.InvariantCulture);
            AverageBaseCap = double.Parse(averageBaseCap, CultureInfo.InvariantCulture);
            AverageBaseDef = double.Parse(averageBaseDef, CultureInfo.InvariantCulture);
            AverageSR = Math.Round(double.Parse(averageSR, CultureInfo.InvariantCulture) *100, 2);
            AveragePotential = int.Parse(averagePotential);
            AverageSpotting = int.Parse(averageSpotting);
        }

        public string Ship { get; }
        public int Tier { get; }
        public string Class { get; }
        public int Players { get; }
        public int TotalBattles { get; }
        public double AverageWr { get; }
        public int AverageXP { get; }
        public int AverageDamage { get; }
        public double AverageKills { get; }
        public double AveragePlanes { get; }
        public double AverageBaseCap { get; }
        public double AverageBaseDef { get; }
        public double AverageSR { get; }
        public int AveragePotential { get; }
        public int AverageSpotting { get; }
    }
}