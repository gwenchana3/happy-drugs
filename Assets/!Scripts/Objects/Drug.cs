using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class Drug : MonoBehaviour
{
	/// <summary>
	/// The type of this drug
	/// </summary>
	[field: SerializeField]
	[field: Tooltip("The type of this drug")]
	public DrugType drugType { get; private set; } = DrugType.LSD;
}
