using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
	private Player plyr;
	private PlayerDash plyrDash;
	public UnityEngine.AI.NavMeshAgent agent;

	public int destPoint = 0;
	public Transform goal;

	public float playerDistance;
	public float awareAI;
	public float atkRange;

	[Header("Vida")]
	public float vida;
	public bool dead;

	[Header("AtaqueBasico")]
	public float ataqueNormalDMG;
	public GameObject basicoGO;
	//public GameObject atkBTxt;

	public bool coPlay;

	void Start()
	{
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		//agent.autoBraking = false;

		goal = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

		agent.SetDestination(goal.position);

		plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		plyrDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>();

		dead = false;
		basicoGO.SetActive(false);

		//atkBTxt.SetActive(false);

		coPlay = false;
	}

	void Update()
	{
		Muerte();

		playerDistance = Vector3.Distance(transform.position, goal.position);

		if (playerDistance <= awareAI)
		{
			LookAtPlayer();
			Debug.Log("Seen");
			Chase();
			agent.isStopped = false;
		}
		else if (playerDistance > awareAI)
		{
			LookAtPlayer();
			agent.isStopped = true;
		}


		if (playerDistance <= atkRange && coPlay==false)
		{
			StartCoroutine(AtaqueBasico());
			agent.isStopped = false;
		}
		else if (playerDistance > atkRange)
		{
			LookAtPlayer();
			agent.isStopped = false;
		}
	}

	void LookAtPlayer()
	{
		transform.LookAt(goal);
	}

	public void Chase()
	{
		agent.stoppingDistance = 5;
		agent.SetDestination(goal.position);

		//transform.Translate(Vector3.forward * Speed * Time.deltaTime);

		if (agent.remainingDistance > agent.stoppingDistance)
		{
			agent.isStopped = false;

		}
		else if (agent.remainingDistance < agent.stoppingDistance)
		{
			agent.isStopped = true;
		}
	}

	public void Muerte()
	{
		if (vida <= 0)
		{
			plyrDash.killedEnemy = true;
			Destroy(gameObject);
		}
	}


	IEnumerator AtaqueBasico()
	{
		coPlay = true;
		agent.isStopped = true;
		yield return new WaitForSecondsRealtime(0.75f);
		agent.isStopped = false;
		basicoGO.SetActive(true);
		//atkBTxt.SetActive(true);
		yield return new WaitForSecondsRealtime(1.5f);
		basicoGO.SetActive(false);
		//atkBTxt.SetActive(false);
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

		if (collider.gameObject.CompareTag("AtaqueUno")) vida -=3; // Baja la vida del enemigo acorde con el valor que se puso en el ataque.
	}

}
