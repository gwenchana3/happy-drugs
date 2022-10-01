using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
class Stamp : MonoBehaviour
{
	/// <summary>
	/// Does this stamp have ink currently
	/// </summary>
	public bool has_ink { get; set; } = true;
}
