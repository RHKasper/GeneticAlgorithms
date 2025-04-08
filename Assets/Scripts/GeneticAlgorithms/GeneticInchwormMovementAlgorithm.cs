using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgoCore;

namespace InchwormTest
{
    public class GeneticInchwormMovementAlgorithm : GeneticAlgorithmBase<GeneticInchwormMovementAlgorithm.Individual>
    {
        public GeneticInchwormMovementAlgorithm(HashSet<Individual> initialPopulation) : base(initialPopulation)
        {
        }

        protected override Task RunGenerationalFitnessTest()
        {
            throw new System.NotImplementedException();
        }

        protected override float GetIndividualFitness(Individual individual)
        {
            throw new System.NotImplementedException();
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
            
            public Individual(IndividualType individualType) : base(individualType)
            {
            }

            public override GeneticIndividual DeepCopy(IndividualType type)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}