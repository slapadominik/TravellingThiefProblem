using CsvHelper.Configuration;
using TTP.Entities;

namespace TTP
{
    public class TTPResultCsvModelMapper : ClassMap<TTPData>
    {
        public TTPResultCsvModelMapper()
        {
            Map(m => m.PopulationSize).Index(0).Name("Population size");
            Map(m => m.GenerationNumber).Index(1).Name("Generation number");
            Map(m => m.KnapsackCapacity).Index(2).Name("Knapsack capacity");
            Map(m => m.CrossProbability).Index(3).Name("Cross probability");
            Map(m => m.MutationProbability).Index(4).Name("Mutation probability");
            Map(m => m.IndividualsPerTournament).Index(5).Name("Individuals per tournament");
            Map(m => m.MinSpeed).Index(6).Name("Min speed");
            Map(m => m.MaxSpeed).Index(7).Name("Max speed");
        }
    }
}