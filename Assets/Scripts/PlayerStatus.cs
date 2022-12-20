using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int standBlockX;
    public int standBlockY;
    public int standBlockZ;

    public int standChunkX;
    public int standChunkZ;

    public LayerMask groundLayer;
    TerrainChunk tc;

    public Text xyz;

    public int hp;
    public int maxHp;

    private void Start()
    {
        GetStandBlock();
    }

    private void Update()
    {
        GetStandBlock();

        xyz.text = string.Format($"X : {standBlockX}\nY : {standBlockY}\nZ : {standBlockZ}");
        
    }

    private void GetStandBlock()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, groundLayer))
        {
            Vector3 targetPos = hitInfo.point;

            standChunkX = Mathf.FloorToInt(targetPos.x / 16f) * 16;
            standChunkZ = Mathf.FloorToInt(targetPos.z / 16f) * 16;

            ChunkPos cp = new ChunkPos(standChunkX, standChunkZ);

            tc = TerrainGenerator.buildedChunks[cp];

            //index of the target block
            standBlockX = Mathf.FloorToInt(targetPos.x);
            standBlockY = Mathf.FloorToInt(transform.position.y - 0.1f);
            standBlockZ = Mathf.FloorToInt(targetPos.z);
        }
    }
}
