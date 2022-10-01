using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class CardReader : MonoBehaviour
{
	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	protected virtual void InteractHandler(InteractableObject interactWith)
	{
		Card card = interactWith.GetComponent<Card>();
		if (card != null && card.valid)
		{
			BeepPositive();
			Debug.Log(card.balance);
		}
		else
		{
			BeepNegative();
		}
	}

	protected virtual void BeepPositive()
	{
		Debug.Log("Card is valid");
	}

	protected virtual void BeepNegative()
	{
		Debug.Log("Card is invalid");
	}
}
