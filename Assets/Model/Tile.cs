using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile 
{
    Action<Tile> cbTileTypeChanged;
    GameObject myVisualTileGameObject;

    public enum TileType{
        Empty,
        Floor
    }
    
    private TileType type=TileType.Empty;

    public TileType Type
    {
        get
        {
            return type;
        }
        set
        {
            TileType oldType = type;
            type = value;
            //call the callback, let things know we changed
            if(cbTileTypeChanged!=null&&oldType!=type)
                cbTileTypeChanged(this);
        }
    }

    public int X {
        get
        {
            return x;
        }
    }
    public int Y { get => y; }

    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    int x;
    int y;

    public Tile(World world,int x,int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;

        
    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

    public bool PlaceObject(InstalledObject objInstance)
    {
        if (objInstance == null)
        {
            //we are uninstalling whatever was here before
            installedObject = null;
            return true;
        }

        //objInstance isn't null
        if (installedObject != null)
        {
            Debug.LogError("try to assign an installed object to a tile that already has one!");
            return false;
        }

        //at this point, everything is fine!

        installedObject = objInstance;
        return true;
    }

    
}
