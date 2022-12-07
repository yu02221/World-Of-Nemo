using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform player;

    public GameObject soilBlock;
    public GameObject stoneBlock;
    public GameObject grassBlock;

    public int width;

    public float terrainDetail;
    public float terrainHeight;

    int seed;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
        seed = Random.Range(100000, 999999);
        GenerateTerrain(0, 0);
    }

    private void Update()
    {
        if (player.position.x > width && player.position.z > width)
            GenerateTerrain(width, width);
        else if (player.position.x > width)
            GenerateTerrain(width, 0);
        else if (player.position.z > width)
            GenerateTerrain(0, width);
    }

    private void GenerateTerrain(int xPos, int zPos)
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int x = xPos - width; x < xPos + width; x++)
        {
            for (int z = zPos - width; z < zPos + width; z++)
            {
                int maxY = (int)(Mathf.PerlinNoise((x / 2 + seed) / terrainDetail, (z / 2 + seed) / terrainDetail) * terrainHeight);
                GameObject grass = Instantiate(grassBlock, new Vector3(x, maxY, z), Quaternion.identity);
                grass.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);

                for (int y = 0; y < maxY; y++)
                {
                    int soilLayer = Random.Range(1, 5);
                    if (y >= maxY - soilLayer)
                    {
                        GameObject soil = Instantiate(soilBlock, new Vector3(x, y, z), Quaternion.identity);
                        soil.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);
                    }
                    else
                    {
                        GameObject stone = Instantiate(stoneBlock, new Vector3(x, y, z), Quaternion.identity);
                        stone.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);
                    }

                    
                }
            }
        }
    }
}
