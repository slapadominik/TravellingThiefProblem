using CsvHelper.Configuration;
using TTP.Entities;

namespace TTP
{
    public class TSPStatisticsCsvModelMapper : ClassMap<TSPPopulationStatistics>
    {
        public TSPStatisticsCsvModelMapper()
        {
            Map(x => x.PopulationNumber).Index(0).Name("Population number");
            Map(m => m.BestQuality).Index(1).Name("Best quality");
            Map(m => m.AverageQuality).Index(2).Name("Average quality");
            Map(m => m.WorstQuality).Index(3).Name("Worst quality");
        }
    }
}