using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class DebugCube : MonoBehaviour
{
	protected virtual void Awake()
	{
		GetComponent<InteractableObject>().OnInteracted += OnInteract;
	}

	private void OnInteract(InteractableObject interactWith)
	{
		Debug.Log("Hit");
	}
}
