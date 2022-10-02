using UnityEngine;

class CameraController : MonoBehaviour
{
	[SerializeField]
	float turnSpeed = 0.25f;

	private Vector3 rotation;
	private Vector3 newRotation;

	private int cooldown = 3;

	private Vector3 startingPos;

	protected virtual void Awake()
	{
		rotation = transform.rotation.eulerAngles;
		newRotation = rotation;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			startingPos = Input.mousePosition;
			rotation = newRotation;
		}
		if (Input.GetMouseButton(1))
		{
			Vector3 delta = (Input.mousePosition - startingPos) * turnSpeed;
			// rotate 90
			float y = delta.y;
			delta.y = delta.x;
			delta.x = -y;

			newRotation = rotation + delta;
			newRotation.x = Mathf.Clamp(newRotation.x, -90, 90);

			transform.rotation = Quaternion.Euler(newRotation);
		}
	}
}
