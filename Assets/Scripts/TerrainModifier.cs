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

    TerrainChunk tc;

    public BlockType placeBlock;

    public Inventory hotInven;
    public Item item;
    public ItemSet itemSet;

    public int curSlot = 0;

    private void Start()
    {
        ps = player.GetComponent<PlayerStatus>();
        SetCurSlot(curSlot);
    }

    private void Update()
    {
        MouseInput();
        HotkeyInput();
        ScrollInput();
        print(curSlot);
    }

    private void MouseInput()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick)
        {
            if (GetTargetBlock(1))
                MiningBlock();
            else
                player.GetComponent<PlayerMove>().Attack();
        }
        else if (rightClick)
        {
            if (hotInven.slots[curSlot].item != null &&
                hotInven.slots[curSlot].item.itemType == Item.ItemType.Block &&
                GetTargetBlock(-1))
                PlacingBlock();
        }
    }

    private void HotkeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCurSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCurSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetCurSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetCurSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SetCurSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SetCurSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SetCurSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SetCurSlot(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SetCurSlot(8);
    }

    private void ScrollInput()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput < 0)
        {
            if (curSlot == hotInven.slots.Length - 1)
                SetCurSlot(0);
            else
                SetCurSlot(curSlot + 1);
        }
        else if (wheelInput > 0)
        {
            if (curSlot == 0)
                SetCurSlot(hotInven.slots.Length - 1);
            else
                SetCurSlot(curSlot - 1);
        }
            
    }

    private void SetCurSlot(int index)
    {
        curSlot = index;
        for (int i = 0; i < hotInven.slots.Length; i++)
        {
            if (i == curSlot)
                hotInven.slots[i].selected.SetActive(true);
            else
                hotInven.slots[i].selected.SetActive(false);
        }
    }

    private void MiningBlock()
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
            GetItem(tc.blocks[bix, biy, biz]);
            tc.blocks[bix, biy, biz] = BlockType.Air;
            tc.BuildMesh();
        }
    }

    private void PlacingBlock()
    {
        if (ps.standBlockX - ps.standChunkX != bix ||
                ps.standBlockZ - ps.standChunkZ != biz ||
                ps.standBlockY != biy)
        {
            tc.blocks[bix, biy, biz] = hotInven.slots[curSlot].item.blockType;
            if (--hotInven.slots[curSlot].itemCount == 0)
                hotInven.slots[curSlot].item = null;
            hotInven.slots[curSlot].SetItemCountText();
            tc.BuildMesh();
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
            case BlockType.Dirt:
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

    private void GetItem(BlockType block)
    {
        switch (block)
        {
            case BlockType.Grass:
            case BlockType.Dirt:
                item = itemSet.dirt;
                break;
            case BlockType.Stone:
            case BlockType.CobbleStone:
                item = itemSet.cobbleStone;
                break;
            case BlockType.OakLog:
                item = itemSet.oakLog;
                break;
            case BlockType.Coal:
                item = itemSet.coal;
                break;
            case BlockType.Diamond:
                item = itemSet.diamond;
                break;
            case BlockType.Iron:
                item = itemSet.rawIron;
                break;
            case BlockType.Gold:
                item = itemSet.rawGold;
                break;
            case BlockType.Furnace:
                item = itemSet.furnace;
                break;
            case BlockType.CraftingTable:
                item = itemSet.craftingTable;
                break;
            case BlockType.OakPlanks:
                item = itemSet.oakPlanks;
                break;
            default:
                item = itemSet.dirt;
                break;
        }
        
        hotInven.AddItem(item);
    }
}
