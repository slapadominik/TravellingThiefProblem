using System.Collections.Generic;
using System.Linq;

namespace TTP.Entities
{
    public class Knapsack
    {
        public IList<Item> Items { get; set; }
        public int Capacity { get; }

        public int Profit
        {
            get { return Items.Sum(x => x.Profit); }
        }

        public int Weight
        {
            get { return Items.Sum(x => x.Weight); }
        }

        public Knapsack(int capacity)
        {
            Items = new List<Item>();
            Capacity = capacity;
        }
    }
}