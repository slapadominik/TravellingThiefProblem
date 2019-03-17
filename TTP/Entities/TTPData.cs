using System.Collections.Generic;
using TTP.Entities;

namespace TTP.Entities
{
    public class TTPData
    {
        public string ProblemName { get; set; }
        public string KnapsackDataType { get; set; }
        public int Dimension { get; set; }
        public int KnapsackCapacity { get; set; }
        public double MinSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double RentingRatio { get; set; }
        public List<City> Cities { get; set; }
        public List<Item> Items { get; set; }

        public TTPData()
        {
            Cities= new List<City>();
            Items = new List<Item>();
        }
    }
}