using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthManager : MonoBehaviour
{
    public ItemSeeds item;

    public int curState;

    float growing;

    public GameObject clonePlant;

    /// <summary>
    /// Grow the plant and change its state overtime
    /// </summary>
    public void Growing()
    {
        growing -= Time.deltaTime;

        if (growing < 0 && curState < item.Plant.Length)
        {

            Destroy(clonePlant);

            clonePlant = Instantiate(item.Plant[curState], transform.position, Quaternion.identity);


            growing = item.growTime;

            curState++;
        }
        if (curState == item.Plant.Length - 1)
        {
            GetComponent<Ground>().curGroundState = 6;
        }
        if (curState == item.Plant.Length)
        {
            GetComponent<Ground>().curGroundState = 7;
        }
    }
}
