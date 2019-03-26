using CsvHelper.Configuration;
using TTP.Entities;

namespace TTP
{
    public class CityToCsvModelMapper : ClassMap<City>
    {
        public CityToCsvModelMapper()
        {
            Map(x => x.Index).Index(0).Name("Index");
            Map(x => x.X).Index(1).Name("X");
            Map(x => x.Y).Index(2).Name("Y");
        }
    }
}