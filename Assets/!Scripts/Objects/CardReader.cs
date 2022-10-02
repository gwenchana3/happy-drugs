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
		}
		else
		{
			BeepNegative();
		}
	}

	protected virtual void BeepPositive()
	{
		animator.SetTrigger("Valid");
		audioSource.clip = _valid;
		audioSource.Play();
	}

	protected virtual void BeepNegative()
	{
		animator.SetTrigger("Invalid");
		audioSource.clip = _invalid;
		audioSource.Play();
	}
}
