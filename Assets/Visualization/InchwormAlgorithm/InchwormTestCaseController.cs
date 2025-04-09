using System;
using GeneticAlgorithms;
using UnityEngine;

namespace Visualization.InchwormAlgorithm
{
    public class InchwormTestCaseController : MonoBehaviour
    {
        [SerializeField] private InchwormController inchworm;
        
        private GeneticInchwormMovementAlgorithm.Individual _individual;
        private float _elapsedTime = 0;
        
        void Update()
        {
            if (_individual == null)
            {
                throw new Exception("Individual is null");
            }

            int frame = (int)Mathf.Clamp(_elapsedTime / _individual.FrameDuration, 0, _individual.FrontSegmentVelocityFrames.Length-1);
            inchworm.SetFrontSegmentTargetVelocity(_individual.FrontSegmentVelocityFrames[frame]);
            inchworm.SetRearSegmentTargetVelocity(_individual.RearSegmentVelocityFrames[frame]);
            
            _elapsedTime += Time.deltaTime;
        }

        public void Init(GeneticInchwormMovementAlgorithm.Individual individual)
        {
            _individual = individual;
            inchworm.SetColor(individual.GetColor());
        }
    }
}
