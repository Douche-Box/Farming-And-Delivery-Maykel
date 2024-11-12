using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 move;
    public float horizontal;
    public float vertical;
    public float speed;

    Rigidbody rb;

    public RaycastHit hit;
    public GameObject player;

    public GameObject fpsCam;
    public bool isOutOfCar;

    void Start()
    {
        speed = 1.5f;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isOutOfCar)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            move.x = horizontal;
            move.z = vertical;

            transform.Translate(move * speed * Time.deltaTime * 5);
        }
    }
}
