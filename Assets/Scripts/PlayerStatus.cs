using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    PlayerMove pm;

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
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public int hunger;
    public int maxHunger;
    private float hungerTime;
    public Image[] hungers;
    public Sprite fullHunger;
    public Sprite halfHunger;
    public Sprite emptyHunger;


    private void Start()
    {
        pm = GetComponent<PlayerMove>();
        GetStandBlock();
    }

    private void Update()
    {
        GetStandBlock();

        xyz.text = string.Format($"X : {standBlockX}\nY : {standBlockY}\nZ : {standBlockZ}");

        if (pm.playerState == PlayerState.Run || pm.playerState == PlayerState.Jump)
            hungerTime += 0.1f * Time.deltaTime;
        else
            hungerTime += 0.01f * Time.deltaTime;

        if (hungerTime >= 1f)
        {
            hunger--;
            SetHunger();
            hungerTime = 0;
        }
    }

    private void GetStandBlock()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, groundLayer))
        {
            Vector3 targetPos = hitInfo.point;

            standChunkX = Mathf.FloorToInt(targetPos.x / 16f);
            standChunkZ = Mathf.FloorToInt(targetPos.z / 16f);

            ChunkPos cp = new ChunkPos(standChunkX, standChunkZ);

            tc = TerrainGenerator.buildedChunks[cp];

            //index of the target block
            standBlockX = Mathf.FloorToInt(targetPos.x);
            standBlockY = Mathf.FloorToInt(transform.position.y - 0.1f);
            standBlockZ = Mathf.FloorToInt(targetPos.z);
        }
    }

    public void SetHp()
    {
        if (hp > maxHp)
            hp = maxHp;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp / 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i == hp / 2)
            {
                if (hp % 2 == 1)
                    hearts[i].sprite = halfHeart;
                else
                    hearts[i].sprite = emptyHeart;
            }
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    public void SetHunger()
    {
        if (hunger > maxHunger)
            hunger = maxHunger;

        for (int i = 0; i < hungers.Length; i++)
        {
            if (i < hunger / 2)
            {
                hungers[i].sprite = fullHunger;
            }
            else if (i == hunger / 2)
            {
                if (hunger % 2 == 1)
                    hungers[i].sprite = halfHunger;
                else
                    hungers[i].sprite = emptyHunger;
            }
            else
                hungers[i].sprite = emptyHunger;
        }
    }
}
