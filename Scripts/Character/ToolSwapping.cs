using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwapping : MonoBehaviour
{

    public GameObject cam;

    public bool working;

    public RaycastHit hit;

    public GameObject[] tools;

    public Animation wellAnimation;

    public int waterInCan;

    public enum Toolstates
    {
        Hoe,
        Water,
        Shovel,
        Scythe

    }
    public Toolstates toolstates;

    void Start()
    {
        toolstates = Toolstates.Hoe;

        wellAnimation = gameObject.GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if ((int)toolstates == 0)
            {
                toolstates = (Toolstates)System.Enum.GetValues(typeof(Toolstates)).Length - 1;
            }
            else
            {
                toolstates--;
            }
            SwapTool();

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if ((int)toolstates == System.Enum.GetValues(typeof(Toolstates)).Length - 1)
            {
                toolstates = (Toolstates)0;
            }
            else
            {
                toolstates++;
            }
            SwapTool();
        }
        Vector3 direction = cam.transform.forward;

        if (Input.GetButtonDown("Fire1") && working == false && Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.tag == "FarmTile")
        {
            Working();
        }
        if (Input.GetButtonDown("Fire1") && working == false && Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.tag == "Well")
        {
            if (waterInCan == 0)
            {
                switch (toolstates)
                {
                    case Toolstates.Water:
                        waterInCan = 15;
                        wellAnimation.Play();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This swaps the currently held tool
    /// </summary>
    public void SwapTool()
    {
        foreach (GameObject tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)toolstates].SetActive(true);
        wellAnimation = tools[(int)toolstates].GetComponent<Animation>();

    }

    /// <summary>
    /// This handles the logic for all the the tools
    /// </summary>
    public void Working()
    {
        switch (toolstates)
        {
            case Toolstates.Hoe:
                hit.transform.gameObject.GetComponent<Ground>().WorkTheGround(Ground.Toolstages.Hoe);
                wellAnimation.Play();
                break;
            case Toolstates.Water:
                if (waterInCan > 0)
                {
                    waterInCan--;
                    hit.transform.gameObject.GetComponent<Ground>().WorkTheGround(Ground.Toolstages.Water);
                    wellAnimation.Play();
                }
                break;
            case Toolstates.Shovel:

                int seedSlot = GetComponent<InventoryManager>().slotIndex;

                if (GetComponent<InventoryManager>().inventorySlots[seedSlot].itemS != null)
                {
                    hit.transform.gameObject.GetComponent<PlantGrowthManager>().item = GetComponent<InventoryManager>().inventorySlots[seedSlot].itemS;

                    hit.transform.gameObject.GetComponent<Ground>().WorkTheGround(Ground.Toolstages.Shovel);

                    GetComponent<InventoryManager>().RemoveItem(1);
                }
                wellAnimation.Play();
                break;
            case Toolstates.Scythe:
                hit.transform.gameObject.GetComponent<Ground>().WorkTheGround(Ground.Toolstages.Scythe);
                wellAnimation.Play();
                break;
        }
    }
}
