using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Yesod_o_matic_30000.Model
{
    class MainWindowModel : ModelBase
    {
        private List<ShipStat> FullList = new List<ShipStat>();

        private Dictionary<string, ShipData> shipNames = new Dictionary<string, ShipData>();
        public MainWindowModel()
        {
            shipNames = getShipsDictionary();
            ShouldResetFilters = false;
            MinTier = 1;
            MaxTier = 10;
            DD = true;
            CA = true;
            BB = true;
            CV = true;
            Search = "";
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value.Trim());
        }

        private List<ShipStat> _shipStats;
        public List<ShipStat> ShipStats
        {
            get => _shipStats;
            set => SetProperty(ref _shipStats, value);
        }

        private bool _dd;
        public bool DD
        {
            get => _dd;
            set
            {
                SetProperty(ref _dd, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private bool _ca;
        public bool CA
        {
            get => _ca;
            set
            {
                SetProperty(ref _ca, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private bool _bb;
        public bool BB
        {
            get => _bb;
            set
            {
                SetProperty(ref _bb, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private bool _cv;
        public bool CV
        {
            get => _cv;
            set
            {
                SetProperty(ref _cv, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private int _minTier;
        public int MinTier
        {
            get => _minTier;
            set
            {
                SetProperty(ref _minTier, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private int _maxTier;
        public int MaxTier
        {
            get => _maxTier;
            set
            {
                SetProperty(ref _maxTier, value);
                var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = filteredList;
            }
        }

        private string _search;
        public string Search
        {
            get => _search;
            set {
                SetProperty(ref _search, value);
                var searchedList = FullList.Where(x => x.Ship.Contains(Search,StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
                ShipStats = searchedList;
                }
        }

        private bool _shouldResetFilters;
        public bool ShouldResetFilters
        {
            get => _shouldResetFilters;
            set => SetProperty(ref _shouldResetFilters, value);
        }

        private DelegateCommand _openFile;

        public DelegateCommand OpenFile
        {
            get
            {
                return _openFile ?? (_openFile = new DelegateCommand((x) =>
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Title = "Select the Ship data file";
                    if (dialog.ShowDialog() == true)
                    {
                        FileName = dialog.FileName;
                    }
                }));
            }
        }

        private DelegateCommand _readFile;

        public DelegateCommand ReadFile
        {
            get
            {
                return _readFile ?? (_readFile = new DelegateCommand((x) =>
                {
                    List<ShipStat> list = new List<ShipStat>();
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        if (IsLocalPath(FileName))
                        {
                            if (!File.Exists(FileName))
                            {
                                MessageBox.Show("File Path is not correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                using var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                                using var sr = new StreamReader(fs, Encoding.UTF8);

                                string line;

                                while ((line = sr.ReadLine()) != null)
                                {
                                    var data = line.Split("\t");
                                    var shipID = data[0];
                                    var shipData = shipNames[shipID];
                                    string name = shipData.Name;
                                    string players = data[1];
                                    if (!(name.Contains("[") || int.Parse(players) == 0 || name.Contains("(old)")))
                                    {
                                        string tier = shipData.Tier;
                                        string type = shipData.Type;
                                        string potential = (int.Parse(data[14]) + int.Parse(data[15])).ToString();
                                        ShipStat stats = new ShipStat(name, tier, type, players, data[2], data[4], data[13], data[8], data[9], data[10], data[11], data[12], data[7], potential, data[16]);
                                        list.Add(stats);
                                    }
                                }
                                FullList = list;
                                ResetFilters();
                                ShipStats = ApplyFilters();
                            }
                        }
                        else
                        {
                            try
                            {
                                var client = new WebClient();
                                using (var stream = client.OpenRead(FileName))
                                using (var reader = new StreamReader(stream))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        var data = line.Split("\t");
                                        var shipID = data[0];
                                        if (shipNames.ContainsKey(shipID))
                                        {
                                            var shipData = shipNames[shipID];
                                            string name = shipData.Name;
                                            string players = data[1];
                                            if (!(name.Contains("[") || int.Parse(players) == 0 || name.Contains("(old)")))
                                            {
                                                string tier = shipData.Tier;
                                                string type = shipData.Type;
                                                string potential = (int.Parse(data[14]) + int.Parse(data[15])).ToString();
                                                ShipStat stats = new ShipStat(name, tier, type, players, data[2], data[4], data[13], data[8], data[9], data[10], data[11], data[12], data[7], potential, data[16]);
                                                list.Add(stats);
                                            } 
                                        }
                                    }
                                }

                                FullList = list;
                                ResetFilters();
                                ShipStats = ApplyFilters();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("File URL is not correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }));
            }
        }

        private Dictionary<string, ShipData> getShipsDictionary()
        {
            Dictionary<string, ShipData> shipName = new Dictionary<string, ShipData>();
            var url = "http://maplesyrup.sweet.coocan.jp/wows/shipstats/res/mst_ships.txt";
            var client = new WebClient();
            using (var stream = client.OpenRead(url))
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split("\t");
                    ShipData shipData = new ShipData(data[1], data[2], data[4]);
                    shipNames.Add(data[0], shipData);
                }
            }
            return shipNames;
        }

        private bool IsLocalPath(string p)
        {
            try
            {
                return new Uri(p).IsFile;
            }
            catch
            {
                return true;
            }
        }

        private bool RespectClassFilter(string shipClass)
        {
            bool respect = false;
            if (DD)
            {
                respect = respect || shipClass.Equals("Destroyer");
            }
            if (CA)
            {
                respect = respect || shipClass.Equals("Cruiser");
            }
            if (BB)
            {
                respect = respect || shipClass.Equals("Battleship");
            }
            if (CV)
            {
                respect = respect || shipClass.Equals("AirCarrier");
            }
            return respect;
        }

        private void ResetFilters()
        {
            if (ShouldResetFilters)
            {
                Search = "";
                MinTier = 1;
                MaxTier = 10;
                DD = true;
                CA = true;
                BB = true;
                CV = true;
            }
        }

        private List<ShipStat> ApplyFilters()
        {
            var filteredList = FullList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
            var searchedList = filteredList.Where(x => x.Ship.Contains(Search, StringComparison.OrdinalIgnoreCase) && (x.Tier >= MinTier) && (x.Tier <= MaxTier) && RespectClassFilter(x.Class)).ToList();
            return searchedList;
        }
    }
}
