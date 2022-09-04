using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientation;
    private Rigidbody rgbd;
    private PlayerMovement playerMovement;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;

    [Header("Reset")]
    public bool killedEnemy;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    public void Dash()
    {
        if (killedEnemy == false) return;

        Vector3 forcetoApply = rgbd.velocity.normalized * dashForce;

        rgbd.AddForce(forcetoApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash),0.01f);

        // Invoke(nameof(ResetDash), dashDuration);
    }

    public void ResetDash()
    {
        killedEnemy = false;
    }
}
