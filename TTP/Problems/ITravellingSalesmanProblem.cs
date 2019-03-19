using System.Collections.Generic;
using TTP.Entities;

namespace TTP
{
    public interface ITravellingSalesmanProblem
    {
        TSPIndividual Initialize(List<City> cities);
        double Fitness(double maxSpeed, double minSpeed, TSPIndividual individual, Knapsack knapsack);
        void Mutation(TSPIndividual tspEntity);
        TSPIndividual Crossover(TSPIndividual child1, TSPIndividual child2);
        List<TSPIndividual> Selection(List<TSPIndividual> population, int individualsPerTorunament);
        (TSPIndividual bestIndividual, IEnumerable<TSPPopulationStatistics> statistics) ResolveProblem(TTPData initData, Knapsack knapsack);
    }
}