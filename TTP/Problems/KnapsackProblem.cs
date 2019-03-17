using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TTP.Entities;

namespace TTP
{
    public class KnapsackProblem : IKnapsackProblem
    {
        public Knapsack GetMaximumProfit(int knapsackCapacity, IList<Item> items)
        {
            Knapsack knapsack = new Knapsack(knapsackCapacity);

            int currentWeight = 0;
            List<Item> bestItemsInCities = new List<Item>();
            var itemsInCities = items.GroupBy(x => x.AssignedNode);
            foreach (var city in itemsInCities)
            {
                bestItemsInCities.Add(city.OrderByDescending(x => (double) x.Profit / (double) x.Weight).First(
                    ));
            }
            bestItemsInCities = bestItemsInCities.OrderByDescending(x => (double) x.Profit / (double) x.Weight).ToList();

            for (int i = 0; i < bestItemsInCities.Count || currentWeight == knapsackCapacity; i++)
            {
                if (currentWeight + bestItemsInCities[i].Weight < knapsackCapacity)
                {
                    knapsack.Items.Add(bestItemsInCities[i]);
                    currentWeight += bestItemsInCities[i].Weight;
                }
            }
            return knapsack;
        }
    }
}