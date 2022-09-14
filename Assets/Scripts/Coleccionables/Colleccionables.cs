using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleccionables : MonoBehaviour
{
    public PlayerCollects playerCollects;
    public LayerMask layer;
    public bool destroy;

    void Start()
    {
        playerCollects = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollects>();
    }

    void Update()
    {
        Collider[] a = Physics.OverlapSphere(transform.position,1.8f,layer);
        foreach (Collider plyr in a)
        {
            Debug.Log("Player esta cerca del colleccionable." + plyr.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Player agarro el colleccionable." + plyr.name);
                playerCollects.counterCollectables++;
                destroy = true;
            }
        }
        Destruccion();
    }

    private void Destruccion()
    {
        if (destroy) Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.8f);
    }
}
