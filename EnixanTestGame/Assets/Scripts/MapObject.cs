using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // info where object pasted on the map
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }

    [SerializeField]
    private int tileAxisCount = 10;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }
    // check is current move is possible. if true then return X Y coordinates
    public bool[,] PossibleMove()
    {
        MapObject m;
        bool[,] r = new bool[tileAxisCount, tileAxisCount];

        for (int i = 0; i < tileAxisCount; i++)
        {
            for (int j = 0; j < tileAxisCount; j++)
            {
                m = MapManager.Instance.MapObjects[i, j];
                if (m == null)
                {
                    r[i, j] = true;
                }
            }
        }

        return r;
    }
}
