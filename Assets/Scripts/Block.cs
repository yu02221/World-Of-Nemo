using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Tile top, front, side, bottom;

    public TilePos topPos, frontPos, sidePos, bottomPos;

    public Block(Tile tile)
    {
        top = front = side = bottom = tile;
        GetPositions();
    }

    public Block(Tile top, Tile front, Tile side, Tile bottom)
    {
        this.top = top;
        this.front = front;
        this.side = side;
        this.bottom = bottom;
        GetPositions();
    }

    void GetPositions()
    {
        topPos = TilePos.tiles[top];
        frontPos = TilePos.tiles[front];
        sidePos = TilePos.tiles[side];
        bottomPos = TilePos.tiles[bottom];
    }


    public static Dictionary<BlockType, Block> blocks = new Dictionary<BlockType, Block>(){
        {BlockType.Grass, new Block(Tile.Grass, Tile.GrassSide, Tile.GrassSide, Tile.Dirt)},
        {BlockType.Dirt, new Block(Tile.Dirt)},
        {BlockType.Stone, new Block(Tile.Stone)},
        {BlockType.CobbleStone, new Block(Tile.CobbleStone)},
        {BlockType.OakLog, new Block(Tile.OakLogTop, Tile.OakLogSide, Tile.OakLogSide, Tile.OakLogTop)},
        {BlockType.Leaves, new Block(Tile.Leaves)},
        {BlockType.OakPlanks, new Block(Tile.OakPlanks)},
        {BlockType.Furnace, new Block(Tile.FurnaceTop, Tile.FurnaceFront, Tile.FurnaceSide, Tile.FurnaceTop)},
        {BlockType.Iron, new Block(Tile.Iron)},
        {BlockType.Gold, new Block(Tile.Gold)},
        {BlockType.Diamond, new Block(Tile.Diamond)},
        {BlockType.Coal, new Block(Tile.Coal)},
        {BlockType.CraftingTable, new Block(Tile.CraftingTableTop, Tile.CraftingTableFront, Tile.CraftingTableSide, Tile.OakPlanks)},
    };
}

public enum BlockType 
{
    Air, 
    Dirt, 
    Grass, 
    Stone, 
    CobbleStone, 
    OakLog, 
    Leaves, 
    OakPlanks,
    Furnace,
    Iron,
    Gold,
    Diamond,
    Coal,
    CraftingTable,
}