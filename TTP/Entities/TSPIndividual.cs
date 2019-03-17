using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TTP.Entities
{
    public class TSPIndividual
    {
        public List<City> Route { get; private set; }
        public int PopulationNumber { get; }
        public double Quality { get; set; }
        private readonly IEqualityComparer<City> _cityEqualityComparer;

        public TSPIndividual(List<City> route, int populationNumber)
        {
            Route = route;
            PopulationNumber = populationNumber;
            _cityEqualityComparer = new CityEqualityComparer();
        }

        public void RepairRoute(TSPIndividual parent1, TSPIndividual parent2)
        {
            var uniqueCities = Route.Distinct(_cityEqualityComparer).ToList();
            var citySubset = parent1.Route.Except(uniqueCities, _cityEqualityComparer);
            var missingCities = citySubset.Take(Route.Count - uniqueCities.Count());
            uniqueCities.AddRange(missingCities);

            if (uniqueCities.Distinct(_cityEqualityComparer).Count() != Route.Count)
            {
                Console.WriteLine("Repairing child didn't work out, let's try again");
                RepairRoute(parent1, parent2);
            }
            else
            {
                Route = uniqueCities;
            }
        }

        public override string ToString()
        {
            return $"Population: {PopulationNumber}, Quality: {Quality}";
        }
    }
}