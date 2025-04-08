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

            int frame = (int)(_elapsedTime / _individual.FrameDuration);
            inchworm.SetFrontSegmentTargetVelocity(_individual.FrontSegmentVelocityFrames[frame]);
            inchworm.SetRearSegmentTargetVelocity(_individual.RearSegmentVelocityFrames[frame]);
            
            _elapsedTime += Time.deltaTime;
        }

        public void Init(GeneticInchwormMovementAlgorithm.Individual individual)
        {
            _individual = individual;
        }
    }
}
