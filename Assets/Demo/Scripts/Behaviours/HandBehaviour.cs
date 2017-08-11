using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for the player's hand.
/// </summary>
public class HandBehaviour : MonoBehaviour {
  public float ShiftTime = 1f;
  public Hand<PlayingCard> Hand;
  public List<CardBehaviour> Cards;

  float interval {
    get {
      var cardRenderer = Cards[0].GetComponent<MeshRenderer>();
      return cardRenderer.bounds.extents.x * 1.2f;
    }
  }

  void Start() {
    Hand = new Hand<PlayingCard>();
    Cards = new List<CardBehaviour>();
  }

  public void AddCard(CardBehaviour card) {
    Hand.AddCard(card.Card);
    card.transform.SetParent(transform);
    Cards.Add(card);
    StartCoroutine(AdjustCardPositions(card));
  }

  /// <summary>
  /// Shifts card positions so they are all in line.
  /// </summary>
  /// <param name="card">New card to flip once everything is shifted.</param>
  /// <returns></returns>
  private IEnumerator AdjustCardPositions(CardBehaviour card) {
    var startPositions = new Vector3[Cards.Count];
    for (var i = 0; i < Cards.Count; i++) {
      startPositions[i] = Cards[i].transform.position;
    }
    var targetPositions = new Vector3[Cards.Count];

    // If odd number of cards, the middle card will be in the exact center and
    // the ones to either side will spread out.
    // If even number, the top half start slightly to the right and bottom start
    // slightly to the left.
    if (Cards.Count % 2 == 0) {
      CalculateEvenPositions(targetPositions);
    } else {
      CalculateOddPositions(targetPositions);
    }

    // Shift cards over ShiftTime seconds.
    var startTime = Time.time;
    var endTime = startTime + ShiftTime;
    while (Time.time < endTime) {
      for (var i = 0; i < Cards.Count; i++) {
        var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
        Cards[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], iLerp);
      }
      yield return null;
    }

    for (var i = 0; i < Cards.Count; i++) {
      Cards[i].transform.position = targetPositions[i];
    }

    // Rotate card to face forward.
    startTime = Time.time;
    endTime = startTime + ShiftTime;
    var startRotation = card.transform.rotation;
    var targetEuler = startRotation.eulerAngles + new Vector3(0, 180, 0);
    var targetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);
    while (Time.time < endTime) {
      var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
      card.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, iLerp);
      yield return null;
    }
    card.transform.rotation = targetRotation;
  }

  private void CalculateEvenPositions(Vector3[] targetPositions) {
    var middle = Cards.Count / 2;
    var interv = interval;

    var offset = interv - 2 * interv * middle;
    for (var i = 0; i < Cards.Count; i++) {
      targetPositions[i] = new Vector3(offset, transform.position.y, 0);
      offset += 2 * interv;
    }
  }

  private void CalculateOddPositions(Vector3[] targetPositions) {
    var middle = Cards.Count / 2;
    var interv = interval;
    for (var i = 0; i < Cards.Count; i++) {
      var offset = (i - middle) * interv * 2;
      targetPositions[i] = new Vector3(offset, transform.position.y, 0);
    }
  }
}
