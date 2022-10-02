using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
[RequireComponent(typeof(Animator))]
class CardReader : MonoBehaviour
{
	Animator animator = null;

	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
		animator = GetComponent<Animator>();
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
		animator.SetTrigger("Valid");
	}

	protected virtual void BeepNegative()
	{
		Debug.Log("Card is invalid");
		animator.SetTrigger("Invalid");
	}
}
