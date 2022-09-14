using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmg : MonoBehaviour
{
    [Header("Vida")]
    public float actualvida;
    private float maxVida = 30f;

    private BombDmg Bdmg;
    private DmgController dmgC;

    void Start()
    {
        Bdmg = GameObject.FindGameObjectWithTag("Bombita").GetComponent<BombDmg>();
        dmgC = GameObject.FindGameObjectWithTag("damageController").GetComponent<DmgController>();
        actualvida = maxVida;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.CompareTag("AtkBomb"))
        {

          actualvida -= 0.25f * dmgC.dmgMultiplier;

        }

    }
}
