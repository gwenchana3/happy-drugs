using UnityEngine;
using Util;

class ObjectSelector : MonoBehaviour
{
	/// <summary>
	/// The main camera
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("The main camera")]
	protected Camera _camera { get; private set; } = null;

	/// <summary>
	/// Fire input buffer
	/// </summary>
	protected TimeBufferedValue fireBuffer;

	/// <summary>
	/// The delay for the fire input buffer
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("The delay for the fire input buffer")]
	protected float _fireBufferTimer { get; private set; } = 0.1f;

	private InteractableObject _current = null;

	/// <summary>
	/// The gameobject that's currently hovered over
	/// </summary>
	public InteractableObject current
	{
		get
		{
			return _current;
		}
		set
		{
			if (_current != value)
			{
				if (_current != null && _current != _held)
				{
					_current.HoverStop();
				}
				_current = value;
				if (_current != null)
				{
					_current.HoverStart();
				}
			}
		}
	}

	private InteractableObject _held = null;

	/// <summary>
	/// The gameobject that's currently hovered over
	/// </summary>
	public InteractableObject held
	{
		get
		{
			return _held;
		}
		set
		{
			if (_held != value)
			{
				if (_held != null)
				{
					_held.HoverStop();
					_held.Drop();
					if (current != null)
					{
						current.Interact(_held);
					}
				}
				_held = value;
				if (_held != null)
				{
					_held.PickUp();
					_held.HoverStart();
				}
			}
		}
	}

	/// <summary>
	/// The physics layers objects can be dragged on
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("The physics layers objects can be dragged on")]
	protected LayerMask _dragLayers { get; private set; } = 0;

	/// <summary>
	/// Called once the object is instantiated
	/// </summary>
	protected virtual void Awake()
	{
		fireBuffer = new TimeBufferedValue(_fireBufferTimer);
	}

	/// <summary>
	/// Called every frame
	/// </summary>
	protected virtual void Update()
	{
		// update input buffers and such
		UpdateTimeBufferedValues();

		// activate fire buffer on fire
		if (Input.GetButtonDown("Fire1"))
		{
			fireBuffer.Active = true;
		}

		// get vectors for raycast
		Vector3 origin = _camera.transform.position;
		Vector3 cursorPos = _camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * _camera.nearClipPlane);
		Vector3 dir = cursorPos - _camera.transform.position;

		// Raycast
		RaycastHit hit;
		bool didHit = Physics.Raycast(origin, dir, out hit, float.PositiveInfinity, _dragLayers);

		// hover logic
		InteractableObject next = didHit ? hit.transform.gameObject.GetComponent<InteractableObject>() : null;
		current = next;

		// holding logic
		if (held != null)
		{
			if (Input.GetButton("Fire1") && didHit)
			{
				// move held
				held.wantedPos = hit.point;
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				// drop held
				held = null;
			}
		}
		else if (current != null && fireBuffer.Active)
		{
			// grab current
			held = current;
			fireBuffer.Active = false;
		}
	}

	/// <summary>
	/// Calles the Update function of the TimeBufferedValues of this object
	/// </summary>
	protected virtual void UpdateTimeBufferedValues()
	{
		fireBuffer.Update(Time.deltaTime);
	}
}
