using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {
    [SerializeField]
    private Sprite[] gridButtonSprites;
    [SerializeField]
    private Sprite[] magazineButtonSprites;
    // line holder object which consist of created lines
    [SerializeField]
    private GameObject lineHolder;
    // magazine inventory object
    [SerializeField]
    private GameObject magazineInventory;
    // counters using for switching control buttons 
    private int clickGridButtonCounter;
    private int clickMagazineButtonCounter;
    // check is magazine inventory window is active
    private static bool isMagazineInventoryActive;

    public static bool IsMagazineInventoryActive
    {
        get
        {
            return isMagazineInventoryActive;
        }

        set
        {
            isMagazineInventoryActive = value;
        }
    }

    private void Start()
    {
        clickGridButtonCounter = 0;
        clickMagazineButtonCounter = 0;
        // on start game hide inventory window and lines grid
        magazineInventory.SetActive(false);
        lineHolder.SetActive(false);
        IsMagazineInventoryActive = false;
    }
    // function for switch on / off of grid 
    public void GridButton()
    {
        clickGridButtonCounter++;
        // when we click on grid button
        if (clickGridButtonCounter == 1)
        {
            lineHolder.SetActive(true);
            // change button image resource
            GameObject.FindGameObjectWithTag("GridButton").GetComponent<Image>().sprite = gridButtonSprites[0];
        }
        else
        {
            lineHolder.SetActive(false);
            GameObject.FindGameObjectWithTag("GridButton").GetComponent<Image>().sprite = gridButtonSprites[1];
            clickGridButtonCounter = 0;
        }
        
    }
    // function for switch on / off of inventory window 
    public void MagazineButton()
    {
        clickMagazineButtonCounter++;

        if (clickMagazineButtonCounter == 1)
        {
            magazineInventory.SetActive(true);
            IsMagazineInventoryActive = true;
            GameObject.FindGameObjectWithTag("MagazineButton").GetComponent<Image>().sprite = magazineButtonSprites[0];
        }
        else
        {
            magazineInventory.SetActive(false);
            IsMagazineInventoryActive = false;
            GameObject.FindGameObjectWithTag("MagazineButton").GetComponent<Image>().sprite = magazineButtonSprites[1];
            clickMagazineButtonCounter = 0;
        }
    }
}
