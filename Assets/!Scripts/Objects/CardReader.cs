using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
class CardReader : MonoBehaviour
{
	Animator animator = null;
	AudioSource audioSource = null;

	[SerializeField]
	AudioClip _valid;
	[SerializeField]
	AudioClip _invalid;

	protected virtual void Awake()
	{
		InteractableObject interactable = GetComponent<InteractableObject>();
		interactable.OnInteracted += InteractHandler;
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
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
		audioSource.clip = _valid;
		audioSource.Play();
	}

	protected virtual void BeepNegative()
	{
		Debug.Log("Card is invalid");
		animator.SetTrigger("Invalid");
		audioSource.clip = _invalid;
		audioSource.Play();
	}
}
