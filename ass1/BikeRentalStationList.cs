using System;
using System.Collections.Generic;

namespace ass1

{
    [Serializable]
    public class BikeRentalStationList
    {
        public List<BikeRentalStation> stations;
    }

    [Serializable]
    public class BikeRentalStation
    {
        public int id;
        public string name;
        public float x;
        public float y;
        public int bikesAvailable;
        public int spacesAvailable;
        public bool allowDropoff;
        public bool isFloatingBike;
        public bool isCarStation;
        public string state;
        public List<string> networks;
        bool realTimeData;

    }
}