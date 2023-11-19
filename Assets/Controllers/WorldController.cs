using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldController : MonoBehaviour
{
    
    public static WorldController Instance { get; protected set; }
    public Sprite floorSprite;

    Dictionary<Tile, GameObject> tileGameObjectMap;
    Dictionary<InstalledObject, GameObject> installedObjectGameObjectMap;
    public World World { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;

        World = new World();

        World.RegisterInstalledObjectCreated(OnInstalledObjectCreated);

        //world.RandomizeTiles();

        //create a gameobject for each tile, so they show visually

        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile_data = World.GetTileAt(x, y);
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                //no rush to add a sprite for it now
                tile_go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileTypeChangedCallback((tile)=>{ OnTileTypeChanged(tile, tile_go); });
            }
        }
        World.RandomizeTiles();
    }

    float randomizeTileTimer = 2f;


    // Update is called once per frame
    void Update()
    {
        //randomizeTileTimer -= Time.deltaTime;
        //if(randomizeTileTimer<0)
        //{
        //    World.RandomizeTiles();
        //    randomizeTileTimer = 2f;
        //}
    }

    
    void OnTileTypeChanged(Tile tile_data,GameObject tile_go)
    {
        if(tile_data.Type==Tile.TileType.Floor)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if(tile_data.Type == Tile.TileType.Empty)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("UnTileChanged-Unrecognized TileType");
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        //GameObject.FindObjectOfType<WorldController>();


        return WorldController.Instance.World.GetTileAt(x, y);
    }

    public void OnInstalledObjectCreated(InstalledObject obj)
    {
        //create a visual gameobject linked to this data
    }
}
