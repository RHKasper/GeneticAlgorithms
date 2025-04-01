using GeneticAlgoCore;

namespace ValueMatcherTest
{
    public class GeneticValueMatcherIndividual : GeneticIndividual
    {
        public float Value;

        public GeneticValueMatcherIndividual(float value)
        {
            Value = value;
        }
    }
}