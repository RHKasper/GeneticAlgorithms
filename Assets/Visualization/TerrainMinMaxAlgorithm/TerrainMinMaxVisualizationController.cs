using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithms;
using UnityEngine;
using Vector2Extensions;
using Vector3Extensions;

namespace Visualization.TerrainMinMaxAlgorithm
{
    public class TerrainMinMaxVisualizationController : MonoBehaviour
    {
        [SerializeField] private TerrainCollider terrainCollider;
        [SerializeField] private int numberOfGenerations = 25;
        [SerializeField] private int populationSize = 25;

        private GeneticTerrainMinMaxFinder _geneticTerrainMinMaxFinder;
        private List<MeshRenderer> _spheres = new();
        
        private void Start()
        {
            _geneticTerrainMinMaxFinder = new GeneticTerrainMinMaxFinder(populationSize, GeneticTerrainMinMaxFinder.MinOrMax.Max, terrainCollider);

            for (int i = 0; i < populationSize; i++)
            {
                _spheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshRenderer>());
                _spheres[^1].transform.localScale = new Vector3(15, 15, 15);
            }
        }

        private async void Update()
        {
            if (Input.anyKeyDown && _geneticTerrainMinMaxFinder.CurrentGenerationNumber < numberOfGenerations)
            {
                var previousFitnesses = await _geneticTerrainMinMaxFinder.RunGeneration();
                
                for (int i = 0; i < previousFitnesses.Count; i++)
                {
                    var (fitness, individual) = previousFitnesses[i];
                    var sphere = _spheres[i];

                    sphere.transform.position = individual.XZCoords.XOY() + Vector3.up * (_geneticTerrainMinMaxFinder.MinimumOrMaximum == GeneticTerrainMinMaxFinder.MinOrMax.Max ? fitness : -fitness);
                    sphere.material.color = individual.GetColor();
                }
            }
        }
    }
}
