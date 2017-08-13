using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for the player's hand.
/// </summary>
public class HandBehaviour : MonoBehaviour {
  public float ShiftTime = .2f;
  public Hand<PlayingCard> Hand;
  public List<CardBehaviour> Cards;
  public bool IsPlayerHand;

  float interval {
    get {
      var cardRenderer = Cards[0].GetComponent<MeshRenderer>();
      return cardRenderer.bounds.extents.x * 1.2f;
    }
  }

  void Awake() {
    Hand = new Hand<PlayingCard>();
    Cards = new List<CardBehaviour>();
  }

  public void AddCard(CardBehaviour card) {
    Hand.AddCard(card.Card);
    card.transform.SetParent(transform);
    Cards.Add(card);
    StartCoroutine(AdjustCardPositions(card, IsPlayerHand));
  }

  public IEnumerator DealCards(CardBehaviour[] cards) {
    // First deal the cards.
    foreach (var card in cards) {
      Hand.AddCard(card.Card);
      card.transform.SetParent(transform);
      Cards.Add(card);
      yield return StartCoroutine(AdjustCardPositions(card));
    }
    // Now flip all the cards if applicable, separated by 1/2 ShiftTime.
    if (IsPlayerHand) {
      foreach (var card in Cards) {
        StartCoroutine(FlipCard(card));
        var startTime = Time.time;
        var endTime = startTime + .5f * ShiftTime;
        while (Time.time < endTime) {
          yield return null;
        }
      }
    }
  }

  /// <summary>
  /// Flip some or all of the cards in the hand.
  /// </summary>
  /// <param name="start">Start flipping at this index.</param>
  /// <param name="num">Number of cards to reveal.</param>
  public IEnumerator FlipCards(int start=0, int num=0) {
    if (num == 0) {
      num = Cards.Count - start;
    }
    for (var i = start; i < start + num; i++) {
      yield return FlipCard(Cards[i]);
    }
  }

  /// <summary>
  /// Shifts card positions so they are all in line.
  /// </summary>
  /// <param name="card">New card to flip once everything is shifted.</param>
  /// <returns></returns>
  private IEnumerator AdjustCardPositions(CardBehaviour card, bool flipImmediately=false) {
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

    // If player hand, immediately rotate card to face forward.
    if (flipImmediately) {
      yield return FlipCard(card);
    }
  }

  private IEnumerator FlipCard(CardBehaviour card) {
    var startTime = Time.time;
    var endTime = startTime + ShiftTime;
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
      targetPositions[i] = transform.position + new Vector3(-offset, 0, 0);
      offset += 2 * interv;
    }
  }

  private void CalculateOddPositions(Vector3[] targetPositions) {
    var middle = Cards.Count / 2;
    var interv = interval;
    for (var i = 0; i < Cards.Count; i++) {
      var offset = (i - middle) * interv * 2;
      targetPositions[i] = transform.position + new Vector3(-offset, 0, 0);
    }
  }
}
