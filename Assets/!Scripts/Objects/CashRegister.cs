using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
[RequireComponent(typeof(AudioSource))]
class CashRegister : MonoBehaviour
{
	private int balance = 0;

	AudioSource audioSource = null;

	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
		audioSource = GetComponent<AudioSource>();
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
			audioSource.Play();
		}
	}
}
