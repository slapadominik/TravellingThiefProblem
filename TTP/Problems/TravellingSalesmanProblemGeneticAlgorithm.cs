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

        public TravellingSalesmanProblemGeneticAlgorithm()
        {
            _randomGenerator = new Random();
        }

        public TSPIndividual Initialize(List<City> cities)
        {
            return new TSPIndividual(cities.Shuffle());
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

        public (TSPIndividual child1, TSPIndividual child2) Crossover(TSPIndividual parent1, TSPIndividual parent2)
        {
            List<City> child1Route = new List<City>();
            List<City> child2Route = new List<City>();
            if (parent1.Route.Count != parent2.Route.Count)
            {
                throw new InvalidOperationException($"Crossover is not possible for child1 {parent1.Route.Count}, child2 {parent2.Route.Count}");
            }

            var splitPoint = _randomGenerator.Next(0, parent1.Route.Count);
            child1Route.AddRange(parent1.Route.GetRange(0, splitPoint).ToList());
            child1Route.AddRange(parent2.Route.GetRange(splitPoint, parent2.Route.Count-splitPoint));
            TSPIndividual child1 = new TSPIndividual(child1Route);
            child1.RepairRoute(parent1, parent2);

            var splitPoint2 = _randomGenerator.Next(0, parent1.Route.Count);
            child2Route.AddRange(parent1.Route.GetRange(0, splitPoint2).ToList());
            child2Route.AddRange(parent2.Route.GetRange(splitPoint2, parent2.Route.Count - splitPoint2));
            TSPIndividual child2 = new TSPIndividual(child2Route);
            child2.RepairRoute(parent1, parent2);

            return (child1, child2);
        }

        public TSPIndividual SelectionTournament(List<TSPIndividual> population, int individualsPerTournament)
        {
            TSPIndividual best = null;
            for (int i = 0; i < individualsPerTournament; i++)
            {
                var selected = population[_randomGenerator.Next(0, population.Count)];
                if (best == null || selected.Quality > best.Quality)
                {
                    best = selected;
                }
            }

            return best;
        }

        public (TSPIndividual bestIndividual, IEnumerable<TSPPopulationStatistics> statistics) ResolveProblem(TTPData initData, Knapsack knapsack)
        {
            var tspStatistics = new List<TSPPopulationStatistics>();
            var population = new List<TSPIndividual>();
            int populationNumber = 0;

            for (int i = 0; i < initData.PopulationSize; i++)
            {
                population.Add(Initialize(initData.Cities));
                population[i].Quality = Fitness(initData.MaxSpeed, initData.MinSpeed, population[i], knapsack);
            }

            var qualityDesc = population.OrderByDescending(x => x.Quality);
            tspStatistics.Add(new TSPPopulationStatistics{ PopulationNumber = populationNumber, AverageQuality = population.Average(x => x.Quality), BestQuality = qualityDesc.First().Quality, WorstQuality = qualityDesc.Last().Quality});

            for (int i = 0; i < initData.GenerationNumber-1; i++)
            {
                var childGeneration = new List<TSPIndividual>();

                while (childGeneration.Count<initData.PopulationSize)
                {
                    var parent1 = SelectionTournament(population, initData.IndividualsPerTournament);
                    var parent2 = SelectionTournament(population, initData.IndividualsPerTournament);
                    TSPIndividual child1, child2;
                    if (initData.CrossProbability > _randomGenerator.NextDouble())
                    {
                        (child1, child2) = Crossover(parent1, parent2);
                    }
                    else
                    {
                        child1 = parent1;
                        child2 = parent2;
                    }
                    if (initData.MutationProbability > _randomGenerator.NextDouble())
                    {
                        Mutation(child1);
                        Mutation(child2);
                    }
                    child1.Quality = Fitness(initData.MaxSpeed, initData.MinSpeed, child1, knapsack);
                    child2.Quality = Fitness(initData.MaxSpeed, initData.MinSpeed, child2, knapsack);
                    childGeneration.Add(child1);
                    childGeneration.Add(child2);
                }
                population = childGeneration;
                populationNumber++;
                var averageQuality = population.Average(x => x.Quality);
                var populationOrderedByQualityDesc = population.OrderByDescending(x => x.Quality);                
                tspStatistics.Add(new TSPPopulationStatistics{PopulationNumber = populationNumber, BestQuality = populationOrderedByQualityDesc.First().Quality, AverageQuality = averageQuality, WorstQuality = populationOrderedByQualityDesc.Last().Quality});
                Console.WriteLine($"Calculated generation: {populationNumber}");
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