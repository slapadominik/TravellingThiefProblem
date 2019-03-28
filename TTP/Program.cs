using System;
using System.IO;

namespace TTP
{
    class Program
    {
        private static IFileHelper fileHelper;
        private static IBootstrapper bootstrapper;
        private static IKnapsackProblem knapsackProblem;
        private static ITravellingSalesmanProblem tsp;

        static void Main(string[] args)
        {
            Init();
            try
            {
                var (ttpData, statisticsOutputPath, bestSolutionOutputPath) = bootstrapper.ParseArguments(args);

                Console.WriteLine("Started resolving TravellingSalesmanProblem.");
                var knapsack = knapsackProblem.GetMaximumProfit(ttpData.KnapsackCapacity, ttpData.Items);
                var (bestIndividual, statistics) = tsp.ResolveProblem(ttpData, knapsack);
                Console.WriteLine("Ended processing TravellingSalesmanProblem.");

                fileHelper.WriteStatisticsToCsv(ttpData, statistics, knapsack, statisticsOutputPath);
                fileHelper.WriteBestSolutionToCsv(bestIndividual, bestSolutionOutputPath);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        private static void Init()
        {
            fileHelper = new FileHelper();
            bootstrapper = new Bootstrapper(new Loader(fileHelper));
            knapsackProblem = new KnapsackProblem();
            tsp = new TravellingSalesmanProblemGeneticAlgorithm();
        }
    }
}
