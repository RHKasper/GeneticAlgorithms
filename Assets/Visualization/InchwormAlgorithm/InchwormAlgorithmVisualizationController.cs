using GeneticAlgorithms;
using UnityEngine;
using UnityEngine.Serialization;

namespace Visualization.InchwormAlgorithm
{
    public class InchwormAlgorithmVisualizationController : MonoBehaviour
    {
        [SerializeField] private int populationSize = 20;
        [SerializeField] private float frameDurationSeconds = .25f;
        [SerializeField] private int frameCount = 20;
        [SerializeField] private int verticalSpacingMeters = 4;
        
        [SerializeField] private InchwormTestCaseController testCasePrefab;

        private GeneticInchwormMovementAlgorithm _algorithm;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            _algorithm = new GeneticInchwormMovementAlgorithm(frameDurationSeconds, frameCount, testCasePrefab, verticalSpacingMeters, populationSize);
            await _algorithm.RunGeneration();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
