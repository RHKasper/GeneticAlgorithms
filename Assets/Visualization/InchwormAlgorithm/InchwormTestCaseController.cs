using System;
using GeneticAlgorithms;
using UnityEngine;

namespace Visualization.InchwormAlgorithm
{
    public class InchwormTestCaseController : MonoBehaviour
    {
        private GeneticInchwormMovementAlgorithm.Individual _individual;
        
        void Update()
        {
            if (_individual == null)
            {
                throw new Exception("Individual is null");
            }
        }

        public void Init(GeneticInchwormMovementAlgorithm.Individual individual)
        {
            _individual = individual;
        }
    }
}
