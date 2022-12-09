using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public Transform player;
    private PlayerStatus ps;

    public LayerMask groundLayer;

    //public Inventory inv;

    public float maxDist = 4;

    private float durability;
    private float curDurability;

    int curBlockX = TerrainChunk.chunkWidth;
    int curBlockY = TerrainChunk.chunkHeight;
    int curBlockZ = TerrainChunk.chunkWidth;

    int bix;
    int biy;
    int biz;

    bool placible;

    TerrainChunk tc;

    private void Start()
    {
        ps = player.GetComponent<PlayerStatus>();
    }

    void Update()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick)
            MiningBlock();
        else if (rightClick)
            PlacingBlock();
    }

    private void MiningBlock()
    {
        if (GetTargetBlock(1))
        {
            if (bix != curBlockX || biy != curBlockY || biz != curBlockZ)
            {
                curDurability = durability;
                curBlockX = bix;
                curBlockY = biy;
                curBlockZ = biz;
            }
            else
            {
                curDurability -= Time.deltaTime;
            }
            
            if (curDurability <= 0)
            {
                curDurability = durability;
                tc.blocks[bix, biy, biz] = BlockType.Air;
                tc.BuildMesh();
            }
        }
    }

    private void PlacingBlock()
    {
        if (GetTargetBlock(-1))
        {
            if (ps.standBlockX - ps.standChunkX != bix ||
                ps.standBlockZ - ps.standChunkZ != biz ||
                ps.standBlockY != biy)
            {
                tc.blocks[bix, biy, biz] = BlockType.Soil;
                tc.BuildMesh();
            }

        }
    }

    private bool GetTargetBlock(int sign)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist + 1, groundLayer))
        {
            Vector3 targetPos = hitInfo.point + transform.forward * .01f * sign;

            int chunkPosX = Mathf.FloorToInt(targetPos.x / 16f) * 16;
            int chunkPosZ = Mathf.FloorToInt(targetPos.z / 16f) * 16;

            ChunkPos cp = new ChunkPos(chunkPosX, chunkPosZ);

            tc = TerrainGenerator.buildedChunks[cp];

            //index of the target block
            bix = Mathf.FloorToInt(targetPos.x) - chunkPosX;
            biy = Mathf.FloorToInt(targetPos.y);
            biz = Mathf.FloorToInt(targetPos.z) - chunkPosZ;

            if (biy >= TerrainChunk.chunkHeight || biy < 0)
                return false;
            GetBlockDurability(tc.blocks[bix, biy, biz]);
            return true;
        }
        else
            return false;
    }

    private void GetBlockDurability(BlockType targetBlock)
    {
        switch (targetBlock)
        {
            case BlockType.Grass:
            case BlockType.Soil:
                durability = 1.0f;
                break;
            case BlockType.Stone:
                durability = 4.0f;
                break;
            default:
                durability = 1;
                break;
        }
    }
}
