using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHighlights : MonoBehaviour {
    // set access to map highlights from another scripts
    public static MapHighlights Instance { set; get; }

    [SerializeField]
    private GameObject highlightPrefab;
    // a list containing all the highlight prefabs  
    private List<GameObject> highlights;
    [SerializeField]
    private int tileAxisCount = 10;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }
    /* every single highlight object is going to be on / off
     * we use active pool to know if there use or not */
    private GameObject GetHighlightObject()
    {
        // find from highlights list first object which matches this condition
        GameObject go = highlights.Find(g => !g.activeSelf);
        // if no object then we'll instantiate prefab
        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }
    // instantiale all allowed highlight moves 
    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < tileAxisCount; i++)
        {
            for (int j = 0; j < tileAxisCount; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }
    // hide all highlight prefabs
    public void HidelightLights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
