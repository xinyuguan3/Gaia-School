using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World
{

    Tile[,] tiles;

    Dictionary<string, InstalledObject> InstalledObjectPrototypes;

    int width;

    public int Width
    {
        get
        {
            return width;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }
    Action<InstalledObject> cbInstalledObjectCreated;



    int height;

    public World(int width=100, int height=100)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];

        for(int x = 0; x <width; x++)
        {
            for(int y=0;y<height;y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

       
    }

    void CreateInstalledObjectPrototypes()
    {
        InstalledObjectPrototypes = new Dictionary<string, InstalledObject>();

        InstalledObject wallPrototype = InstalledObject.CreatePrototype(
            "Wall",
            0,//impassable
            1,//width
            1//height
            );

        InstalledObjectPrototypes.Add("Wall", InstalledObject.CreatePrototype(
                        "Wall",
                        0,//impassable
                        1,//width
                        1//height
                        )
            );
    }

    

    public void RandomizeTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //this is either 0 or 1
                if(UnityEngine.Random.Range(0,2)==1)
                {
                    tiles[x, y].Type = Tile.TileType.Empty;
                }
                else
                {
                    tiles[x, y].Type = Tile.TileType.Floor;
                }
            }
        }
    }

    public Tile GetTileAt(int x,int y)
    {
        //if (tiles[x, y] == null)
        //{
        //    tiles[x, y] = new Tile(this, x, y);
        //}
        if(x>width||x<0||y>height||y<0)
        {
            Debug.LogError("Tile(" + x + "," + y + ")is out of range.");
            return null;
        }

        return tiles[x, y];
    }

    public void PlaceInstalledObject(string objectType,Tile t)
    {
        //TODO: this function assumes 1*1 tiles--change later

        if (InstalledObjectPrototypes.ContainsKey(objectType) == false)
        {
            Debug.LogError("installedobjectprototypes doesnt contain a proto for key:" + objectType);
            return;
        }

        InstalledObject obj=InstalledObject.PlaceInstance(InstalledObjectPrototypes[objectType], t);

        if (cbInstalledObjectCreated != null)
        {
            cbInstalledObjectCreated(obj);
        }
    }

    public void RegisterInstalledObjectCreated(Action<InstalledObject> callbackfunc)
    {
        cbInstalledObjectCreated += callbackfunc;
    }
    public void UnregisterInstalledObjectCreated(Action<InstalledObject> callbackfunc)
    {
        cbInstalledObjectCreated -= callbackfunc;
    }
}
