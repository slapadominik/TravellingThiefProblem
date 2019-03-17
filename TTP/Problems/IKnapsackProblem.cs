using System.Collections.Generic;
using TTP.Entities;

namespace TTP
{
    public interface IKnapsackProblem
    {
        Knapsack GetMaximumProfit(int knapsackCapacity, IList<Item> items);
    }
}