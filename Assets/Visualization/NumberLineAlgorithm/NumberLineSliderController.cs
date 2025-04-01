using System;
using System.Globalization;
using RKUnityToolkit.UIElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ValueMatcherTest;

namespace Visualization.NumberLineAlgorithm
{
    public class NumberLineSliderController : GenericListDisplay.ListItemController<GeneticValueMatcherIndividual>
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;

        protected override void OnDataSet(GeneticValueMatcherIndividual data)
        {
            slider.value = data.Value;
            text.text = Math.Round(data.Value, 2).ToString(CultureInfo.InvariantCulture);
        }
    }
}
