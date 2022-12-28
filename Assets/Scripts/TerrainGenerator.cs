using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject terrainChunk;

    public static Dictionary<ChunkPos, TerrainChunk> buildedChunks = new Dictionary<ChunkPos, TerrainChunk>();
    private int curChunkPosX;
    private int curChunkPosZ;

    public int cWidth;
    public int cHeight;
    public int cDistance;

    public float terrainDetail;
    public float terrainHeight;

    int seed;
    
    private List<ChunkPos> toGenerate = new List<ChunkPos>();

    private void Start()
    {
        seed = Random.Range(100000, 999999);

        player = GameObject.Find("Player").transform;
        int playerY = (int)(Mathf.PerlinNoise(
            (player.position.x / 2 + seed) / terrainDetail, 
            (player.position.z / 2 + seed) / terrainDetail)
            * terrainHeight) + 19;
        player.position = new Vector3(player.position.x, playerY, player.position.z);

        Instantiate(enemy1);
        Instantiate(enemy2);
        Instantiate(enemy3);

        curChunkPosX = Mathf.FloorToInt(player.position.x / 16);
        curChunkPosZ = Mathf.FloorToInt(player.position.z / 16);

        cWidth = TerrainChunk.chunkWidth;
        cHeight = TerrainChunk.chunkHeight;

        LoadChunk(true);
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

    private void LoadChunk(bool instant = false)
    {
        for (int i = curChunkPosX - cDistance; i <= curChunkPosX + cDistance; i++)
        {
            for (int j = curChunkPosZ - cDistance; j <= curChunkPosZ + cDistance; j++)
            {
                if (instant)
                    BuildChunk(i, j);
                else
                    toGenerate.Add(new ChunkPos(i, j));
            }
        }
        StartCoroutine(DelayBuildChunks());
    }

    private void UnloadChunk()
    {
        List<ChunkPos> toUnload = new List<ChunkPos>();
        foreach (var chunk in buildedChunks)
        {
            ChunkPos cPos = chunk.Key;
            if (Mathf.Abs(curChunkPosX - cPos.x) > (cDistance + 5) ||
                Mathf.Abs(curChunkPosZ - cPos.z) > (cDistance + 5))
            {
                toUnload.Add(chunk.Key);
            }
        }

        while (toUnload.Count > 0)
        {
            buildedChunks[toUnload[0]].gameObject.SetActive(false);
            toUnload.RemoveAt(0);
        }
    }

    private void BuildChunk(int xPos, int zPos)
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
            else
                return;
        }
        else
        {
            GameObject chunkObj = Instantiate(terrainChunk, new Vector3(xPos * 16, 0, zPos * 16), Quaternion.identity);
            chunkObj.transform.parent = GameObject.Find("Terrain").transform;
            chunk = chunkObj.GetComponent<TerrainChunk>();
            buildedChunks.Add(curChunk, chunk);

            for (int x = 0; x < cWidth; x++)
                for (int z = 0; z < cWidth; z++)
                    for (int y = 0; y < cHeight; y++)
                        chunk.blocks[x, y, z] = GetBlockType(xPos * 16 + x - 1, y, zPos * 16 + z - 1);

            
            BuildTrees(chunk.blocks);  

            BuildOres(chunk.blocks);
        }
        chunk.BuildMesh();

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
            bt = BlockType.Dirt;
        else
            bt = BlockType.Stone;

        return bt;
    }

    private void BuildTrees(BlockType[,,] blocks)
    {
        int ranX = Random.Range(5, 16);
        int ranZ = Random.Range(5, 16);
        for (int x = ranX; x < cWidth - 2; x += ranX)
        {
            ranX = Random.Range(5, 16);
            for (int z = ranZ; z < cWidth - 2; z += ranZ)
            {
                ranZ = Random.Range(5, 16);
                for (int y = 1; y < cHeight - 5; y++)
                {
                    if (blocks[x, y - 1, z] == BlockType.Grass && blocks[x, y, z] == BlockType.Air)
                    {
                        int height = Random.Range(4, 7);
                        for (int i = 0; i < height -2; i++)
                        {
                            blocks[x, y + i, z] = BlockType.OakLog;
                        }
                        for (int i = height - 2; i < height; i++)
                        {
                            blocks[x, y + i, z] = BlockType.OakLog;
                            for (int j = -2; j <= 2; j++)
                            {
                                for (int k = -2; k <= 2; k++)
                                {
                                    if (j != 0 || k != 0)
                                        blocks[x + j, y + i, z + k] = BlockType.Leaves;
                                }
                            }
                        }
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                blocks[x + i, y + height, z + j] = BlockType.Leaves;
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildOres(BlockType[,,] blocks)
    {
        BuildDiamond(blocks);
        BuildGold(blocks);
        BuildIron(blocks);
        BuildCoal(blocks);
    }

    private void BuildDiamond(BlockType[,,] blocks)
    {
        int randX = Random.Range(0, 16);
        int randY = Random.Range(1, 16);
        int randZ = Random.Range(0, 16);
        int count = Random.Range(2, 9);
        for (int i = 0; i < 2 && randX + i < cWidth; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2 && randZ + k < cWidth; k++)
                {
                    if (count > 0 && 
                        blocks[randX + i, randY + j, randZ + k] == BlockType.Stone)
                    {
                        blocks[randX + i, randY + j, randZ + k] = BlockType.Diamond;
                        count--;
                    }
                }
            }
        }
    }

    private void BuildGold(BlockType[,,] blocks)
    {
        int randX = Random.Range(0, 16);
        int randZ = Random.Range(0, 16);
        for (int x = randX; x < cWidth; x += randX)
        {
            randX = Random.Range(0, 16);
            for (int z = randZ; z < cWidth; z += randZ)
            {
                randZ = Random.Range(0, 16);
                int randY = Random.Range(1, 20);
                int count = Random.Range(2, 9);
                for (int i = 0; i < 2 && randX + i < cWidth; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2 && randZ + k < cWidth; k++)
                        {
                            if (count > 0 &&
                                blocks[randX + i, randY + j, randZ + k] == BlockType.Stone)
                            {
                                blocks[randX + i, randY + j, randZ + k] = BlockType.Gold;
                                count--;
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildIron(BlockType[,,] blocks)
    {
        int randX = Random.Range(0, 8);
        int randZ = Random.Range(0, 8);
        for (int x = randX; x < cWidth; x += randX)
        {
            randX = Random.Range(0, 8);
            for (int z = randZ; z < cWidth; z += randZ)
            {
                randZ = Random.Range(0, 8);
                int randY = Random.Range(5, 30);
                int count = Random.Range(2, 9);
                for (int i = 0; i < 2 && randX + i < cWidth; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2 && randZ + k < cWidth; k++)
                        {
                            if (count > 0 &&
                                blocks[randX + i, randY + j, randZ + k] == BlockType.Stone)
                            {
                                blocks[randX + i, randY + j, randZ + k] = BlockType.Iron;
                                count--;
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildCoal(BlockType[,,] blocks)
    {
        int randX = Random.Range(0, 4);
        int randZ = Random.Range(0, 4);
        for (int x = randX; x < cWidth; x += randX)
        {
            randX = Random.Range(0, 4);
            for (int z = randZ; z < cWidth; z += randZ)
            {
                randZ = Random.Range(0, 4);
                int randY = Random.Range(10, 30);
                int count = Random.Range(5, 9);
                for (int i = 0; i < 2 && randX + i < cWidth; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2 && randZ + k < cWidth; k++)
                        {
                            if (count > 0 &&
                                blocks[randX + i, randY + j, randZ + k] == BlockType.Stone)
                            {
                                blocks[randX + i, randY + j, randZ + k] = BlockType.Coal;
                                count--;
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DelayBuildChunks()
    {
        while (toGenerate.Count > 0)
        {
            BuildChunk(toGenerate[0].x, toGenerate[0].z);
            toGenerate.RemoveAt(0);

            yield return new WaitForSeconds(.01f);
        }
    }
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
