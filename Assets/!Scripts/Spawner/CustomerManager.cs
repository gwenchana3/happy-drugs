using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(InteractableObject))]
public class CustomerManager : MonoBehaviour
{
	public DrugType DemandedDrug;
	public float WaitingTime = 10;

	private float _spawnTime;
	[HideInInspector]
	public DestinationPoint DestinationPoint;
	private NavMeshAgent _navAgent;
	private ParticleSystem _particleSystem;
	private VisualEffect _visualEffect;
	private SpawnController _spawner;

	private bool _goingDownTheSuislide;

	private void Awake()
	{
		_navAgent = GetComponentInChildren<NavMeshAgent>();
		_visualEffect = GetComponentInChildren<VisualEffect>();
		_visualEffect.enabled = false;
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
		_spawner = Manager.Use<PlayManager>().Spawner;
		_spawnTime = Time.time;
		Debug.Log(_visualEffect.enabled);
	}

	private void Start()
	{
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

		if (_spawnTime + WaitingTime <= Time.time && !_goingDownTheSuislide)
		{
			CommitSuicide();
		}
	}

	private void InteractHandler(InteractableObject interactWith)
	{
		Drug drug = interactWith.GetComponent<Drug>();
		if (drug != null)
		{
			// Customer gets a drug
			ReceiveDrug(drug);
			return;
		}

		Stamp stamp = interactWith.GetComponent<Stamp>();
		if (stamp != null)
		{
			// Customer is sent away
			LeaveRoom();
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

		// Destroy drug
		Destroy(drug.gameObject);
	}

	public void EnterRoom()
	{
		_navAgent.SetDestination(DestinationPoint.transform.position);
		DestinationPoint.IsOccupied = true;
	}

	public void LeaveRoom()
	{
		Debug.Log("Suicide");
		DestinationPoint.IsOccupied = false;
		_particleSystem.Play();
		Invoke("DestroyWithDelay", 1.5f);
	}

	private void DestroyWithDelay()
	{
		DestinationPoint.IsOccupied = false;
		Destroy(gameObject);
	}

	public void CommitSuicide()
	{
		_goingDownTheSuislide = true;
		Debug.Log("Suicide");
		_visualEffect.enabled = true;
		_spawner.DecreaseHP(1);
		Invoke("DestroyWithDelay", 1.5f);
	}
}
