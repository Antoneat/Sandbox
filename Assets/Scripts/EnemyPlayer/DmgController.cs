using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgController : MonoBehaviour
{
    private Player plyr;
    private PlayerDash plyrDash;

    private BombDmg bombdmg;
    public GameObject[] Bombitas;
    public bool deadBomb;

    private BuscadorController dogC;
    private BuscadorDmg dogdmg;
    public bool deadBusc;

    public float dmgMultiplier;

    void Start()
    {
        plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        plyrDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>();


        bombdmg = GameObject.FindGameObjectWithTag("Bombita").GetComponent<BombDmg>();
        dogC = GameObject.FindGameObjectWithTag("Buscador").GetComponent<BuscadorController>();
        dogdmg = GameObject.FindGameObjectWithTag("Buscador").GetComponent<BuscadorDmg>();

        Bombitas = new GameObject[5];
    }


    void Update()
    {
        Bombitas = GameObject.FindGameObjectsWithTag("Bombita");
     
        BombDAÑO();

    }

    public void BombDAÑO()
    {
        foreach (GameObject Bombita in Bombitas)
        {
            dmgMultiplier += 1;
            if (dmgMultiplier >= 5f)
            {
                dmgMultiplier = 0;
            }
        }
    }

    public void BuscadorDAÑO()
    {
        if (dogC.ataco == true)
        {

            plyr.actualvida -= 1.75f;

        }
    }

  

  

}
