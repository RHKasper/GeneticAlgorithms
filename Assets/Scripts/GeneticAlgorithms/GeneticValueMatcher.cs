using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgoCore;
using UnityEngine;

namespace ValueMatcherTest
{
    public class GeneticValueMatcher : GeneticAlgorithmBase<GeneticValueMatcher.GeneticValueMatcherIndividual>
    {
        public const float Range = 20;
        public const float MaxMutationFraction = .1f;
        public float TargetValue;
        
        
        public GeneticValueMatcher(int populationSize, float targetValue) : base(GenerateInitialPopulation(populationSize, targetValue - .5f * Range, targetValue + .5f * Range))
        {
            TargetValue = targetValue;
        }
        
        protected override async Task RunGenerationalFitnessTest()
        {
            await Task.Yield();
        }

        protected override float GetIndividualFitness(GeneticValueMatcherIndividual individual)
        {
            return -Mathf.Abs(TargetValue - individual.Value);
        }

        private static HashSet<GeneticValueMatcherIndividual> GenerateInitialPopulation(int populationSize, float min, float max)
        {
            HashSet<GeneticValueMatcherIndividual> individuals = new();
            for (int i = 0; i < populationSize; i++)
            {
                individuals.Add(new GeneticValueMatcherIndividual(Random.Range(min, max), GeneticIndividual.IndividualType.Initial));
            }

            return individuals;
        }

        protected override GeneticValueMatcherIndividual CreateCrossover(GeneticValueMatcherIndividual parent1, GeneticValueMatcherIndividual parent2)
        {
            return new GeneticValueMatcherIndividual(.5f * (parent1.Value + parent2.Value), GeneticIndividual.IndividualType.Crossover);
        }

        protected override GeneticValueMatcherIndividual CreateMutant(GeneticValueMatcherIndividual parent)
        {
            float mutationFraction = Random.Range(-MaxMutationFraction, MaxMutationFraction);
            float mutationDelta = Range * mutationFraction;

            return new GeneticValueMatcherIndividual(parent.Value + mutationDelta, GeneticIndividual.IndividualType.Mutant);
        }
        
        public class GeneticValueMatcherIndividual : GeneticIndividual
        {
            public float Value;

            public GeneticValueMatcherIndividual(float value, IndividualType individualType) : base(individualType)
            {
                Value = value;
            }

            public override GeneticIndividual DeepCopy(IndividualType type)
            {
                return new GeneticValueMatcherIndividual(Value, type);
            }
        }
    }
}