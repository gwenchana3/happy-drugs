using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class StampPad : MonoBehaviour
{
	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	protected virtual void InteractHandler(InteractableObject interactWith)
	{
		Stamp stamp = interactWith.GetComponent<Stamp>();
		if (stamp != null)
		{
			stamp.has_ink = true;
			Debug.Log("Stamp inked");
		}
	}
}
