using System;
using System.IO;
using TTP.Entities;

namespace TTP
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly ILoader _dataLoader;
        private const int TtpInputFilePathArgument = 0;
        private const int CsvStatisticOutputFilePathArgument = 1;
        private const int CsvRouteOutputFilePathArgument = 1;

        public Bootstrapper(ILoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public (TTPData data, string statisticsOutputPath, string routeOutputPath) ParseArguments(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException($"Wrong arguments count! {Environment.NewLine}Scheme: dotnet run \"inputPath\" \"outputPath\"{Environment.NewLine}Params:{Environment.NewLine}-inputPath - .ttp file location containing data{Environment.NewLine}-outputPath - existing directory where you want to save .csv output{Environment.NewLine}Example: dotnet run \"..\\ttpData\\easy_0.ttp\" \"..\\ttpData\"");
            }
            if (!Directory.Exists(args[CsvStatisticOutputFilePathArgument]))
            {
                throw new DirectoryNotFoundException();
            }
            if (!Directory.Exists(args[CsvRouteOutputFilePathArgument]))
            {
                throw new DirectoryNotFoundException();
            }
            if (!Path.GetFileName(args[TtpInputFilePathArgument]).Contains(".ttp"))
            {
                throw new ArgumentException("Wrong \"inputPath\" argument. Input file should have .ttp extension.");
            }

            var statisticsOutputPath = $"{args[CsvStatisticOutputFilePathArgument]}\\{Path.GetFileNameWithoutExtension(args[TtpInputFilePathArgument])}_statistics.csv";
            var routeOutputPath = $"{args[CsvStatisticOutputFilePathArgument]}\\{Path.GetFileNameWithoutExtension(args[TtpInputFilePathArgument])}_route.csv";
            var ttpData = _dataLoader.LoadFromFile(args[TtpInputFilePathArgument]);
            ttpData.GenerationNumber = 100;
            ttpData.PopulationSize = 100;
            ttpData.IndividualsPerTournament = 50;
            ttpData.MutationProbability = 0.5f;
            ttpData.CrossProbability = 0.25f;
            return (ttpData, statisticsOutputPath, routeOutputPath);
        }
    }
}