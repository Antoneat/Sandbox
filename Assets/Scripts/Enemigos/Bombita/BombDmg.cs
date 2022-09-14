using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDmg : MonoBehaviour
{
    private Player plyr;

    [Header("Vida")]
    public float vida;
    public bool dead;

    void Start()
    {
        plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        vida = 5;
    }

    
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
      
        if (collider.gameObject.CompareTag("AtaqueUno")) vida -= plyr.AttackDmgUno; // Baja la vida del enemigo acorde con el valor que se puso en el ataque.

        if (collider.gameObject.CompareTag("AtaqueDos")) vida -= plyr.AttackDmgDos; // Lo de arriba x2.

        if (collider.gameObject.CompareTag("AtaqueTres")) vida -= plyr.AttackDmgTres; // Lo de arriba x3.

        if (collider.gameObject.CompareTag("AtaqueCargado")) vida -= plyr.AttackDmgCargado; // Lo de arriba x4.
    }
}