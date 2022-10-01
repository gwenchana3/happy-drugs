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

	/// <summary>
	/// The gameobject that's currently hovered over
	/// </summary>
	public InteractableObject current { get; private set; } = null;

	/// <summary>
	/// The gameobject that's currently hovered over
	/// </summary>
	public InteractableObject held { get; private set; } = null;

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
		UpdateTimeBufferedValues();

		// activate fire buffer on fire
		bool justFired = Input.GetButtonDown("Fire1");
		if (justFired)
		{
			fireBuffer.Active = true;
		}

		Vector3 origin = _camera.transform.position;
		Vector3 cursorPos = _camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * _camera.nearClipPlane);
		Vector3 dir = cursorPos - _camera.transform.position;

		// Raycast
		RaycastHit hit;
		bool didHit = Physics.Raycast(origin, dir, out hit, float.PositiveInfinity, _dragLayers);

		// hover logic
		InteractableObject next = didHit ? hit.transform.gameObject.GetComponent<InteractableObject>() : null;
		if (next != current)
		{
			if (current != null && current != held)
			{
				// stop hovering current
				current.HoverStop();
			}

			// update current
			current = next;

			if (current != null)
			{
				// start hover
				current.HoverStart();
			}
		}

		if (held != null)
		{
			if (Input.GetButton("Fire1") && didHit)
			{
				RaycastHit hitWorld;
				Physics.Raycast(hit.point + Vector3.up * 20f, Vector3.down, out hitWorld);

				held.wantedPos = hitWorld.point;
			}
			if (Input.GetButtonUp("Fire1"))
			{
				held.HoverStop();
				held.Drop();
				if (current != null)
				{
					current.Interact(held);
				}
				held = null;
			}
		}
		else if (current != null && fireBuffer.Active)
		{
			held = current;
			held.PickUp();
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
