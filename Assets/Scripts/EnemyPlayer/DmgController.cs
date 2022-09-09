using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgController : MonoBehaviour
{
    private Player plyr;
    private PlayerDash plyrDash;

    private BombController Bcontrol;
    public GameObject[] Bombitas;

    private BuscadorController buscadorC;
    public float dmgMultiplier;
    void Start()
    {
       plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        plyrDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>();

        Bombitas = new GameObject[5];
    }


    void Update()
    {
        Bombitas = GameObject.FindGameObjectsWithTag("Bombita");
        MuerteBomba();
        BombDAÑO();
        BuscadorDAÑO();
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
        if (buscadorC.ataco == true)
        {

            plyr.actualvida -= 1.75f;

        }
    }

    public void MuerteBomba()
    {
        if (Bcontrol.vida <= 0)
        {
            plyrDash.killedEnemy = true;
            Destroy(gameObject);
        }
    }

    public void MuerteBuscador()
    {
        if (buscadorC.vida <= 0)
        {
            plyrDash.killedEnemy = true;
            Destroy(gameObject);
        }
    }

}
