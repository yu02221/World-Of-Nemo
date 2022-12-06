using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public LayerMask groundLayer;

    //public Inventory inv;

    public float maxDist = 4;

    public float durability = 1;

    int chunkPosX = -1;
    int chunkPosZ = -1;

    void Update()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick)
            MiningBlock();
        else if (rightClick)
            PlacingBlock();

        /*
        if (leftClick || rightClick)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist, groundLayer))
            {
                Vector3 pointInTargetBlock;

                //destroy
                if (leftClick)
                    pointInTargetBlock = hitInfo.point + transform.forward * .01f;//move a little inside the block
                else
                    pointInTargetBlock = hitInfo.point - transform.forward * .01f;
                //get the terrain chunk (can't just use collider)
                int chunkPosX = Mathf.FloorToInt(pointInTargetBlock.x / 16f) * 16;
                int chunkPosZ = Mathf.FloorToInt(pointInTargetBlock.z / 16f) * 16;

                print("X :" + chunkPosX);
                print("Z :" + chunkPosZ);
                ChunkPos cp = new ChunkPos(chunkPosX, chunkPosZ);

                TerrainChunk tc = TerrainGenerator.buildedChunks[cp];

                //index of the target block
                int bix = Mathf.FloorToInt(pointInTargetBlock.x) - chunkPosX;
                int biy = Mathf.FloorToInt(pointInTargetBlock.y);
                int biz = Mathf.FloorToInt(pointInTargetBlock.z) - chunkPosZ;

                if (leftClick)//replace block with air
                {
                    //inv.AddToInventory(tc.blocks[bix, biy, biz]);
                    tc.blocks[bix, biy, biz] = BlockType.Air;
                    tc.BuildMesh();
                }
                else if (rightClick)
                {
                    tc.blocks[bix, biy, biz] = BlockType.Soil;

                    tc.BuildMesh();
                }
            }
        }
        */
    }

    private void MiningBlock()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist, groundLayer))
        {
            Vector3 targetBlock = hitInfo.point + transform.forward * .01f;

            int ChunkPosX = Mathf.FloorToInt(targetBlock.x / 16f) * 16;
            int ChunkPosZ = Mathf.FloorToInt(targetBlock.z / 16f) * 16;

            ChunkPos cp = new ChunkPos(chunkPosX, chunkPosZ);

            TerrainChunk tc = TerrainGenerator.buildedChunks[cp];

            //index of the target block
            int bix = Mathf.FloorToInt(targetBlock.x) - chunkPosX;
            int biy = Mathf.FloorToInt(targetBlock.y);
            int biz = Mathf.FloorToInt(targetBlock.z) - chunkPosZ;
            if (false)
            {
                
                durability = 1;
            }
            else
            {
                durability -= Time.deltaTime;
            }

            if (durability <= 0)
            {
                tc.blocks[bix, biy, biz] = BlockType.Air;
                tc.BuildMesh();
            }
        }

        
    }

    private void PlacingBlock()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist + 1, groundLayer))
        {

        }
    }
}
