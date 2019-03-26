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
            if (args.Length != 3)
            {
                throw new ArgumentException($"Wrong arguments count! Scheme: ttp \"inputPath\" \"outputPath\"");
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
            ttpData.GenerationNumber = 400;
            ttpData.PopulationSize = 150;
            ttpData.IndividualsPerTournament = 5;
            ttpData.MutationProbability = 0.55f;
            ttpData.CrossProbability = 0.5f;
            return (ttpData, statisticsOutputPath, routeOutputPath);
        }
    }
}