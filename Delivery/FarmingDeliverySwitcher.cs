using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingDeliverySwitcher : MonoBehaviour
{
    public GameObject carCam;
    public GameObject playerCam;
    public GameObject miniMap;

    public Transform car;
    public Transform player;
    public Transform parkingSpot;

    public bool inRangeOfCar;
    public bool inRangeOfParkingSpot;
    public bool inCar;

    public float parkingSpotDetectRange;
    public float inRangeOfCarDetectRange;

    public float currentDistanceToCar;
    public float currentDistanceToParkingSpot;


    void Start()
    {
        inRangeOfCarDetectRange = 5;
        parkingSpotDetectRange = 10;
        GetOutOfCar();
        carCam.SetActive(false);
        miniMap.SetActive(false);
        playerCam.SetActive(true);
    }

    void Update()
    {
        currentDistanceToCar = Vector3.Distance(player.position, car.position);
        if (currentDistanceToCar < inRangeOfCarDetectRange)
        {
            inRangeOfCar = true;
        }
        else
        {
            inRangeOfCar = false;
        }

        currentDistanceToParkingSpot = Vector3.Distance(car.position, parkingSpot.position);
        if (currentDistanceToParkingSpot < parkingSpotDetectRange)
        {
            inRangeOfParkingSpot = true;
        }
        else
        {
            inRangeOfParkingSpot = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (inCar && inRangeOfParkingSpot)
            {
                GetOutOfCar();
            }
            else if (!inCar && inRangeOfCar)
            {
                GetIntoCar();
            }
        }
    }

    void GetIntoCar()
    {
        car.GetComponent<Car>().isControllingCar = true;
        carCam.SetActive(true);
        miniMap.SetActive(true);
        playerCam.SetActive(false);
        player.GetComponent<Movers>().isOutOfCar = false;
        inCar = true;
    }

    void GetOutOfCar()
    {
        car.GetComponent<Car>().isControllingCar = false;
        player.GetComponent<Movers>().isOutOfCar = true;
        carCam.SetActive(false);
        playerCam.SetActive(true);
        miniMap.SetActive(false);
        inCar = false;
    }
}
