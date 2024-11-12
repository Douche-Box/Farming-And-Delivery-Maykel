using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeliverySpot : MonoBehaviour
{
    public Transform[] points;
    public GameObject deliverPoint;
    public GameObject pointer;

    public GameObject parkSpot;

    public GameObject clone;
    public Transform parkSpotTransform;

    public bool parked;

    public GameObject requestManagerGo;
    public Transform player;
    public int indexNumber;

    public float currentDistanceToParkSpace;
    public float inRangeOfParkSpace;

    public bool acceptedRequest;

    public Transform respawnpoint;

    public void Start()
    {
        inRangeOfParkSpace = 1;
        parked = false;
    }

    public void Update()
    {
        if (acceptedRequest == true)
        {
            currentDistanceToParkSpace = Vector3.Distance(player.position, parkSpotTransform.position);
            if (currentDistanceToParkSpace < inRangeOfParkSpace)
            {
                parked = true;
            }
            else
            {
                parked = false;
            }

            if (parked == true && Input.GetButtonDown("Jump"))
            {
                Destroy(clone);

                parked = false;
                acceptedRequest = false;
                requestManagerGo.GetComponent<RequestManager>().GiveMoney(indexNumber);

                pointer.GetComponent<Pointer>().target = respawnpoint;
            }
        }
    }

    public void PickPoints()
    {
        int indexNumber = Random.Range(0, points.Length);
        Debug.Log(points[indexNumber].name);

        clone = Instantiate(deliverPoint, points[indexNumber].position, deliverPoint.transform.rotation);

        parkSpotTransform = clone.transform;

        pointer.GetComponent<Pointer>().target = points[indexNumber].transform;

        acceptedRequest = true;
    }
}
