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

	private bool _isTrustworthy;

	[SerializeField]
	private GameObject[] validObjects = new GameObject[0];
	[SerializeField]
	private GameObject[] invalidObjects = new GameObject[0];

	private GameObject _instantiatedObject = null;

	private bool _hasArrived = false;

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
		_isTrustworthy = Random.value > 0.8;
	}

	private void Start()
	{
		EnterRoom();
	}

	private void Update()
	{
		if (!_navAgent.hasPath && !_hasArrived)
		{
			_hasArrived = true;
			Vector3 dir = Camera.main.transform.position - transform.position;
			dir.y = 0;
			transform.rotation = Quaternion.LookRotation(dir);

			SpawnObjects();
		}

		if (_spawnTime + WaitingTime <= Time.time && !_goingDownTheSuislide)
		{
			CommitSuicide();
		}
	}

	protected virtual void SpawnObjects()
	{
		if (!_isTrustworthy)
		{
			_instantiatedObject = Instantiate(invalidObjects[Random.Range(0, invalidObjects.Length)], DestinationPoint.ObjectSpawnPoint.position, DestinationPoint.ObjectSpawnPoint.rotation);
		}
		else
		{
			_instantiatedObject = Instantiate(validObjects[Random.Range(0, validObjects.Length)], DestinationPoint.ObjectSpawnPoint.position, DestinationPoint.ObjectSpawnPoint.rotation);
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

	public void CommitSuicide()
	{
		_goingDownTheSuislide = true;
		Debug.Log("Suicide");
		_visualEffect.enabled = true;
		_spawner.DecreaseHP(1);

		Destroy(gameObject, 1.5f);
	}

	protected virtual void OnDestroy()
	{
		DestinationPoint.IsOccupied = false;
		if (_instantiatedObject != null)
		{
			Destroy(_instantiatedObject);
		}
	}
}
