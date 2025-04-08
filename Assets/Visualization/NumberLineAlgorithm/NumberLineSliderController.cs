using System;
using System.Collections.Generic;
using System.Globalization;
using Easing;
using GeneticAlgoCore;
using GeneticAlgorithms;
using RKUnityToolkit.UIElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visualization.NumberLineAlgorithm
{
    public class NumberLineSliderController : GenericListDisplay.ListItemController<GeneticValueMatcher.GeneticValueMatcherIndividual>
    {
        private const float EasingDurationSeconds = .5f;
        private const float PauseDurationSeconds = .5f;
        
        [SerializeField] private Slider slider;
        [SerializeField] private Image handle;
        [SerializeField] private Image fill;
        [SerializeField] private TextMeshProUGUI text;

        private readonly Queue<GeneticValueMatcher.GeneticValueMatcherIndividual> _valuesToShow = new();
        private float _timeElapsedForCurrentValue;
        private GeneticValueMatcher.GeneticValueMatcherIndividual _target;
        private GeneticValueMatcher.GeneticValueMatcherIndividual _initial;

        private void Start()
        {
            SetValue(0);
        }

        protected override void OnDataSet(GeneticValueMatcher.GeneticValueMatcherIndividual data)
        {
            if (_target != null)
            {
                _valuesToShow.Enqueue(data);
            }
            else
            {
                Debug.Log("Setting initial Data");
                SetTarget(data);
                SetValue(data.Value);
            }
        }

        private void Update()
        {
            _timeElapsedForCurrentValue += Time.deltaTime;
            
            if (_valuesToShow.TryPeek(out _) && _timeElapsedForCurrentValue > EasingDurationSeconds + PauseDurationSeconds)
            {
                SetTarget(_valuesToShow.Dequeue());
            }

            if (_initial == null)
            {
                SetValue(_target.Value);
            }
            else
            {
                SetValue(_initial.Value + (_target.Value - _initial.Value) * EasingFunctions.EaseInSine(_timeElapsedForCurrentValue / EasingDurationSeconds));
            }
        }

        private void SetTarget(GeneticValueMatcher.GeneticValueMatcherIndividual newTarget)
        {
            _initial = _target;
            _target = newTarget;
            _timeElapsedForCurrentValue = 0;
            SetColors(newTarget);
        }
        
        private void SetValue(float value)
        {
            slider.value = value;
            text.text = Math.Round(value, 3).ToString(CultureInfo.InvariantCulture);
        }

        private void SetColors(GeneticValueMatcher.GeneticValueMatcherIndividual individual)
        {
            var color = individual.GetColor();
            fill.color = color;
            handle.color = color;
        }
    }
}
