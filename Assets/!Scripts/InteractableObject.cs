using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
class InteractableObject : MonoBehaviour
{
	/// <summary>
	/// The type of this interactable object's behaviour callbacks for interaction
	/// </summary>
	/// <param name="interactWith">The object to interact with (can be null)</param>
	public delegate void InteractHandler(InteractableObject interactWith);

	/// <summary>
	/// The type of this interactable object's behaviour callbacks for dragging
	/// </summary>
	public delegate void DragHandler();

	/// <summary>
	/// The type of this interactable object's behaviour callbacks for dropping
	/// </summary>
	public delegate void DropHandler();

	/// <summary>
	/// Event that's run, when an object is interacted with
	/// </summary>
	public event InteractHandler OnInteracted;

	/// <summary>
	/// Event that's run, when an object is dragged
	/// </summary>
	public event DragHandler OnDragged;

	/// <summary>
	/// Event that's run, when an object is dropped
	/// </summary>
	public event DropHandler OnDropped;

	/// <summary>
	/// Can this object be interacted with?
	/// TODO implement
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("Can this object be interacted with?")]
	public bool interactable { get; protected set; } = true;

	/// <summary>
	/// Can this object be dragged?
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("Can this object be dragged?")]
	public bool draggable { get; protected set; } = false;

	/// <summary>
	/// Can this object respawn?
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("Can this object respawn?")]
	public bool canRespawn { get; protected set; } = true;

	/// <summary>
	/// The position this object should move towards
	/// </summary>
	public Vector3 wantedPos { get; set; } = default;

	/// <summary>
	/// The position this object should respawn at
	/// </summary>
	public Vector3 spawnPoint { get; set; } = default;

	/// <summary>
	/// The rotation this object should respawn with
	/// </summary>
	public Quaternion spawnRotation { get; set; } = default;

	/// <summary>
	/// Is this object being dragged currently
	/// </summary>
	[HideInInspector]
	public bool isDragged = false;

	private Vector3 _velocity = default;

	/// <summary>
	/// Determines how quickly this object moves towards it's goal position
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("Determines how quickly this object moves towards it's goal position")]
	protected float _smoothTime { get; private set; } = 0.1f;

	[SerializeField]
	AudioClip[] sounds = new AudioClip[0];

	protected AudioSource audioSource = null;
	protected Animator animator = null;
	protected new Rigidbody rigidbody = null;

	/// <summary>
	/// Called once the object is instantiated
	/// </summary>
	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		spawnPoint = transform.position;
		spawnRotation = transform.rotation;
	}

	protected virtual void Update()
	{
		if (isDragged)
		{
			Vector3.SmoothDamp(transform.position, wantedPos, ref _velocity, _smoothTime);
			rigidbody.velocity = _velocity;
		}
		if (transform.position.y < -10)
		{
			if (canRespawn)
			{
				transform.position = spawnPoint;
				transform.rotation = spawnRotation;
				rigidbody.velocity = Vector3.zero;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// Interact with this object
	/// </summary>
	/// <param name="interactWith">The object to interact with (can be null)</param>
	public void Interact(InteractableObject interactWith)
	{
		OnInteracted?.Invoke(interactWith);
	}

	/// <summary>
	/// plays the hover animation
	/// </summary>
	public void HoverStart()
	{
		animator.SetBool("Hover", true);
	}

	/// <summary>
	/// stops the hover animation
	/// </summary>
	public void HoverStop()
	{
		animator.SetBool("Hover", false);
	}

	/// <summary>
	/// called when this object is picked up
	/// </summary>
	public void PickUp()
	{
		isDragged = true;
		rigidbody.useGravity = false;
		wantedPos = transform.position;
		foreach (Collider c in GetComponents<Collider>())
		{
			c.enabled = false;
		}
		OnDragged?.Invoke();
		animator.SetBool("Held", true);
	}

	/// <summary>
	/// called when this object is dropped
	/// </summary>
	public void Drop()
	{
		isDragged = false;
		rigidbody.useGravity = true;
		_velocity = Vector3.zero;
		foreach (Collider c in GetComponents<Collider>())
		{
			c.enabled = true;
		}
		OnDropped?.Invoke();
		animator.SetBool("Held", false);
		if (!audioSource.isPlaying)
		{
			audioSource.clip = sounds[Random.Range(0, sounds.Length)];
			audioSource.Play();
		}
	}
}
