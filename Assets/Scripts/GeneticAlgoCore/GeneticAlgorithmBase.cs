using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GeneticAlgoCore
{
    public abstract class GeneticAlgorithmBase<TIndividual> where TIndividual : GeneticIndividual
    {
        public readonly int EliteCount = 8;
        public readonly float CrossoverFraction = .8f;
        public readonly float ParentsFraction = .3f;
        protected IReadOnlyCollection<TIndividual> Individuals;

        public int CurrentGenerationNumber { get; private set; } = 0;

        public GeneticAlgorithmBase(HashSet<TIndividual> initialPopulation)
        {
            Individuals = initialPopulation;
        }
        
        /// <summary>
        /// returns fitnesses for the previous generation
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tuple<float, TIndividual>>> RunGeneration()
        {
            Debug.Log("Running generation " + CurrentGenerationNumber);
            // run fitness test
            await RunGenerationalFitnessTest();
         
            // score individuals' fitnesses
            List<Tuple<float, TIndividual>> fitnesses = new();
            foreach (TIndividual individual in Individuals)
            {
                fitnesses.Add(new Tuple<float, TIndividual>(GetIndividualFitness(individual), individual));
            }
            fitnesses.Sort((f1, f2) => -f1.Item1.CompareTo(f2.Item1));
            
            // create next generation
            Individuals = CreateNextGeneration(fitnesses); 
            CurrentGenerationNumber++;

            return fitnesses;
        }

        public List<TIndividual> GetCopyOfIndividualsList() => Individuals.ToList();
        
        /// <summary>
        /// runs the fitness test for the generation. After this method completes, <see cref="GetIndividualFitness"/> is ready to be called
        /// </summary>
        protected abstract Task RunGenerationalFitnessTest();

        /// <summary>
        /// returns fitness value for one individual. Higher is better.
        /// </summary>
        protected abstract float GetIndividualFitness(TIndividual individual);
        
        private HashSet<TIndividual> CreateNextGeneration(List<Tuple<float, TIndividual>> fitnesses)
        {
            int population = Individuals.Count;
            int crossoverCount = Mathf.RoundToInt((population - EliteCount) * CrossoverFraction);
            int mutantCount = population - EliteCount - crossoverCount;
            int parentsCount = Mathf.RoundToInt(population * ParentsFraction);
            int stoppingPoint = Mathf.Max(parentsCount, EliteCount);

            HashSet<TIndividual> nextGeneration = new();
            List<TIndividual> parents = new();

            Debug.Log($"Creating next generation with {EliteCount} elites, {crossoverCount} crossovers, and {mutantCount} mutants");
            
            // get elites and parents
            for (int i = 0; i < stoppingPoint; i++)
            {
                if (i < EliteCount)
                {
                    Debug.Log("Keeping as elite: " + fitnesses[i]);
                    nextGeneration.Add((TIndividual)fitnesses[i].Item2.DeepCopy(GeneticIndividual.IndividualType.Elite));
                }

                if (i < parentsCount)
                {
                    Debug.Log("Using as parent: " + fitnesses[i]);
                    parents.Add(fitnesses[i].Item2);
                }
            }
            
            Assert.AreEqual(parentsCount, parents.Count);
            Assert.AreEqual(EliteCount, nextGeneration.Count);
            
            // create crossovers
            for (int i = 0; i < crossoverCount; i++)
            {
                List<TIndividual> chosenParents = ChooseNRandomElements(parents, 2);
                nextGeneration.Add(CreateCrossover(chosenParents[0], chosenParents[1]));
                Debug.Log("Created " + nextGeneration.Last() + " as crossover between " + chosenParents[0] + " and " + chosenParents[1]);
            }

            Assert.AreEqual(EliteCount + crossoverCount, nextGeneration.Count);
            
            // create mutants
            for (int i = 0; i < mutantCount; i++)
            {
                TIndividual chosenParent = parents[Random.Range(0, parents.Count)];
                nextGeneration.Add(CreateMutant(chosenParent));
                Debug.Log("Created " + nextGeneration.Last() + " as mutant from " + chosenParent);
            }
            Assert.AreEqual(EliteCount + crossoverCount + mutantCount, nextGeneration.Count);
            Assert.AreEqual(population, nextGeneration.Count);

            return nextGeneration;
        }

        /// <summary>
        /// Creates a new crossover individual by blending the traits of <paramref name="parent1"/> and <paramref name="parent2"/>
        /// </summary>
        protected abstract TIndividual CreateCrossover(TIndividual parent1, TIndividual parent2);
        
        /// <summary>
        /// Creates a new mutant individual by randomly tweaking <paramref name="parent"/>'s traits
        /// </summary>
        protected abstract TIndividual CreateMutant(TIndividual parent);

        // todo: test distribution on this and move it to the RK Unity toolkit if it's good
        private List<T> ChooseNRandomElements<T>(List<T> elements, int numElements, System.Random random = null)
        {
            if (numElements > elements.Count)
            {
                throw new ArgumentOutOfRangeException("Cannot choose " + numElements + " random elements from a list with " + elements.Count + " elements");
            }
            
            int numerator = numElements;
            int denominator = elements.Count;
            List<T> results = new();
            random ??= new System.Random(DateTime.Now.Millisecond);
            
            for (int i = 0; i < elements.Count; i++)
            {
                if (random.NextDouble() <= (float)numerator / denominator)
                {
                    results.Add(elements[i]);
                    numerator--;
                    if (numerator == 0)
                    {
                        break;
                    }
                }

                denominator--;
            }

            return results;
        }
        
        /// <summary>
        /// Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they can be cached or something for better performance, but who cares.
        /// Source: https://bitbucket.org/Superbest/superbest-random/src/master/Superbest%20random/RandomExtensions.cs
        /// </summary>
        /// <param name = "mu">Mean of the distribution</param>
        /// <param name = "sigma">Standard deviation</param>
        /// <returns></returns>
        public static float NextGaussian(float mu = 0, float sigma = 1)
        {
            var u1 = Random.value;
            var u2 = Random.value;

            var rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                                  Mathf.Sin(2.0f * Mathf.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}
