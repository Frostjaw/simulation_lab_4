using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace simulation_lab_4
{
    public class NormalDistributionStatistics
    {
        private List<double> _sample;
        private int _numberOfExperiments;
        private double _theoreticalMean;
        private double _theoreticalVariance;

        public NormalDistributionStatistics(List<double> sample, double mean, double variance)
        {
            _sample = sample;
            _numberOfExperiments = sample.Count;
            _theoreticalMean = mean;
            _theoreticalVariance = variance;

            EmpiricalMean = CalculateMean();
            EmpiricalVariance = CalculateVariance();
            MeanError = Math.Abs(EmpiricalMean - _theoreticalMean);
            VarianceError = Math.Abs(EmpiricalVariance - _theoreticalVariance);
            Occurrences = CalculateOccurrences();
            ChiSquared = CalculateChiSquared();
        }

        public double EmpiricalMean { get; }
        public double EmpiricalVariance { get; }
        public double MeanError { get; }
        public double VarianceError { get; }
        public double ChiSquared { get; }
        public Dictionary<(int, int), int> Occurrences { get; }

        private double CalculateMean()
        {
            var sum = 0d;
            foreach (var number in _sample)
            {
                sum += number;
            }

            return sum / _numberOfExperiments;
        }

        private double CalculateVariance()
        {
            var sum = 0d;

            foreach (var number in _sample)
            {
                sum += Math.Pow(number - EmpiricalMean, 2);
            }

            return sum / (_numberOfExperiments - 1);
        }

        private Dictionary<(int, int), int> CalculateOccurrences()
        {
            var occurences = new Dictionary<(int, int), int>();

            var bottomBound = (int)Math.Floor(_sample.Min());
            var upperBound = (int)Math.Ceiling(_sample.Max());

            for (var i = bottomBound; i < upperBound; i++)
            {
                occurences.Add((i, i + 1), 0);
            }

            foreach (var number in _sample)
            {
                var currentBottomBound = (int)Math.Floor(number);
                var currentUpperBound = (int)Math.Ceiling(number);
                occurences[(currentBottomBound, currentUpperBound)]++;
            }

            return occurences;
        }

        private double CalculateChiSquared()
        {
            var sum = 0d;

            foreach (var range in Occurrences)
            {
                var bottomBound = range.Key.Item1;
                var upperBound = range.Key.Item2;

                var bottomBoundCDF = Normal.CDF(_theoreticalMean, Math.Sqrt(_theoreticalVariance), bottomBound);
                var upperBoundCDF = Normal.CDF(_theoreticalMean, Math.Sqrt(_theoreticalVariance), upperBound);

                var expectedFreq = upperBoundCDF - bottomBoundCDF;

                var n = range.Value;

                sum += n * n / (_numberOfExperiments * expectedFreq);
            }

            return sum - _numberOfExperiments;
        }
    }
}
