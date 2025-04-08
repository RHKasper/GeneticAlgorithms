using System.Threading.Tasks;
using GeneticAlgorithms;
using UnityEngine;
using UnityEngine.Serialization;

namespace Visualization.InchwormAlgorithm
{
    public class InchwormAlgorithmVisualizationController : MonoBehaviour
    {
        [SerializeField] private int numGenerations = 20;
        [SerializeField] private int populationSize = 20;
        [SerializeField] private float frameDurationSeconds = .25f;
        [SerializeField] private int frameCount = 20;
        [SerializeField] private int verticalSpacingMeters = 4;
        [SerializeField] private Vector2 motorVelocityRange = new Vector2(-200, 200);
        
        [SerializeField] private InchwormTestCaseController testCasePrefab;

        private GeneticInchwormMovementAlgorithm _algorithm;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            _algorithm = new GeneticInchwormMovementAlgorithm(frameDurationSeconds, frameCount, motorVelocityRange, testCasePrefab, verticalSpacingMeters, populationSize);

            for (int i = 0; i < numGenerations; i++)
            {
                await _algorithm.RunGeneration();
            }
        }
    }
}
