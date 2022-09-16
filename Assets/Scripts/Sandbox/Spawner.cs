using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject bombita;
    public GameObject buscador;

    public Vector3 tpBicho;
    public Vector3 tpBuscador;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.CompareTag("DetectorBicho"))
        {

            Instantiate(bombita, tpBicho, Quaternion.identity);

        }

        if (collider.gameObject.CompareTag("DetectorBuscador"))
        {

            Instantiate(buscador, tpBicho, Quaternion.identity);

        }


        if (collider.gameObject.CompareTag("DetectorBichoOUT"))
        {

            Destroy(bombita);

        }

        if (collider.gameObject.CompareTag("DetectorBuscadorOUT"))
        {

            Destroy(buscador);

        }
    }
}
