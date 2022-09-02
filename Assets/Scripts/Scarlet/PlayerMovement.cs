using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float maxSpeed;

    public Rigidbody rgbd;


    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();

        if(rgbd.velocity.magnitude > maxSpeed)
        {
            rgbd.velocity = Vector3.ClampMagnitude(rgbd.velocity, maxSpeed);
        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rgbd.velocity = new Vector3(horizontal * speed * Time.fixedDeltaTime, rgbd.velocity.y, vertical * speed * Time.fixedDeltaTime);
    }
}
