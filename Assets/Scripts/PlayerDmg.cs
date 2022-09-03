using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmg : MonoBehaviour
{
    [Header("Vida")]
    public float actualvida;
    private float maxVida = 30;

    public float dmgMultiplier;

    public GameObject[] Bombitas;
    void Start()
    {
        actualvida = maxVida;
    }

    // Update is called once per frame
    void Update()
    {
        if (Bombitas == null)
            Bombitas = GameObject.FindGameObjectsWithTag("Bombita");

            BombDAÑO();
    }


    void BombDAÑO()
    {
        foreach (GameObject Bombita in Bombitas)
        {
            dmgMultiplier+=1;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // if (collider.gameObject.CompareTag("RangoAtaqueEnemy1")) enmy.playerOnRange = true;


        if (collider.gameObject.CompareTag("AtkBomb"))
        {

            actualvida -= 0.75f; // * dmgMultiplier;
         
        }
    }
}
