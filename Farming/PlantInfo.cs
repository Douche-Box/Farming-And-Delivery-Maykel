using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInfo : MonoBehaviour
{
    public int[] growthTimePerState;
    public GameObject[] states;
    public float growTime;
    public int curState;


    /// <summary>
    /// Growing the plant and spawning a new plant when the growth time has been reached
    /// </summary>
    public void Grow()
    {
        growTime += Time.deltaTime;

        if (growTime > growthTimePerState[curState])
        {
            if (curState < states.Length)
            {
                Instantiate(states[curState], transform.position, Quaternion.identity);
                growTime = 0;
            }

            curState++;
        }
    }
}
