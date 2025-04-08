using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgoCore;
using UnityEngine;
using Visualization.InchwormAlgorithm;
using Object = UnityEngine.Object;

namespace GeneticAlgorithms
{
    public class GeneticInchwormMovementAlgorithm : GeneticAlgorithmBase<GeneticInchwormMovementAlgorithm.Individual>
    {
        private readonly float frameDurationSeconds;
        private readonly int numFrames;
        private readonly InchwormTestCaseController testCasePrefab;
        private readonly float verticalSpacing;

        private Dictionary<Individual, Vector3> _startPositions;

        private float TestCaseDurationSeconds => numFrames * frameDurationSeconds; 
        
        public GeneticInchwormMovementAlgorithm(float frameDurationSeconds, int numFrames, InchwormTestCaseController testCasePrefab, float verticalSpacing, int populationSize) : base(GenerateInitialPopulation(numFrames, populationSize))
        {
            this.frameDurationSeconds = frameDurationSeconds;
            this.numFrames = numFrames;
            this.testCasePrefab = testCasePrefab;
            this.verticalSpacing = verticalSpacing;
        }

        private static HashSet<Individual> GenerateInitialPopulation(int numFrames, int populationSize)
        {
            HashSet<Individual> population = new();
            
            for (int i = 0; i < populationSize; i++)
            {
                var individual = new Individual(GeneticIndividual.IndividualType.Initial, numFrames);
                // todo: randomly assign initial traits

                population.Add(individual);
            }

            return population;
        }

        protected override async Task RunGenerationalFitnessTest()
        {
            Debug.Log(Individuals.Count + " individuals");
            Dictionary<Individual, InchwormTestCaseController> testCases = new(); 
            
            foreach (Individual individual in Individuals)
            {
                var testCase = Object.Instantiate(testCasePrefab);
                testCases.Add(individual, testCase);
                testCase.transform.position = Vector3.zero + testCases.Count * verticalSpacing * Vector3.up;
                testCase.Init(individual);

                individual.StartingPosition = testCase.transform.position;
                individual.StartingForwardVector = testCase.transform.forward;
            }

            await Task.Delay((int)(TestCaseDurationSeconds*1000));

            foreach (Individual individual in Individuals)
            {
                individual.EndingPosition = testCases[individual].transform.position;
                Object.Destroy(testCases[individual]);
            }
        }

        protected override float GetIndividualFitness(Individual individual)
        {
            return (individual.EndingPosition - individual.StartingPosition).sqrMagnitude;
        }

        protected override Individual CreateCrossover(Individual parent1, Individual parent2)
        {
            throw new System.NotImplementedException();
        }

        protected override Individual CreateMutant(Individual parent)
        {
            throw new System.NotImplementedException();
        }
        
        
        public class Individual : GeneticIndividual
        {
            public List<float> RearSegmentVelocityFrames;
            public List<float> FrontSegmentVelocityFrames;
            public Vector3 StartingPosition;
            public Vector3 StartingForwardVector;
            public Vector3 EndingPosition;

            public Individual(IndividualType individualType, int numFrames) : base(individualType)
            {
                RearSegmentVelocityFrames = new List<float>(numFrames);
                FrontSegmentVelocityFrames = new List<float>(numFrames);
            }

            public override GeneticIndividual DeepCopy(IndividualType type)
            {
                return new Individual(type, RearSegmentVelocityFrames.Count)
                {
                    RearSegmentVelocityFrames = RearSegmentVelocityFrames.ToList(),
                    FrontSegmentVelocityFrames = FrontSegmentVelocityFrames.ToList()
                };
            }
        }
    }
}