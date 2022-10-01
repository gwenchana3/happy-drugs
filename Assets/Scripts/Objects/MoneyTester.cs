using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class MoneyTester : MonoBehaviour
{
	void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	protected virtual void InteractHandler(InteractableObject interactWith)
	{
		Money money = interactWith.GetComponent<Money>();
		if (money != null && money.valid)
		{
			BeepPositive();
		}
		else
		{
			BeepNegative();
		}
	}

	protected virtual void BeepPositive()
	{
		Debug.Log("Money is valid");
	}

	protected virtual void BeepNegative()
	{
		Debug.Log("Money is invalid");
	}
}
