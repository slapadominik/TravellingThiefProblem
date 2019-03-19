using System;
using System.IO;
using TTP.Entities;

namespace TTP
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly ILoader _dataLoader;
        private const int TtpInputFilePathArgument = 0;
        private const int CsvOutputFilePathArgument = 1;

        public Bootstrapper(ILoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public (TTPData data, string csvOutputPath) ParseArguments(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException($"Wrong arguments count! Scheme: ttp \"inputPath\" \"outputPath\"");
            }
            if (!Directory.Exists(args[CsvOutputFilePathArgument]))
            {
                throw new DirectoryNotFoundException();
            }
            if (!Path.GetFileName(args[TtpInputFilePathArgument]).Contains(".ttp"))
            {
                throw new ArgumentException("Wrong \"inputPath\" argument. Input file should have .ttp extension.");
            }

            var csvOutputPath = $"{args[CsvOutputFilePathArgument]}\\{Path.GetFileNameWithoutExtension(args[TtpInputFilePathArgument])}.csv";
            var ttpData = _dataLoader.LoadFromFile(args[TtpInputFilePathArgument]);
            ttpData.GenerationNumber = 100;
            ttpData.PopulationSize = 100;
            ttpData.IndividualsPerTournament = 10;
            ttpData.MutationProbability = 0.6f;
            ttpData.CrossProbability = 0.5f;
            return (ttpData, csvOutputPath);
        }
    }
}