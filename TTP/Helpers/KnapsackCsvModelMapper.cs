using CsvHelper.Configuration;
using TTP.Entities;

namespace TTP
{
    public class KnapsackCsvModelMapper : ClassMap<Knapsack>
    {
        public KnapsackCsvModelMapper()
        {
            Map(x => x.Profit).Index(0).Name("Knapsack profit");
            Map(x => x.Weight).Index(1).Name("Knapsack weight");
        }
    }
}