using UnityEngine;

/// <summary>
/// Rotates an object on hover, and rotates back on mouse exit.
/// </summary>
[RequireComponent(typeof(MeshCollider))]
public class RotateBehaviour : MonoBehaviour {
  public float HoverRotation;
  public float TimeToRotate;
  public Transform ToRotate;

  private Quaternion initialRotation;
  private Quaternion hoverRotation;
  private Quaternion targetRotation;
  private float rotIncrement;

  void Start () {
    // If no GameObject to rotate provided, rotate self.
    if (ToRotate == null) {
      ToRotate = transform;
    }

    initialRotation = ToRotate.localRotation;
    var hoverEuler = initialRotation.eulerAngles + new Vector3(0, HoverRotation, 0);
    hoverRotation = Quaternion.Euler(hoverEuler.x, hoverEuler.y, hoverEuler.z);
    rotIncrement = Mathf.Abs(HoverRotation / TimeToRotate);
  }

  private void Update() {
    if (ToRotate.localRotation == targetRotation) {
      return;
    }

    ToRotate.localRotation = Quaternion.RotateTowards(ToRotate.localRotation, targetRotation, Time.deltaTime * rotIncrement);
  }

  /// <summary>
  /// Rotate the top card slightly when hovering over, as though to draw a card.
  /// </summary>
  private void OnMouseEnter() {
    targetRotation = hoverRotation;
  }

  private void OnMouseExit() {
    targetRotation = initialRotation;
  }
}
