using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using CsvHelper;
using TTP.Entities;
using TTP.Extensions;

namespace TTP
{
    public class TravellingSalesmanProblemGeneticAlgorithm : ITravellingSalesmanProblem
    {
        private readonly Random _randomGenerator;
        private const int INIT_POPULATION_NUMBER = 1;

        public TravellingSalesmanProblemGeneticAlgorithm()
        {
            _randomGenerator = new Random();
        }

        public TSPIndividual Initialize(List<City> cities)
        {
            return new TSPIndividual(cities.Shuffle(), INIT_POPULATION_NUMBER);
        }

        public double Fitness(double maxSpeed, double minSpeed, TSPIndividual individual, Knapsack knapsack)
        {
            var speeds = CalculateSpeeds(maxSpeed, minSpeed, individual, knapsack);
            var routeTime = CalculateRouteTime(individual, speeds);
            return knapsack.Profit - routeTime;
        }

        public void Mutation(TSPIndividual tspEntity)
        {
            tspEntity.Route.Swap();
        }

        public TSPIndividual Crossover(TSPIndividual parent1, TSPIndividual parent2)
        {
            List<City> childRoute = new List<City>();
            if (parent1.Route.Count != parent2.Route.Count)
            {
                throw new InvalidOperationException($"Crossover is not possible for child1 {parent1.Route.Count}, child2 {parent2.Route.Count}");
            }

            var splitPoint = _randomGenerator.Next(0, parent1.Route.Count);
            childRoute.AddRange(parent1.Route.GetRange(0, splitPoint).ToList());
            childRoute.AddRange(parent2.Route.GetRange(splitPoint, parent2.Route.Count-splitPoint));

            TSPIndividual child = new TSPIndividual(childRoute, parent1.PopulationNumber+1);
            child.RepairRoute(parent1, parent2);

            return child;
        }

        public List<TSPIndividual> Selection(List<TSPIndividual> population, int individualsPerTournament)
        {
            List<TSPIndividual> individuals = new List<TSPIndividual>();

            var tournamentGroups = population.SplitList(individualsPerTournament);
            foreach (var tournamentGroup in tournamentGroups)
            {
                individuals.Add(RunTournament(tournamentGroup));
            }

            return individuals;
        }

        public (TSPIndividual bestIndividual, IEnumerable<TSPPopulationStatistics> statistics) ResolveProblem(TTPData initData, Knapsack knapsack)
        {
            var tspStatistics = new List<TSPPopulationStatistics>();
            var population = new List<TSPIndividual>();

            for (int i = 0; i < initData.PopulationSize; i++)
            {
                population.Add(Initialize(initData.Cities));
                population[i].Quality = Fitness(initData.MaxSpeed, initData.MinSpeed, population[i], knapsack);
            }

            var qualityDesc = population.OrderByDescending(x => x.Quality);
            tspStatistics.Add(new TSPPopulationStatistics{ PopulationNumber = qualityDesc.First().PopulationNumber, AverageQuality = population.Average(x => x.Quality), BestQuality = qualityDesc.First().Quality, WorstQuality = qualityDesc.Last().Quality});

            for (int i = 0; i < initData.GenerationNumber-1; i++)
            {
                var nextGeneration = new List<TSPIndividual>();
                var selectedIndividuals = Selection(population, initData.IndividualsPerTournament);
                while (nextGeneration.Count!=initData.PopulationSize)
                {
                    nextGeneration.Add(
                        Crossover(selectedIndividuals[_randomGenerator.Next(selectedIndividuals.Count)], 
                            selectedIndividuals[_randomGenerator.Next(selectedIndividuals.Count)])
                        );
                }

                foreach (var individual in nextGeneration)
                {
                    if (initData.MutationProbability <= _randomGenerator.NextDouble())
                    {
                        Mutation(individual);
                    }
                    individual.Quality = Fitness(initData.MaxSpeed, initData.MinSpeed, individual, knapsack);
                }

                population = nextGeneration;
                var averageQuality = population.Average(x => x.Quality);
                var populationOrderedByQualityDesc = population.OrderByDescending(x => x.Quality);                
                tspStatistics.Add(new TSPPopulationStatistics{PopulationNumber = populationOrderedByQualityDesc.First().PopulationNumber, BestQuality = populationOrderedByQualityDesc.First().Quality, AverageQuality = averageQuality, WorstQuality = populationOrderedByQualityDesc.Last().Quality});
            }

            return (population.OrderByDescending(x => x.Quality).First(), tspStatistics);
        }

        private double CalculateRouteTime(TSPIndividual tspEntity, IList<double> speeds)
        {
            double time = 0;
            for (int i = 0; i < tspEntity.Route.Count - 1; i++)
            {
                time += tspEntity.Route[i].CalculateTime(tspEntity.Route[i + 1], speeds[i]);
            }

            return time + tspEntity.Route[tspEntity.Route.Count - 1].CalculateTime(tspEntity.Route[0], speeds[speeds.Count - 1]);
        }

        private List<double> CalculateSpeeds(double maxSpeed, double minSpeed, TSPIndividual tspEntity, Knapsack knapsack)
        {
            List<double> speeds = new List<double>();
            int currentWeight = 0;
            for (int i = 0; i < tspEntity.Route.Count; i++)
            {
                var takenItem = knapsack.Items.SingleOrDefault(x => x.AssignedNode == tspEntity.Route[i].Index);
                if (takenItem != null)
                {
                    currentWeight += takenItem.Weight;
                }

                speeds.Add(CalculateSpeed(maxSpeed, minSpeed, currentWeight, knapsack.Capacity));
            }

            return speeds;
        }

        private TSPIndividual RunTournament(List<TSPIndividual> participants)
        {
            if (participants == null || !participants.Any())
            {
                throw new ArgumentException();
            }

            return participants.OrderByDescending(x => x.Quality).First();
        }

        private double CalculateSpeed(double maxSpeed, double minSpeed, int currentWeight, int knapsackCapacity)
        {
            return maxSpeed - currentWeight * ((maxSpeed - minSpeed) / knapsackCapacity);
        }

        private double[,] CalculateDistances(IList<City> cities)
        {
            var orderedCities = cities.OrderBy(x => x.Index).ToList();
            var distances = new double[cities.Count, cities.Count];
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = 0; j < cities.Count; j++)
                {
                    distances[i, j] = orderedCities[i].CalculateDistance(orderedCities[j]);
                }
            }
            return distances;
        }
    }
}