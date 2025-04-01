using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GeneticAlgoCore
{
    public abstract class GeneticAlgorithmBase<TIndividual> where TIndividual : GeneticIndividual
    {
        public int EliteCount = 2;
        public float CrossoverFraction = .8f;
        public float ParentsFraction = .5f;
        private IReadOnlyCollection<TIndividual> _individuals;

        public int CurrentGenerationNumber { get; private set; } = 0;

        public GeneticAlgorithmBase(HashSet<TIndividual> initialPopulation)
        {
            _individuals = initialPopulation;
        }
        
        public async void RunGeneration()
        {
            // run fitness test
            await RunGenerationalFitnessTest();
         
            // score individuals' fitnesses
            List<Tuple<float, TIndividual>> fitnesses = new();
            foreach (TIndividual individual in _individuals)
            {
                fitnesses.Add(new Tuple<float, TIndividual>(GetIndividualFitness(individual), individual));
            }
            fitnesses.Sort((f1, f2) => f1.Item1.CompareTo(f2.Item1));
            
            // create next generation
            _individuals = CreateNextGeneration(fitnesses); 
            CurrentGenerationNumber++;
        }
        
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
            int population = _individuals.Count;
            int crossoverCount = Mathf.RoundToInt((population - EliteCount) * CrossoverFraction);
            int mutantCount = population - EliteCount - crossoverCount;
            int parentsCount = Mathf.RoundToInt(population * ParentsFraction);
            int stoppingPoint = Mathf.Max(parentsCount, EliteCount);

            HashSet<TIndividual> nextGeneration = new();
            List<TIndividual> parents = new();

            // get elites and parents
            for (int i = 0; i < stoppingPoint; i++)
            {
                if (i < EliteCount)
                {
                    nextGeneration.Add(fitnesses[i].Item2);
                }

                if (i < parentsCount)
                {
                    parents.Add(fitnesses[i].Item2);
                }
            }
            
            Assert.AreEqual(parentsCount, parents.Count);
            Assert.AreEqual(EliteCount, nextGeneration.Count);
            
            // create crossovers
            for (int i = 0; i < crossoverCount; i++)
            {
                nextGeneration.Add(CreateCrossover(parents));
            }

            Assert.AreEqual(EliteCount + crossoverCount, nextGeneration.Count);
            
            // create mutants
            for (int i = 0; i < mutantCount; i++)
            {
                nextGeneration.Add(CreateMutant(parents));
            }
            
            Assert.AreEqual(EliteCount + crossoverCount + mutantCount, nextGeneration.Count);
            Assert.AreEqual(population, nextGeneration.Count);

            return nextGeneration;
        }

        /// <summary>
        /// Randomly selects two parents from <see cref="availableParents"/> and blends their traits to create a new crossover individual
        /// </summary>
        protected abstract TIndividual CreateCrossover(List<TIndividual> availableParents);
        
        
        /// <summary>
        /// Randomly selects one parent from <see cref="availableParents"/> and randomly tweaks its traits to create a new mutant individual
        /// </summary>
        protected abstract TIndividual CreateMutant(List<TIndividual> availableParents);
        
        
    }
}
