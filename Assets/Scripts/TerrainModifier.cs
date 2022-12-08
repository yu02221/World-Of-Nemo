using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBlock : MonoBehaviour
{
    public LayerMask groundLayer;

    //public Inventory inv;

    public float maxDist = 4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
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
    }
}
