using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(InteractableObject))]
public class CustomerManager : MonoBehaviour
{
	public DrugType DemandedDrug;
	public float WaitingTime;

	private float _waitedTime;
	[HideInInspector]
	public DestinationPoint DestinationPoint;
	private NavMeshAgent _navAgent;
	private ParticleSystem _particleSystem;
	private VisualEffect _visualEffect;

	private SpawnController _spawnController;

	private void Awake()
	{
		_navAgent = GetComponentInChildren<NavMeshAgent>();
		_visualEffect = GetComponent<VisualEffect>();
		_particleSystem = GetComponent<ParticleSystem>();
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	private void Start()
	{
		_waitedTime = Time.time;
		_spawnController = Manager.Use<PlayManager>().Spawner;
		EnterRoom();
	}

	private void Update()
	{
		if (!_navAgent.hasPath)
		{
			Vector3 dir = Camera.main.transform.position - transform.position;
			dir.y = 0;
			transform.rotation = Quaternion.LookRotation(dir);
		}

		if (_waitedTime + WaitingTime <= Time.time)
		{
			CommitSuicide();
		}
	}

	private void InteractHandler(InteractableObject interactWith)
	{
		Drug drug = interactWith.GetComponent<Drug>();
		if (drug != null)
		{
			ReceiveDrug(drug);
			return;
		}

		Stamp stamp = interactWith.GetComponent<Stamp>();
		if (stamp != null)
		{
			CommitSuicide();
			stamp.has_ink = false;
			Debug.Log("Used stamp on customer");
		}
	}

	public void ReceiveDrug(Drug drug)
	{
		if (drug.drugType != DemandedDrug)
		{
			CommitSuicide();
		}
		else
		{
			LeaveRoom();
		}

		Destroy(drug.gameObject);
	}

	public void EnterRoom()
	{
		_navAgent.SetDestination(DestinationPoint.transform.position);
		DestinationPoint.IsOccupied = true;
	}

	public void LeaveRoom()
	{
		DestinationPoint.IsOccupied = false;
		StartCoroutine(PlayParticles());
		Destroy(gameObject);
	}

	IEnumerator PlayParticles()
	{
		_particleSystem.Play();
		yield return new WaitForSeconds(1.5f);
	}

	IEnumerator PlayVisualEffect()
	{
		_visualEffect.Play();
		yield return new WaitForSeconds(1.5f);
	}

	public void CommitSuicide()
	{
		// TODO: Go down the Suisilde
	}
}
