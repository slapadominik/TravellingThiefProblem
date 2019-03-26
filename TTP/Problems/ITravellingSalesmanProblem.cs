using System.Collections.Generic;
using TTP.Entities;

namespace TTP
{
    public interface ITravellingSalesmanProblem
    {
        (TSPIndividual bestIndividual, IEnumerable<TSPPopulationStatistics> statistics) ResolveProblem(TTPData initData, Knapsack knapsack);
    }
}