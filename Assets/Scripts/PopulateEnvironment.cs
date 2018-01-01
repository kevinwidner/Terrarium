using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateEnvironment : MonoBehaviour
{
    public GameObject[] Plants;
    public int VegetationDensitiy = 10;

    // Use this for initialization
    void Start()
    {
        Vegetate();
    }

    private void Vegetate()
    {

        Terrain terrain = transform.GetComponent<Terrain>();
        Vector3 groundSize = terrain.terrainData.size;// Vector3.Scale(transform.localScale, transform.GetComponent<MeshFilter>().mesh.bounds.size);

        for (int i = 0; i < VegetationDensitiy; i++)
        {
            float randomX = Random.Range(-(groundSize.x / 2), groundSize.x / 2);
            float randomZ = Random.Range(-(groundSize.z / 2), groundSize.z / 2);
            float positionY = Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 randomSpot = new Vector3(randomX, positionY, randomZ);
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject plant = Instantiate(getPlant(), randomSpot, randomRotation);
            plant.transform.SetParent(transform, true);
        }
    }

    private GameObject getPlant()
    {
        return Plants[Random.Range(0, Plants.Length)];
    }
}
