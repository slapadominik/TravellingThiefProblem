using System.Collections;
using System.Collections.Generic;
using TTP.Entities;

namespace TTP
{
    public class CityEqualityComparer : IEqualityComparer<City>
    {
        public bool Equals(City x, City y)
        {
            return x.Index == y.Index;
        }

        public int GetHashCode(City obj)
        {
            return obj.Index.GetHashCode()*7;
        }
    }
}