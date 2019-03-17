using System.Collections.Generic;
using System.Linq;

namespace TTP.Entities
{
    public class Knapsack
    {
        public IList<Item> Items { get; set; }
        public int Capacity { get; }

        public Knapsack(int capacity)
        {
            Items = new List<Item>();
            Capacity = capacity;
        }

        public int GetProfit()
        {
            return Items.Sum(x => x.Profit);
        }

        public int GetWeight()
        {
            return Items.Sum(x => x.Weight);
        }
    }
}