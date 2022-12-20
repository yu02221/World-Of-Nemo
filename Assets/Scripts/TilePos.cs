using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePos
{
    int xPos, yPos;

    Vector2[] uvs;

    public TilePos(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        uvs = new Vector2[]
        {
            new Vector2(xPos/10f + .001f, yPos/4f + .001f),
            new Vector2(xPos/10f+ .001f, (yPos+1)/4f - .001f),
            new Vector2((xPos+1)/10f - .001f, (yPos+1)/4f - .001f),
            new Vector2((xPos+1)/10f - .001f, yPos/4f+ .001f),
        };
    }

    public Vector2[] GetUVs()
    {
        return uvs;
    }


    public static Dictionary<Tile, TilePos> tiles = new Dictionary<Tile, TilePos>()
    {
        {Tile.Grass,                new TilePos(0,0)},
        {Tile.GrassSide,            new TilePos(0,1)},
        {Tile.Dirt,                 new TilePos(0,2)},
        {Tile.Stone,                new TilePos(1,0)},
        {Tile.CobbleStone,          new TilePos(1,1)},
        {Tile.OakLogSide,           new TilePos(2,0)},
        {Tile.OakLogTop,            new TilePos(2,1)},
        {Tile.Leaves,               new TilePos(2,2)},
        {Tile.OakPlanks,            new TilePos(3,0)},
        {Tile.FurnaceTop,           new TilePos(4,0)},
        {Tile.FurnaceSide,          new TilePos(4,1)},
        {Tile.FurnaceFront,         new TilePos(4,2)},
        {Tile.FurnaceUsed,          new TilePos(4,3)},
        {Tile.Iron,                 new TilePos(5,0)},
        {Tile.Gold,                 new TilePos(5,1)},
        {Tile.Diamond,              new TilePos(5,2)},
        {Tile.Coal,                 new TilePos(5,3)},
        {Tile.CraftingTableTop,     new TilePos(6,0)},
        {Tile.CraftingTableFront,   new TilePos(6,1)},
        {Tile.CraftingTableSide,    new TilePos(6,2)},
    };
}

public enum Tile 
{
    Dirt, Grass, GrassSide, 
    Stone,
    CobbleStone,
    OakLogTop, OakLogSide, 
    Leaves,
    OakPlanks,
    FurnaceTop, FurnaceSide, FurnaceFront, FurnaceUsed,
    Iron,
    Gold,
    Diamond,
    Coal,
    CraftingTableTop, CraftingTableFront, CraftingTableSide,
}
