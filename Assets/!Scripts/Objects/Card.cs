using UnityEngine;

class Card : MonoBehaviour
{
	/// <summary>
	/// Determines the validity, of this card
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("Is this card valid, or not")]
	public bool valid { get; private set; } = true;

	/// <summary>
	/// The amount of money on the card
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("The amount of money on the card")]
	public int balance { get; private set; } = 0;
}
