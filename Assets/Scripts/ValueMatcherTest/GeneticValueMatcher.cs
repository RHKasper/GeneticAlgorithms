using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgoCore;
using UnityEngine;

namespace ValueMatcherTest
{
    public class GeneticValueMatcher : GeneticAlgorithmBase<GeneticValueMatcherIndividual>
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
            return Mathf.Abs(TargetValue - individual.Value);
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

        protected override GeneticValueMatcherIndividual CreateCrossover(List<GeneticValueMatcherIndividual> availableParents)
        {
            int firstParentIndex = Random.Range(0, availableParents.Count);
            int secondParentIndex = Random.Range(0, availableParents.Count);

            // todo: make this deterministically choose two (technically, this could run forever if random always gave the same value)
            while (secondParentIndex == firstParentIndex)
            {
                secondParentIndex = Random.Range(0, availableParents.Count);
            }

            return new GeneticValueMatcherIndividual(.5f * (availableParents[firstParentIndex].Value + availableParents[secondParentIndex].Value), GeneticIndividual.IndividualType.Crossover);
        }

        protected override GeneticValueMatcherIndividual CreateMutant(List<GeneticValueMatcherIndividual> availableParents)
        {
            int parentIndex = Random.Range(0, availableParents.Count);
            float mutationFraction = Random.Range(-MaxMutationFraction, MaxMutationFraction);
            float mutationDelta = Range * mutationFraction;

            return new GeneticValueMatcherIndividual(availableParents[parentIndex].Value + mutationDelta, GeneticIndividual.IndividualType.Mutant);
        }
    }
}