using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCombo : MonoBehaviour
{
    // public Animator anim;
    public BoxCollider armaCollider;

    void Start()
    {
        // anim = GetCompenent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // anim.SetTrigger("PrimerAtaque");
            armaCollider.enabled = true;
        }
    }

    public void Attacking()
    {
        Debug.Log("Atacando");
        armaCollider.enabled = true;
    }

    public void AfterAttacking()
    {
        Debug.Log("Termino de atacar");
        armaCollider.enabled = false;
    }
}
