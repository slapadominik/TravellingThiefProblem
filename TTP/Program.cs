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
                var (ttpData, outputPath) = bootstrapper.ParseArguments(args);
                Console.WriteLine("Started resolving TravellingSalesmanProblem.");

                var knapsack = knapsackProblem.GetMaximumProfit(ttpData.KnapsackCapacity, ttpData.Items);
                var (bestIndividual, statistics) =
                    tsp.ResolveProblem(ttpData, knapsack);

                fileHelper.WriteToCsv(ttpData, statistics, knapsack, outputPath);
                Console.WriteLine("Ended processing TravellingSalesmanProblem.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
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
