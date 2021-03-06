using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy2 : MonoBehaviour
{

    public Player plyr;
    public StateManager SM;
    public enemyPatrol2 eP2;

    public float proyectileSpeed = 4;

    [Header("Vida")]
    public float vida;
    public bool dead;

    [Header("AtaqueBasico")]
    public float atkbasDMG;
    public GameObject atkbasGO;
    public GameObject atkBTxt;

    [Header("GolpeAlPiso")]
    public float golpeDMG;
    public GameObject golpeGO;
    public GameObject golpeTxt;

    [Header("Rafaga")]
    public float rafagaDMG;
    public GameObject rafagaGO;
    public GameObject rafagaTxt;

    [Header("Extra")]
    [SerializeField] private float knockbackStrength;
    public Vector3 playerpos;

    public bool coPlay;

    void Start()
    {
        plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       
        dead = false;

        atkbasGO.SetActive(false);
        golpeGO.SetActive(false);
        rafagaGO.SetActive(false);

        atkBTxt.SetActive(false);
        golpeTxt.SetActive(false);
        rafagaTxt.SetActive(false);

        coPlay = false;
    }

    void Update()
    {
       
        Muerte();
        playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Muerte()
    {
        if (vida <= 0)
        {
            plyr.killedEnemy = true;
            plyr.dashIMG.SetActive(true);
            plyr.actualvida += 10;
            plyr.almas += 10;
            plyr.enemigosDerrotados++;
            dead = true;
            Destroy(gameObject);
        }
    }

    public void ChooseAtk2()
    {
        if (coPlay == false && SM.ps == PlayerState.Normal && eP2.playerDistance < eP2.atkRange || coPlay == false &&  SM.ps == PlayerState.Sangrado && eP2.playerDistance < eP2.atkRange || coPlay == false &&  SM.ps == PlayerState.Quemado && eP2.playerDistance < eP2.atkRange)
        {
            StartCoroutine(AtaqueBasico());
        }
        else if (coPlay == false && SM.ps == PlayerState.Stun && eP2.playerDistance < eP2.atkRange)
        {
            StartCoroutine(GolpeAlPiso());
        }
        else if (coPlay == false && eP2.playerDistance > eP2.atkRange && eP2.playerDistance < eP2.awareAI)
        {
            StartCoroutine(Rafaga());
        }

    }

    IEnumerator AtaqueBasico()
    {
        coPlay = true;
        eP2.agent.isStopped = true;
        yield return new WaitForSecondsRealtime(1.5f);
        eP2.agent.isStopped = false;
        atkbasGO.SetActive(true);
        atkBTxt.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        atkbasGO.SetActive(false);
        atkBTxt.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        coPlay = false;
        yield break;
    }

    IEnumerator GolpeAlPiso()
    {
        coPlay = true;
        eP2.agent.isStopped = true;
        //wea q lo sigue 
        yield return new WaitForSecondsRealtime(3f);
        //GameObject clone = Instantiate(golpeGO, playerpos, Quaternion.identity);
        golpeGO.transform.position = playerpos;
        golpeGO.SetActive(true);
        golpeTxt.SetActive(true);
        // SM.ps = PlayerState.Quemado;
        eP2.agent.isStopped = false;
        yield return new WaitForSecondsRealtime(1f);
        golpeGO.SetActive(false);
        golpeTxt.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        coPlay = false;
        yield break;
    }

    IEnumerator Rafaga()
    {
        coPlay = true;
        eP2.agent.isStopped = true;
        yield return new WaitForSecondsRealtime(1f);
        rafagaGO.SetActive(true);
        rafagaTxt.SetActive(true);
        float step = proyectileSpeed * Time.deltaTime; // calculate distance to move
        rafagaGO.transform.position = Vector3.MoveTowards(transform.position, playerpos, step);
        //SM.ps = PlayerState.Quemado;
        eP2.agent.isStopped = false;
        yield return new WaitForSecondsRealtime(2f);
        rafagaGO.SetActive(false);
        rafagaTxt.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        coPlay = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FueraDelMundo")) Destroy(gameObject); // Si toca los colliders de FueraDelMundo, se destruye.
    }

    private void OnTriggerEnter(Collider collider)
    {
        Rigidbody rb = collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = collider.transform.position - transform.position;
            direction.y = 0;

            rb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }

        if (collider.gameObject.CompareTag("AtaqueUno")) vida -= plyr.AttackDmgUno; // Baja la vida del enemigo acorde con el valor que se puso en el ataque.

        if (collider.gameObject.CompareTag("AtaqueDos")) vida -= plyr.AttackDmgDos; // Lo de arriba x2.

        if (collider.gameObject.CompareTag("AtaqueTres")) vida -= plyr.AttackDmgTres; // Lo de arriba x3.

        if (collider.gameObject.CompareTag("AtaqueCargado")) vida -= plyr.AttackDmgCargado; // Lo de arriba x4.
    }
}
