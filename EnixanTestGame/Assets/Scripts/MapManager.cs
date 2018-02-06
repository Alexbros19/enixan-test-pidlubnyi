using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
    public static MapManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }
    // we're going to be able to fetch objects using X and Y coordinates
    public MapObject[,] MapObjects { get; set; }
    // current selected object
    private MapObject selectedObject;
    
    // const tile size of 1 meter and offset of 0.5 meter
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float LINE_WIDTH = 0.02f;
    // value of raycast fucntion parameter;
    private const float MAX_DISTANCE = 25.0f;
    // tile axis counter of grid
    [SerializeField]
    private int tileAxisCount = 10;
    // we'll set -1 if no tile is selected 
    private int selectionX = -1;
    private int selectionY = -1; 
    // list of objects prefabs
    [SerializeField]
    private List<GameObject> objectPrefab;
    // active list of object 
    private List<GameObject> activeObject;
    // using own material for creating line 
    [SerializeField]
    private Material lineMaterial;

    private void Awake()
    {
        Instance = this;
        activeObject = new List<GameObject>();
        MapObjects = new MapObject[tileAxisCount, tileAxisCount];
        // spawn 10 objects on the corners of map
        SpawnObject(0, 0, 0);
        SpawnObject(1, 0, 4);
        SpawnObject(0, 0, 5);
        SpawnObject(1, 0, 9);
        SpawnObject(0, 1, 9);
        SpawnObject(0, 2, 9);
        SpawnObject(1, 9, 4);
        SpawnObject(0, 9, 1);
        SpawnObject(1, 4, 0);
        SpawnObject(1, 5, 0);
        // call drawing grid function
        DrawMap();
    }

    private void Update()
    {
        // call update selection function
        UpdateSelection();
        // using input for select object
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedObject == null)
                {
                    // Select the object
                    SelectMapObject(selectionX, selectionY);
                }
                else
                {
                    // Move the object
                    MoveMapObject(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectMapObject(int x, int y)
    {
        // if magazine window is not active
        if (!GameUIManager.IsMagazineInventoryActive)
        {
            if (MapObjects[x, y] == null)
                return;
            // call SetObjectSelected fucntion
            SetObjectSelected(x, y);
        }
    }

    private void MoveMapObject(int x, int y)
    {
        // if no another object in place where we put selected object 
        if (allowedMoves[x, y])
        {
            MapObjects[selectedObject.CurrentX, selectedObject.CurrentY] = null;
            selectedObject.transform.position = GetTileCenter(x, y);
            selectedObject.SetPosition(x, y);
            MapObjects[x, y] = selectedObject;
        }
        // stop play animation when move object
        selectedObject.GetComponent<Animation>().Stop();
        // reset moved object scale to default 
        selectedObject.transform.localScale = new Vector3(1, 1, 1);
        // hide object info title
        selectedObject.transform.GetChild(0).gameObject.SetActive(false);
        // hide map highlights
        MapHighlights.Instance.HidelightLights();
        // reset selected object to null 
        selectedObject = null;
    }
    // set parameters and draw single line 
    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        // set line game objects to Lines Holder object
        myLine.transform.parent = GameObject.FindGameObjectWithTag("LinesHolder").transform;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.startColor = Color.black;
        lr.endColor = Color.black;
        lr.startWidth = LINE_WIDTH;
        lr.endWidth = LINE_WIDTH;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    // draw a map grid
    private void DrawMap()
    {
        // width and height of map grid
        Vector3 widthLine = Vector3.right * tileAxisCount;
        Vector3 heightLine = Vector3.forward * tileAxisCount;
        // draw grid 
        for (int i = 0; i <= tileAxisCount; i++)
        {
            // draw horizontal lines
            Vector3 start = Vector3.forward * i;
            DrawLine(start, start + widthLine, Color.black);
            for (int j = 0; j <= tileAxisCount; j++)
            {
                // draw vertical lines
                start = Vector3.right * j;
                DrawLine(start, start + heightLine, Color.black);
            }
        }

        // Draw the selection in debug mode
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1),
                Color.black);
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1), 
                Color.black);
        }
    }
    // function for updating current selection of object
    private void UpdateSelection()
    {
        // if there is no camera then return
        if (!Camera.main)
            return;
        // use raycast hit system for back resoult of hit collision  
        // we only hit a map using layer mask
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, MAX_DISTANCE, LayerMask.GetMask("Map Plane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    // spawn object function
    private void SpawnObject(int index, int x, int y)
    {
        // create object 
        GameObject go = Instantiate(objectPrefab[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        MapObjects[x, y] = go.GetComponent<MapObject>();
        MapObjects[x, y].SetPosition(x, y);
        activeObject.Add(go);
    }
    // set object in centre of tile
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void SetObjectSelected(int x, int y)
    {
        allowedMoves = MapObjects[x, y].PossibleMove();
        selectedObject = MapObjects[x, y];
        // play animation when select object
        selectedObject.GetComponent<Animation>().Play();
        // set visible description title for selected object
        selectedObject.transform.GetChild(0).gameObject.SetActive(true);
        // change id value for selected description of object
        GameObject.FindGameObjectWithTag("ObjectIDText").GetComponent<Text>().text = "ID : " + x.ToString() + y.ToString();
        MapHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }
    
    // spawn tree object from inventory
    public void SpawnTreeObject()
    {
        MapObject m;

        for (int i = 0; i < tileAxisCount; i++)
        {
            for (int j = 0; j < tileAxisCount; j++)
            {
                m = MapManager.Instance.MapObjects[i, j];
                if (m == null)
                {
                    SpawnObject(1, i, j);
                    SetObjectSelected(i, j);
                    return;
                }
            }
        }
    }
    // spawn stones object from inventory
    public void SpawnStonesObject()
    {
        MapObject m;

        for (int i = 0; i < tileAxisCount; i++)
        {
            for (int j = 0; j < tileAxisCount; j++)
            {
                m = MapManager.Instance.MapObjects[i, j];
                if (m == null)
                {
                    SpawnObject(0, i, j);
                    SetObjectSelected(i, j);
                    return;
                }
            }
        }
    }
}
