using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgoCore;
using UnityEngine;
using Visualization.InchwormAlgorithm;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GeneticInchwormMovementAlgorithm : GeneticAlgorithmBase<GeneticInchwormMovementAlgorithm.Individual>
{
    private readonly float frameDurationSeconds;
    private readonly int numFrames;
    private readonly Vector2 motorVelocityRange;
    private readonly InchwormTestCaseController testCasePrefab;
    private readonly float verticalSpacing;

    private Dictionary<Individual, Vector3> _startPositions;

    private float TestCaseDurationSeconds => numFrames * frameDurationSeconds; 
        
    public GeneticInchwormMovementAlgorithm(float frameDurationSeconds, int numFrames, Vector2 motorVelocityRange, InchwormTestCaseController testCasePrefab, float verticalSpacing, int populationSize) : base(GenerateInitialPopulation(numFrames, populationSize, motorVelocityRange, frameDurationSeconds))
    {
        this.frameDurationSeconds = frameDurationSeconds;
        this.numFrames = numFrames;
        this.motorVelocityRange = motorVelocityRange;
        this.testCasePrefab = testCasePrefab;
        this.verticalSpacing = verticalSpacing;
    }

    private static HashSet<Individual> GenerateInitialPopulation(int numFrames, int populationSize, Vector2 motorVelocityRange, float frameDuration)
    {
        HashSet<Individual> population = new();
            
        for (int i = 0; i < populationSize; i++)
        {
            var individual = new Individual(GeneticIndividual.IndividualType.Initial, numFrames, frameDuration);

            for (int frame = 0; frame < numFrames; frame++)
            {
                individual.FrontSegmentVelocityFrames.Add(Random.Range(motorVelocityRange.x, motorVelocityRange.y));
                individual.RearSegmentVelocityFrames.Add(Random.Range(motorVelocityRange.x, motorVelocityRange.y));
            }

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
        public readonly float FrameDuration;
        public List<float> RearSegmentVelocityFrames;
        public List<float> FrontSegmentVelocityFrames;
        
        public Vector3 StartingPosition; // todo: make these fields internal
        public Vector3 StartingForwardVector;
        public Vector3 EndingPosition;

        public Individual(IndividualType individualType, int numFrames, float frameDuration) : base(individualType)
        {
            this.FrameDuration = frameDuration;
            RearSegmentVelocityFrames = new List<float>(numFrames);
            FrontSegmentVelocityFrames = new List<float>(numFrames);
        }

        public override GeneticIndividual DeepCopy(IndividualType type)
        {
            return new Individual(type, RearSegmentVelocityFrames.Count, FrameDuration)
            {
                RearSegmentVelocityFrames = RearSegmentVelocityFrames.ToList(),
                FrontSegmentVelocityFrames = FrontSegmentVelocityFrames.ToList()
            };
        }
    }
}

namespace GeneticAlgorithms
{
}