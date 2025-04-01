using GeneticAlgoCore;

namespace ValueMatcherTest
{
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