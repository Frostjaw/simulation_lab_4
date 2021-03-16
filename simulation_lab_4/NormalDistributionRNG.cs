using System;
using System.Collections.Generic;

namespace simulation_lab_4
{
    public class NormalDistributionRNG
    {
        private readonly Random _rng;

        public NormalDistributionRNG()
        {
            _rng = new Random();
        }

        public double GetRandomNumber(double mean, double variance)
        {
            var zeta = BoxMullerTransform(_rng.NextDouble(), _rng.NextDouble());

            return Math.Sqrt(variance) * zeta + mean;
        }

        public List<double> GetRandomNumberList(double mean, double variance, int length)
        {
            var randomList = new List<double>();

            for (var i = 0; i < length; i++)
            {
                var number = GetRandomNumber(mean, variance);
                randomList.Add(number);
            }

            return randomList;
        }

        private double BoxMullerTransform(double alpha1, double alpha2)
        {
            return Math.Sqrt(-2 * Math.Log(alpha1)) * Math.Cos(2 * Math.PI * alpha2);
        }
    }
}
