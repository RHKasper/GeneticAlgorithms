using UnityEngine;

namespace Visualization.InchwormAlgorithm
{
    public class InchwormController : MonoBehaviour
    {
        [SerializeField] private HingeJoint rearSegment;
        [SerializeField] private HingeJoint frontSegment;

        public void SetFrontSegmentTargetVelocity(float value)
        {
            var temp = frontSegment.motor;
            temp.targetVelocity = value;
            frontSegment.motor = temp;
        }
        
        public void SetRearSegmentTargetVelocity(float value)
        {
            var temp = rearSegment.motor;
            temp.targetVelocity = value;
            rearSegment.motor = temp;
        }

        public void SetColor(Color color)
        {
            rearSegment.GetComponent<MeshRenderer>().material.color = color;
            frontSegment.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
