using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class DrugBox : MonoBehaviour
{
	/// <summary>
	/// The type of this drug
	/// </summary>
	[field: SerializeField]
	public DrugType drugType { get; private set; } = DrugType.LSD;

	[SerializeField]
	[Tooltip("The drug game object to instantiate")]
	private GameObject _drug = null;


	[SerializeField]
	[Tooltip("The spawn position offset for new drugs")]
	private Vector3 spawnOffset = default;

	private InteractableObject _currentDrug = null;

	void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	protected virtual void FixedUpdate()
	{
		if (_currentDrug == null)
		{
			GameObject newDrug = Instantiate(_drug, transform.position + spawnOffset, Quaternion.identity);
			InteractableObject interactable = newDrug.GetComponent<InteractableObject>();
			interactable.OnDragged += ResetDrug;
			interactable.isDragged = true;
			interactable.wantedPos = transform.position + spawnOffset;
			_currentDrug = interactable;
		}
		else
		{
			_currentDrug.transform.position = transform.position + spawnOffset;
		}
	}

	private void ResetDrug()
	{
		_currentDrug.OnDragged -= ResetDrug;
		_currentDrug = null;
	}

	protected virtual void InteractHandler(InteractableObject interactWith)
	{
		Drug drug = interactWith.GetComponent<Drug>();
		if (drug != null && drug.drugType == drugType)
		{
			Destroy(drug.gameObject);
		}
	}
}
