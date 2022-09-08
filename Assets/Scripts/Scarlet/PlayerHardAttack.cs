using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardAttack : MonoBehaviour
{
    // public Animator anim;
    public BoxCollider armaCollider;

    [SerializeField] private int combo;

    //public AudioSource audio;
    // public AudioClip[] sonido;

    void Start()
    {
        // anim = GetComponent<Animator>();
        // audio = GetComponent<AudioSouce>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // anim.SetTrigger("PrimerAtaqueFuerte" + combo);
            // audio.clip = sonido[combo];
            // audio.Play();
            armaCollider.enabled = true;
        }
    }

    public void HardAttacking()
    {
        Debug.Log("Atacando");
        armaCollider.enabled = true;
        if (combo < 3) combo++;
    }

    public void AfterHardAttacking()
    {
        Debug.Log("Termino de atacar");
        armaCollider.enabled = false;
        combo = 0;
    }
}
