using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//installed objects are things like walls doors furnitures etc.
public class InstalledObject
{
    //this represents the base tile of object, but in practice large objects may actually occupy
    //multile tiles.
    Tile tile;

    //this obectType will be queried by the visual system to know what sprite to render for this object
    public string objectType
    {
        get; protected set;
    }

    //this is a multipler. So a value of '2' here, means you move twice as slowly(i.e. at half speed)
    //tile types and other environmental effects may be combined.
    //for example, a "rough" tile (cost of 2) with a table (cost of 3) that is on fire(cost of 3)
    //would have a total movement cost of (2+3+3=8), so you move through this tile at 1/8th normal speed.
    //Special: if movementCost=0, then this tile is impassible.(e.g. a wall).
    float movementCost = 1f;

    //for example, a sofa might be 3*2(actual graphics only appear to cover the 3*1 area, but the extra row is for leg room
    int width ;
    int height ;

    //TODO:implement larget object
    //TODO:implement object rotation

    protected InstalledObject()
    {

    }

    //this is basivally used by our object factory to create the prototypical object
    static public InstalledObject CreatePrototype(string objectType,float movementCost=1f, int width=1,int height=1)        
    {
        InstalledObject obj = new InstalledObject();
        obj.objectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;

        return obj;
    }

    static public InstalledObject PlaceInstance(InstalledObject proto,Tile tile)
    {
        InstalledObject obj = new InstalledObject();
        obj.objectType = proto.objectType;
        obj.movementCost = proto.movementCost;
        obj.width = proto.width;
        obj.height = proto.height;

        obj.tile = tile;

        //FIXME:this assumes we are 1*1.
        if (tile.PlaceObject(obj)==false)
        {
            //for some reason, we werent able to place our obj in this tile.
            //(probolly it was already occupied.)

            //dont return our newly instatiated object.
            //(it will be garbage collected)
            return null;
        }

        return obj;
    }


}
