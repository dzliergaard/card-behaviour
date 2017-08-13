using UnityEngine;

/// <summary>
/// Behaviour representing a single card in the game.
/// </summary>
public class CardBehaviour : MonoBehaviour {
  public PlayingCard Card;

	void Start () {
    // Add a MeshCollider component if none detected.
    if (gameObject.GetComponent<MeshCollider>() == null) {
      gameObject.AddComponent<MeshCollider>();
    }
  }
}
