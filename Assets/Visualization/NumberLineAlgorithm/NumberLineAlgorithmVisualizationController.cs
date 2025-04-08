using System;
using System.Threading.Tasks;
using GeneticAlgorithms;
using RKUnityToolkit.UIElements;
using UnityEngine;

namespace Visualization.NumberLineAlgorithm
{
    public class NumberLineAlgorithmVisualizationController : MonoBehaviour
    {
        [SerializeField] private GenericListDisplay genericListDisplay;
        [SerializeField] private NumberLineSliderController numberLineSliderPrefab;
        [SerializeField] private float targetValue = 6.5f;
        [SerializeField] private int numberOfGenerations = 25;

        private GeneticValueMatcher _geneticValueMatcher;
        
        private void Start()
        {
            _geneticValueMatcher = new GeneticValueMatcher(20, targetValue);
            genericListDisplay.DisplayList(_geneticValueMatcher.GetCopyOfIndividualsList(), numberLineSliderPrefab);
        }

        private async void Update()
        {
            if (_geneticValueMatcher.CurrentGenerationNumber < numberOfGenerations)
            {
                await _geneticValueMatcher.RunGeneration();
                genericListDisplay.DisplayList(_geneticValueMatcher.GetCopyOfIndividualsList(), numberLineSliderPrefab);
            }
        }
    }
}
