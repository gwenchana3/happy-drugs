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
	private ParticleSystem _goodParticleSystem;
	private VisualEffect _visualEffect;
	public ParticleSystem[] BadParticles;
	private SpawnController _spawner;

	private bool _goingDownTheSuislide;

	private bool _isTrustworthy;

	[SerializeField]
	private GameObject[] _validObjects = new GameObject[0];
	[SerializeField]
	private GameObject[] _invalidObjects = new GameObject[0];

	[SerializeField]
	private GameObject _speechBubble = null;

	private GameObject _instantiatedObject = null;

	private bool _hasArrived = false;

	private void Awake()
	{
		_navAgent = GetComponentInChildren<NavMeshAgent>();
		_visualEffect = GetComponentInChildren<VisualEffect>();
		_visualEffect.enabled = false;
		_goodParticleSystem = GetComponentInChildren<ParticleSystem>();
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
		_spawner = Manager.Use<PlayManager>().Spawner;
		_spawnTime = Time.time;
		Debug.Log(_visualEffect.enabled);
		_isTrustworthy = Random.value < 0.8;
		_speechBubble.SetActive(false);
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
			_speechBubble.SetActive(true);

			SpawnObjects();
		}

		if (_spawnTime + WaitingTime <= Time.time && !_goingDownTheSuislide)
		{
			CommitSuicide();
		}
	}

	protected virtual void SpawnObjects()
	{
		if (_isTrustworthy)
		{
			_instantiatedObject = Instantiate(_validObjects[Random.Range(0, _validObjects.Length)], DestinationPoint.ObjectSpawnPoint.position, DestinationPoint.ObjectSpawnPoint.rotation);
		}
		else
		{
			_instantiatedObject = Instantiate(_invalidObjects[Random.Range(0, _invalidObjects.Length)], DestinationPoint.ObjectSpawnPoint.position, DestinationPoint.ObjectSpawnPoint.rotation);
		}
	}

	private void InteractHandler(InteractableObject interactWith)
	{
		Drug drug = interactWith.GetComponent<Drug>();
		if (drug != null)
		{
			// Customer gets a drug
			ReceiveDrug(drug);

			// Destroy drug
			Destroy(drug.gameObject);
			return;
		}

		Stamp stamp = interactWith.GetComponent<Stamp>();
		if (stamp != null)
		{
			stamp.has_ink = false;
			if (!_isTrustworthy)
			{
				LeaveRoom();
			}
			else
			{
				CommitSuicide();
			}
		}
	}

	public void ReceiveDrug(Drug drug)
	{
		if (!_isTrustworthy || drug.drugType != DemandedDrug)
		{
			CommitSuicide();
		}
		else
		{
			LeaveRoom();
		}
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
		_goodParticleSystem.Play();
		Destroy(gameObject, 1.5f);
	}

	public void CommitSuicide()
	{
		_goingDownTheSuislide = true;
		Debug.Log("Suicide");
		_visualEffect.enabled = true;
		_spawner.DecreaseHP(1);

        for (int i = 0; i < BadParticles.Length; i++)
        {
			BadParticles[i].Play();
        }

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
