using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgoCore;
using UnityEngine;
using Vector2Extensions;
using Vector3Extensions;

namespace TerrainMinMaxTest
{
    public class GeneticTerrainMinMaxFinder : GeneticAlgorithmBase<GeneticTerrainMinMaxFinder.Individual>
    {
        private const float TerrainSize = 1000;
        
        public MinOrMax MinimumOrMaximum { get; private set; }
        public TerrainCollider TerrainCollider { get; private set; }
        public Vector2 TerrainDimensions => TerrainCollider.transform.localScale * TerrainSize;
        
        public GeneticTerrainMinMaxFinder(int populationSize, MinOrMax minOrMax, TerrainCollider terrainCollider) : base(CreateInitialPopulation(populationSize, terrainCollider.transform.position, terrainCollider.transform.localScale * TerrainSize))
        {
            MinimumOrMaximum = minOrMax;
            TerrainCollider = terrainCollider;
        }

        protected override async Task RunGenerationalFitnessTest()
        {
            await Task.Yield();
        }

        protected override float GetIndividualFitness(Individual individual)
        {
            Vector3 raycastOrigin = individual.XZCoords.XOY().WithY(Random.Range(-100, 100));
            if(Physics.Raycast(raycastOrigin, Vector3.up, out var upHit))
            {
                return MinimumOrMaximum == MinOrMax.Max ? upHit.point.y : -upHit.point.y;
            }
            else if (Physics.Raycast(raycastOrigin, Vector3.up, out var downHit))
            {
                return MinimumOrMaximum == MinOrMax.Max ? downHit.point.y : -downHit.point.y;
            }

            return int.MinValue;
        }

        protected override Individual CreateCrossover(Individual parent1, Individual parent2)
        {
            return new Individual(.5f * (parent1.XZCoords + parent2.XZCoords), GeneticIndividual.IndividualType.Crossover);
        }

        protected override Individual CreateMutant(Individual parent)
        {
            Vector2 delta = TerrainDimensions.Multiply(Random.insideUnitCircle);
            return new Individual(parent.XZCoords + delta, GeneticIndividual.IndividualType.Mutant);
        }

        private static HashSet<Individual> CreateInitialPopulation(int populationSize, Vector2 terrainZeroPoint, Vector2 terrainDimensions)
        {
            Vector2 centerPoint = (terrainZeroPoint + terrainZeroPoint + terrainDimensions) / 2f;
            HashSet<Individual> individuals = new();
            
            for (int i = 0; i < populationSize; i++)
            {
                Vector2 point = centerPoint + Random.insideUnitCircle.Multiply(terrainDimensions);
                individuals.Add(new Individual(point, GeneticIndividual.IndividualType.Initial));
            }

            return individuals;
        }
        
        public enum MinOrMax
        {
            Min, Max
        }
        
        public class Individual : GeneticIndividual
        {
            public Vector2 XZCoords;
            
            public Individual(Vector2 xzCoords, IndividualType individualType) : base(individualType)
            {
                XZCoords = xzCoords;
            }

            public override GeneticIndividual DeepCopy(IndividualType type)
            {
                return new Individual(XZCoords, type);
            }
        }
    }
}
