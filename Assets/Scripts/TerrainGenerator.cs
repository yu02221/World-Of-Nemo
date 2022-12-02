using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform player;

    public GameObject terrainChunk;

    FastNoise noise = new FastNoise();

    public int width = 16;
    public int height = 64;

    public float terrainDetail;
    public float terrainHeight;

    int seed;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
        seed = Random.Range(100000, 999999);

        for (int i = -4; i < 4; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                BuildChunck(i * 16, j * 16);
            }
        }
    }

    private void Update()
    {
        
    }

    private void BuildChunck(int xPos, int zPos)
    {
        TerrainChunk chunk;

        GameObject chunkObj = Instantiate(terrainChunk, new Vector3(xPos, 0, zPos), Quaternion.identity);
        chunkObj.transform.parent = GameObject.Find("Environment").transform;
        chunk = chunkObj.GetComponent<TerrainChunk>();


        for (int x = 0; x < TerrainChunk.chunkWidth + 2; x++)
        { 
            for (int z = 0; z < TerrainChunk.chunkWidth + 2; z++)
            {
                for (int y = 0; y < TerrainChunk.chunkHeight; y++)
                {
                    //if(Mathf.PerlinNoise((xPos + x-1) * .1f, (zPos + z-1) * .1f) * 10 + y < TerrainChunk.chunkHeight * .5f)
                    chunk.blocks[x, y, z] = GetBlockType(xPos + x - 1, y, zPos + z - 1);
                }
            }
        }

        chunk.BuildMesh();
    }

    private BlockType GetBlockType(int x, int y, int z)
    {
        
        BlockType bt;

        int grassY = (int)(Mathf.PerlinNoise((x / 2 + seed) / terrainDetail, (z / 2 + seed) / terrainDetail) * terrainHeight) +16;
        int soilRange = Random.Range(1, 5);

        if (y > grassY)
            bt = BlockType.Air;
        else if (y == grassY)
            bt = BlockType.Grass;
        else if (y < grassY && y >= grassY - soilRange)
            bt = BlockType.Soil;
        else
            bt = BlockType.Stone;

        return bt;
        /*
        float simplex1 = noise.GetSimplex(x * .8f, z * .8f) * 10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = TerrainChunk.chunkHeight * .5f + heightMap;

        //3d noise for caves and overhangs and such
        float caveNoise1 = noise.GetPerlinFractal(x * 5f, y * 10f, z * 5f);
        float caveMask = noise.GetSimplex(x * .3f, z * .3f) + .3f;

        //stone layer heightmap
        float simplexStone1 = noise.GetSimplex(x * 1f, z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(x * 5f, z * 5f) + .5f) * 20 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = TerrainChunk.chunkHeight * .25f + stoneHeightMap;


        BlockType blockType = BlockType.Air;

        if (y <= baseLandHeight)
        {
            blockType = BlockType.Soil;

            //just on the surface, use a grass type
            if (y > baseLandHeight - 1)
                blockType = BlockType.Grass;

            if (y <= baseStoneHeight)
                blockType = BlockType.Stone;
        }


        if (caveNoise1 > Mathf.Max(caveMask, .2f))
            blockType = BlockType.Air;

        return blockType;
        */
    }
}
