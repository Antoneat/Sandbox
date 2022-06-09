using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed;
    public float maxSpeed;
    public Vector3 movement;
    public Transform playerTransform;
    public Rigidbody rb;
    public SpriteRenderer spriteRenderer; //Giro del sprite

    [Header("Vida")]
    public float actualvida;
    private float maxVida = 20;

    [Header("Desplazamiento")]
    public bool dash;
    public float speedDash;
    private float dashCooldown = 0.2f;
    [SerializeField] private float dashLength = 0.1f;
    public float dashCounter;
    public float dashCoolCounter;

    [Header("AtaqueCombo")]
    public int numberOfClicks = 0;
    private float lastClickedTime = 0;
    private float maxComboDelay = 0.8f;
    public int AttackDmgUno = 10;
    public int AttackDmgDos = 20;
    public int AttackDmgTres = 30;
    public GameObject ataqueUnoGO;
    public GameObject ataqueDosGO;
    public GameObject ataqueTresGO;
    public bool attackCombo = false;
    public float attackCooldown = 0.25f;
    private float timePressed = 0.9f;

    [Header("AtaqueCargado")]
    //[SerializeField] private float radio = 5f;
    public GameObject ataqueCargGO;
    public int AttackDmgCargado = 5;
    public bool attackCharged = false;

    [Header("Bloqueo")]
    public float bloqueoDuracion;
    public float bloqueoMaxDuracion;
    public bool blck;
    public int cargasDeExplosion;
    public float bloqueoCooldown;
    public float bloqueoCounter;
    public LayerMask enemyLayer;

    [Header("Almas")]
    public float almas;
    public TMP_Text almasText;

    [Header("Extra")]
    [SerializeField] private Enemy enmy;
    [SerializeField] private Enemy2 enmy2;
    [SerializeField] private Yaldabaoth yp;
    [SerializeField] private int sceneId = 1;
    public TMP_Text vidapersonajeTxt;
    public TMP_Text dmgTxt;
    int a = 0;
    int b = 0;
    public StateManager SM;
    public bool closeToStair;

    [Header("VFX")]
    public GameObject ataqueUno;
    public GameObject ataqueDos;
    public GameObject ataqueTres;

    [Header("Coleccionables")]
    public int collectables = 1;
    public int counterCollectables = 0;
    public TMP_Text collecTxt;
    public GameObject collecTxtGO;
    public float counterNum;

    [Header("Enemigos")]
    [SerializeField] private GameObject BuscadorPrefab;
    [SerializeField] private GameObject VerdugoPrefab;
    //[SerializeField] private GameObject YaldaPrefab;
    //[SerializeField] private GameObject SamaelPrefab;

  
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        actualvida = maxVida;
        almas = 0;
        blck = false;
        bloqueoDuracion = bloqueoMaxDuracion;

        closeToStair = false;

        collecTxtGO.SetActive(false);

        Console.instance.RegisterCommand("godmode", godmode, "Activar/Desactivar el modo Dios.");
        Console.instance.RegisterCommand("restartlevel", resetlevel, "Reiniciar nivel");
        Console.instance.RegisterCommand("previouslevel", previouslevel, "Nivel anterior");
        Console.instance.RegisterCommand("nextlevel", nextlevel, "Siguiente nivel");
        Console.instance.RegisterCommand("crt", crt, "Creditos");
        Console.instance.RegisterCommand("infinitedmg", infinitedmg, "Da�o infinito");
    }

    void Update()
    {
        invocacionesEnemigos();
        Blocking();

        vidapersonajeTxt.text = "Vida: " + actualvida.ToString();

        dmgTxt.text = "Da�o: " + AttackDmgUno.ToString();

        almasText.text = "Almas: " + almas.ToString();

        collecTxt.text = counterCollectables.ToString() + " / 5";

        counterNum += Time.deltaTime;

        if (collectables == counterCollectables)
        {
            Collects();
            counterNum = 0;
        }
        if (counterNum > 3)
        {
            collecTxtGO.SetActive(false);
            counterNum = 0;
        }

        //movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <= 0f && dashCounter <= 0)
            {
                dash = true;
                dashCounter = dashLength;
                StartCoroutine(Dash());
            }
        }

        if(bloqueoDuracion > 0 && Input.GetKey(KeyCode.K))
        {
            bloqueoDuracion -= Time.deltaTime;
        }

        if (bloqueoCounter <= bloqueoCooldown && blck == false)
        {
            bloqueoCounter -= Time.deltaTime;
        }



        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
        
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                dash = false;
                dashCoolCounter = dashCooldown;
            }
        }

        attackCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.J))
        {
            timePressed -= Time.deltaTime;
        }
        if (timePressed >= 0 && Input.GetKeyUp(KeyCode.J))
        {
            attackCombo = true;
            attackCharged = false;
        }
        if (timePressed < 0 && Input.GetKeyUp(KeyCode.J))
        {
            attackCombo = false;
            attackCharged = true;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            numberOfClicks = 0;
            ataqueUnoGO.SetActive(false);
            ataqueDosGO.SetActive(false);
            ataqueTresGO.SetActive(false);
            ataqueUno.SetActive(false);
            ataqueDos.SetActive(false);
            ataqueTres.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Movimiento();
        
        if (attackCombo)
        {
            AttackCombo();
            timePressed = 1f;
        }

        if (attackCharged)
        {
            StartCoroutine(AttackingCharg());
            timePressed = 1f;
        }
        
        if (closeToStair)
        {
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 6f, ForceMode.Impulse);
            }
            if(rb.velocity.y >= 0)
            {
                speed = 800;
            }
        }
    }

    public void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (dash == false) 
        {
            rb.velocity = new Vector3(horizontal * speed * Time.fixedDeltaTime, 0, vertical * speed * Time.fixedDeltaTime);
            playerTransform.rotation = Quaternion.LookRotation(movement);
            //movement = new Vector3(0, 0, 0);
        }

        if (horizontal > 0) //Direcci�n donde se mueve
        {
            movement.z = 0;
            movement.x = 1;
        }
        else if (horizontal < 0)
        {
            movement.z = 0;
            movement.x = -1;
        }

        if(vertical > 0)
        {
            movement.x = 0;
            movement.z = 1;
        }
        else if (vertical < 0)
        {
            movement.x = 0;
            movement.z = -1;
        }

        if (movement.x < 0) //Giro del sprite cuando mueve DERECHA o IZQUIERDA 
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        /*
        if(horizontal > 0)
        {
            ataqueUnoGO.transform.position = transform.position + new Vector3(2, 0, 0);
            ataqueDosGO.transform.position = transform.position + new Vector3(2, 0, 0);
            ataqueTresGO.transform.position = transform.position + new Vector3(2, 0, 0);
            ataqueCargGO.transform.position = transform.position + new Vector3(2, 0, 0);
        }
        else if(horizontal < 0)
        {
            ataqueUnoGO.transform.position = transform.position + new Vector3(-2, 0, 0);
            ataqueDosGO.transform.position = transform.position + new Vector3(-2, 0, 0);
            ataqueTresGO.transform.position = transform.position + new Vector3(-2, 0, 0);
            ataqueCargGO.transform.position = transform.position + new Vector3(-2, 0, 0);
        }
        else if(vertical > 0)
        {
            ataqueUnoGO.transform.position = transform.position + new Vector3(0, 0, 2);
            ataqueDosGO.transform.position = transform.position + new Vector3(0, 0, 2);
            ataqueTresGO.transform.position = transform.position + new Vector3(0, 0, 2);
            ataqueCargGO.transform.position = transform.position + new Vector3(0, 0, 2);
        }
        else if (vertical < 0)
        {
            ataqueUnoGO.transform.position = transform.position + new Vector3(0, 0, -2);
            ataqueDosGO.transform.position = transform.position + new Vector3(0, 0, -2);
            ataqueTresGO.transform.position = transform.position + new Vector3(0, 0, -2);
            ataqueCargGO.transform.position = transform.position + new Vector3(0, 0, -2);
        }*/
    }
    
    IEnumerator Dash()
    {
        if(movement.z > 0)
        {
            rb.AddForce(Vector3.forward * speedDash, ForceMode.Impulse);
        }
        else if (movement.z < 0)
        {
            rb.AddForce(Vector3.back * speedDash, ForceMode.Impulse);
        }
        else if (movement.x > 0)
        {
            rb.AddForce(Vector3.right * speedDash, ForceMode.Impulse);
        }
        else if (movement.x < 0)
        {
            rb.AddForce(Vector3.left * speedDash, ForceMode.Impulse);
        }

        if (movement.z > 0 && movement.x > 0)
        {
            rb.AddForce(new Vector3(1, 0, 1) * speedDash, ForceMode.Impulse);
        }
        else if (movement.z > 0 && movement.x < 0)
        {
            rb.AddForce(new Vector3(-1, 0, 1) * speedDash, ForceMode.Impulse);
        }
        else if (movement.z < 0 && movement.x > 0)
        {
            rb.AddForce(new Vector3(1, 0, -1) * speedDash, ForceMode.Impulse);
        }
        else if (movement.z < 0 && movement.x < 0)
        {
            rb.AddForce(new Vector3(-1, 0, -1) * speedDash, ForceMode.Impulse);
        }

        //movement = new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.3f);
    }
    
    private void AttackCombo()
    {
        lastClickedTime = Time.time;
        numberOfClicks++;

        if (numberOfClicks == 1 && attackCooldown <=0)
        {
            ataqueUnoGO.SetActive(true);
            ataqueDosGO.SetActive(false);
            ataqueTresGO.SetActive(false);
            ataqueUno.SetActive(true);
            ataqueDos.SetActive(false);
            ataqueTres.SetActive(false);
            attackCooldown = 0.25f;
            StartCoroutine(Slowness());
        }

        numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);

        if (numberOfClicks == 2 && attackCooldown <= 0)
        {
            ataqueUnoGO.SetActive(false);
            ataqueDosGO.SetActive(true);
            ataqueTresGO.SetActive(false);
            ataqueUno.SetActive(false);
            ataqueDos.SetActive(true);
            ataqueTres.SetActive(false);
            attackCooldown = 0.25f;
            StartCoroutine(Slowness());
        }

        if (numberOfClicks == 3 && attackCooldown <= 0)
        {
            ataqueUnoGO.SetActive(false);
            ataqueDosGO.SetActive(false);
            ataqueTresGO.SetActive(true);
            ataqueUno.SetActive(false);
            ataqueDos.SetActive(false);
            ataqueTres.SetActive(true);
            attackCooldown = 0.25f;
            StartCoroutine(Slowness());
            StartCoroutine(RestartCombo());
        }
        attackCombo = false;
    }

    IEnumerator Slowness()
    {
        speed = 70;
        yield return new WaitForSeconds(0.4f);
        speed = 400;
        yield break;
    }

    IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(1f);
        numberOfClicks = 0;
        yield break;
    }

    IEnumerator AttackingCharg()
    {
        for (int i = 0; i < 5; i++)
        {
            ataqueCargGO.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            ataqueCargGO.SetActive(false);
        }
        attackCharged = false;
    }

    private void Blocking()
    {
        if (Input.GetKey(KeyCode.K) && bloqueoCounter <= 0)
        {
            blck = true;
            speed = 0;
            Debug.Log("Bloqueando1");
            //animacion de bloqueo
        }
        if (bloqueoDuracion <= 0 || Input.GetKeyUp(KeyCode.K) || cargasDeExplosion == 5)
        {
            blck = false;
            Debug.Log("Suelte de tecla2");
            // animacion de explosion
            StartCoroutine(DevolverDmg());
            bloqueoCounter = bloqueoCooldown;
            speed = 400;
        }
    }

    IEnumerator DevolverDmg()
    {
        Collider[] EnemiesInRange = Physics.OverlapSphere(transform.position, 4f, enemyLayer);
        foreach (Collider enemyInRange in EnemiesInRange)
        {
            enemyInRange.GetComponent<Enemy>().vida -= cargasDeExplosion * 0.15f;
            enemyInRange.GetComponent<Enemy2>().vida -= cargasDeExplosion * 0.15f;
            Debug.Log("Devolviendo dmg a enemigos3");
        }
        bloqueoDuracion = bloqueoMaxDuracion;
        cargasDeExplosion = 0;
        yield break;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3f);
        Gizmos.color = Color.red;
    }

    private void Collects()
    {
        collecTxtGO.SetActive(true);
        collectables++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FueraDelMundo")) transform.position = new Vector3(0,0.7f,0);

        if (collision.gameObject.CompareTag("manos"))
        {

            actualvida -= 4;

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // if (collider.gameObject.CompareTag("RangoAtaqueEnemy1")) enmy.playerOnRange = true;

        if (collider.gameObject.CompareTag("AtaqueNormalEnemy1"))
        {
            if (blck == true)
            {
                RecieveDmgWhenBlock(enmy.ataqueNormalDMG);
                cargasDeExplosion++;
                Debug.Log("Recibiendo dmg reducido");
            }
            else
            {
                actualvida -= enmy.ataqueNormalDMG;
            }
        }

        if (collider.gameObject.CompareTag("MordiscoEnemy1")) 
        {
            if (blck == true)
            {
                RecieveDmgWhenBlock(enmy.mordiscoDMG);
                cargasDeExplosion++;
                SM.ps = PlayerState.Sangrado;
                Debug.Log("Recibiendo dmg reducido");
            }
            else
            {
                SM.ps = PlayerState.Sangrado;
                actualvida -= enmy.mordiscoDMG;
            }
        } 

        if (collider.gameObject.CompareTag("AtkBasicoE2"))
        {
            if (blck == true)
            {
                RecieveDmgWhenBlock(enmy2.atkbasDMG);
                cargasDeExplosion++;
                Debug.Log("Recibiendo dmg reducido");
            }
            else
            {
                actualvida -= enmy2.atkbasDMG;
            }
        }

        if (collider.gameObject.CompareTag("Golpe2"))
        {
            if (blck == true)
            {
                RecieveDmgWhenBlock(enmy2.golpeDMG);
                cargasDeExplosion++;
                SM.ps = PlayerState.Quemado;
                Debug.Log("Recibiendo dmg reducido");
            }
            else
            {
                SM.ps = PlayerState.Quemado;
                actualvida -= enmy2.golpeDMG;
            }
        }

        if (collider.gameObject.CompareTag("Rafaga2"))
        {
            if (blck == true)
            {
                RecieveDmgWhenBlock(enmy2.rafagaDMG);
                cargasDeExplosion++;
                SM.ps = PlayerState.Quemado;
                Debug.Log("Recibiendo dmg reducido");
            }
            else
            {
                SM.ps = PlayerState.Quemado;
                actualvida -= enmy2.rafagaDMG;
            }
        }

        if (collider.gameObject.CompareTag("onda"))
        {
            actualvida -= 6;
        }

        if (collider.gameObject.CompareTag("basico1"))
        {
                actualvida -= yp.basico1DMG;
        }

        if (collider.gameObject.CompareTag("basico3"))
        {
            actualvida -= yp.basico3DMG;
        }

        if (collider.gameObject.CompareTag("especial"))
        {
            actualvida -= yp.especialDMG;
        }

        if (collider.gameObject.CompareTag("Escalera"))
        {
            closeToStair = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Escalera"))
        {
            closeToStair = false;
            speed = 400;
        }
    }

    /* private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("RangoAtaqueEnemy1")) enmy.playerOnRange = false;
    }
    */

    public void godmode()
    {
        a++;
        if (a % 2 == 0)
        {
            actualvida = maxVida;
        }
        else if (a % 2 == 1)
        {
            actualvida = 999999;
        }
    }

    public void infinitedmg()
    {
        b++;
        if (b % 2 == 0)
        {
            AttackDmgUno = 10;
            AttackDmgDos = 20;
            AttackDmgTres = 30;
            AttackDmgCargado = 5;
        }
        else if (b % 2 == 1)
        {
            AttackDmgUno = 999;
            AttackDmgDos = 999;
            AttackDmgTres = 999;
            AttackDmgCargado = 999;
        }
    }

    public void resetlevel()
    {
        SceneManager.LoadScene(sceneId);
        // Cuando pase de nivel agregarle en el trigger un +1 al sceneId para el reset de lvl en la consola
    }
    public void nextlevel()
    {
        SceneManager.LoadScene(sceneId + 1);
    }
    public void previouslevel()
    {
        SceneManager.LoadScene(sceneId - 1);
    }
    public void crt()
    {
        SceneManager.LoadScene(2);
    }

    public void invocacionesEnemigos()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(BuscadorPrefab, transform.position + new Vector3(2, 0, 0), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Instantiate(VerdugoPrefab, transform.position + new Vector3(2,0,0), Quaternion.identity);
        }
    }

    public void RecieveDmgWhenBlock(float dmg)
    {
        actualvida -= dmg * 0.25f;
        actualvida = Mathf.Max(0, actualvida);
    }
}