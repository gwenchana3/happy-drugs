using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class CashRegister : MonoBehaviour
{
	private int balance = 0;

	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
	}

	protected virtual void InteractHandler(InteractableObject interactWith)
	{
		Money money = interactWith.GetComponent<Money>();
		if (money != null)
		{
			if (money.valid)
			{
				balance += money.balance;
			}
			Destroy(money.gameObject);
			Debug.Log(balance);
		}
	}
}
