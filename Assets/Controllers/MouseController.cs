using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    Tile.TileType buildModeTile = Tile.TileType.Floor;

    bool buildModeIsObjects = false;
    string buildModeObjectType;
    //public Camera
    Vector3 lastFramePosition;
    Vector3 dragStartPosition;

    Vector3 currentFramePosition;
    List<GameObject> dragPreviewGameObjects;


    public GameObject circleCursorPrefab;
    //void UpdateCursor()
    //{

    //    Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currentFramePosition);

    //    if (tileUnderMouse != null)
    //    {
    //        circleCursorPrefab.SetActive(true);
    //        Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
    //        circleCursorPrefab.transform.position = cursorPosition;
    //    }
    //    else
    //    {
    //        circleCursorPrefab.SetActive(false);
    //    }
    //}

    private void Start()
    {
        dragPreviewGameObjects = new List<GameObject>();

        SimplePool.Preload(circleCursorPrefab, 100);
    }

    // Update is called once per frame
    void Update()
    {
        currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentFramePosition.z = 0;

        //UpdateCursor();

        UpdateDragging();


        UpdateCamera();
    
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateDragging()
    {
        //if mouse over a UI element, then bail out of this
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //start drag
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currentFramePosition;
        }

        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currentFramePosition.x);
        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currentFramePosition.y);


        if (end_x < start_x)
        {
            int temp = end_x;
            end_x = start_x;
            start_x = temp;
        }
        if (end_y < start_y)
        {
            int temp = end_y;
            end_y = start_y;
            start_y = temp;
        }

        //clean up old drag preview
        while (dragPreviewGameObjects.Count>0)
        {
            GameObject go = dragPreviewGameObjects[0];
            dragPreviewGameObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            //display a preview of the drag area
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        //display the building hint on top of this tile position
                        GameObject go=SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGameObjects.Add(go);
                    }
                }
            }
        }

        //end drag
        if (Input.GetMouseButtonUp(0))
        {
            

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);

                    
                    if (t != null)
                    {
                        if (buildModeIsObjects == true)
                        {
                            //create installed object and assign it to the file

                            //FIXME:right now we just gonna assume walls
                            WorldController.Instance.World.PlaceInstalledObject(buildModeObjectType, t);

                        }
                        else
                        {
                            //we are in tile changing mode
                            t.Type = buildModeTile;
                        }
                        
                    }
                }
            }
        }
    }

    void UpdateCamera()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))//right or middle button
        {
            Vector3 diff = lastFramePosition - currentFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize*Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 35f);
    }

    public void SetMode_BuildFloor(float s)
    {
        buildModeIsObjects = false;
        buildModeTile = Tile.TileType.Floor;
    }

    public void SetMode_Bulldoze(float s)
    {
        buildModeIsObjects = false;
        buildModeTile = Tile.TileType.Empty;
    }

    public void SetMode_BuildInstalledObject(string objectType)
    {
        buildModeIsObjects = true;
        buildModeObjectType = objectType;
    }
}
