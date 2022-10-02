using UnityEngine;

class CameraController : MonoBehaviour
{
	[SerializeField]
	float turnSpeed = 0.25f;

	private Vector3 rotation;

	private int cooldown = 3;

	private Vector3 startingPos = Vector3.zero;

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			startingPos = Input.mousePosition;
			rotation = transform.rotation.eulerAngles;
		}
		if (Input.GetMouseButton(1))
		{
			Vector3 newRotation = (Input.mousePosition - startingPos) * turnSpeed;
			float y = newRotation.y;
			newRotation.y = newRotation.x;
			newRotation.x = -y;

			newRotation += rotation;
			newRotation.x = Mathf.Clamp(newRotation.x, -90, 90);

			transform.rotation = Quaternion.Euler(newRotation);
			return;
		}
	}
}
