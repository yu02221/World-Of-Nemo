using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform player;

    public GameObject terrainChunk;

    private Dictionary<ChunkPos, TerrainChunk> buildedChunks = new Dictionary<ChunkPos, TerrainChunk>();
    private int curChunkPosX;
    private int curChunkPosZ;

    public int cWidth;
    public int cHeight;
    public int cDistance;

    public float terrainDetail;
    public float terrainHeight;

    int seed;


    private void Start()
    {
        seed = Random.Range(100000, 999999);

        player = GameObject.Find("Player").transform;

        curChunkPosX = Mathf.FloorToInt(player.position.x / 16);
        curChunkPosZ = Mathf.FloorToInt(player.position.z / 16);

        cWidth = TerrainChunk.chunkWidth;
        cHeight = TerrainChunk.chunkHeight;

        LoadChunk();
    }

    private void Update()
    {
        int curPosX = Mathf.FloorToInt(player.position.x / 16);
        int curPosZ = Mathf.FloorToInt(player.position.z / 16);

        if (curChunkPosX != curPosX || curChunkPosZ != curPosZ)
        {
            curChunkPosX = curPosX;
            curChunkPosZ = curPosZ;
            LoadChunk();
            UnloadChunk();
        }
    }

    private void LoadChunk()
    {
        for (int i = curChunkPosX - cDistance; i < curChunkPosX + cDistance; i++)
        {
            for (int j = curChunkPosZ - cDistance; j < curChunkPosZ + cDistance; j++)
            {
                BuildChunck(i * 16, j * 16);
            }
        }
    }

    private void UnloadChunk()
    {
        List<ChunkPos> toUnload = new List<ChunkPos>();
        foreach (var chunk in buildedChunks)
        {
            ChunkPos cPos = chunk.Key;
            if (Mathf.Abs(curChunkPosX * 16 - cPos.x) > 16 * (cDistance) ||
                Mathf.Abs(curChunkPosZ * 16 - cPos.z) > 16 * (cDistance))
            {
                toUnload.Add(chunk.Key);
            }
        }

        foreach (var cPos in toUnload)
        {
            buildedChunks[cPos].gameObject.SetActive(false);
        }
    }

    private void BuildChunck(int xPos, int zPos)
    {
        TerrainChunk chunk;
        ChunkPos curChunk = new ChunkPos(xPos, zPos);
        if (buildedChunks.ContainsKey(curChunk))
        {
            chunk = buildedChunks[curChunk];
            if (!chunk.gameObject.activeSelf)
            {
                chunk.gameObject.SetActive(true);
            }
        }
        else
        {
            GameObject chunkObj = Instantiate(terrainChunk, new Vector3(xPos, 0, zPos), Quaternion.identity);
            chunkObj.transform.parent = GameObject.Find("Terrain").transform;
            chunk = chunkObj.GetComponent<TerrainChunk>();
            buildedChunks.Add(curChunk, chunk);

            for (int x = 0; x < cWidth + 2; x++)
                for (int z = 0; z < cWidth + 2; z++)
                    for (int y = 0; y < cHeight; y++)
                        chunk.blocks[x, y, z] = GetBlockType(xPos + x - 1, y, zPos + z - 1);
        }

        chunk.BuildMesh();
    }

    public struct ChunkPos
    {
        public int x, z;
        public ChunkPos(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
    }

    private BlockType GetBlockType(int x, int y, int z)
    {
        
        BlockType bt;

        int grassY = (int)(Mathf.PerlinNoise((x / 2 + seed) / terrainDetail, (z / 2 + seed) / terrainDetail) * terrainHeight) +16;
        int soilRange = Random.Range(3, 5);

        if (y > grassY)
            bt = BlockType.Air;
        else if (y == grassY)
            bt = BlockType.Grass;
        else if (y < grassY && y >= grassY - soilRange)
            bt = BlockType.Soil;
        else
            bt = BlockType.Stone;

        return bt;
    }
}
