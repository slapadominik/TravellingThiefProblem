using System;

namespace TTP.Entities
{
    public class City
    {
        public int Index { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public double CalculateDistance(City city)
        {
            return Math.Sqrt(Math.Pow(X - city.X, 2) + Math.Pow(Y - city.Y, 2));
        }

        public double CalculateTime(City city, double speed)
        {
            return Math.Sqrt(Math.Pow(X - city.X, 2) + Math.Pow(Y - city.Y, 2)) / speed;
        }

        public override string ToString()
        {
            return $"{Index}, {X}, {Y}";
        }
    }
}