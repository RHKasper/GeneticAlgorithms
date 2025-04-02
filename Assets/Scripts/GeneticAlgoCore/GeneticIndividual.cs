using System;
using UnityEngine;

namespace GeneticAlgoCore
{
    public abstract class GeneticIndividual
    {
        public IndividualType Type { get; protected set; }

        protected GeneticIndividual(IndividualType individualType)
        {
            Type = individualType;
        }

        /// <summary>
        /// returns a deep copy of this object with <see cref="Type"/> replaced by the value in <paramref name="type"/>
        /// </summary>
        public abstract GeneticIndividual DeepCopy(IndividualType type);

        public Color GetColor()
        {
            return Type switch
            {
                GeneticIndividual.IndividualType.Initial => Color.white,
                GeneticIndividual.IndividualType.Elite => Color.cyan,
                GeneticIndividual.IndividualType.Crossover => Color.yellow,
                GeneticIndividual.IndividualType.Mutant => Color.green,
                _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
            };
        }
        
        public enum IndividualType{Initial, Elite, Crossover, Mutant}
    }
}