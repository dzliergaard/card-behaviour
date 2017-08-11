using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Behaviour for an object representing a deck of playing cards.
/// </summary>
public class DeckBehaviour : MonoBehaviour {
  public PlayingCardDeck Deck;
  public PlayingCardPrefabs CardPrefabs;
  public HandBehaviour Hand;
  public int MaxHandSize = 7;

  private int numCards { get { return Hand.Cards.Count; } }

	void Start () {
    Deck = new PlayingCardDeck(true);
	}

  /// <summary>
  /// Return all cards to the deck from the hand.
  /// </summary>
  public void Reset() {
    var handSize = Hand.Hand.Count - 1;
    for (var i = handSize; i >= 0; i--) {
      Deck.AddCard(Hand.Hand.Draw());
    }

    StartCoroutine(SuckUpHandCards());
  }

  private IEnumerator SuckUpHandCards() {
    var startPositions = new Vector3[Hand.Cards.Count];
    var startRotations = new Quaternion[Hand.Cards.Count];
    for (var i = 0; i < Hand.Cards.Count; i++) {
      startPositions[i] = Hand.Cards[i].transform.position;
      startRotations[i] = Hand.Cards[i].transform.rotation;
    }

    // Rotate all cards back to face down.
    var startTime = Time.time;
    var endTime = startTime + Hand.ShiftTime;
    var targetEuler = transform.rotation.eulerAngles + new Vector3(0, 180, 0);
    var targetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);
    while (Time.time < endTime) {
      var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
      for (var i = 0; i < Hand.Cards.Count; i++) {
        Hand.Cards[i].transform.rotation = Quaternion.Lerp(startRotations[i], targetRotation, iLerp);
      }
      yield return null;
    }

    // Suck all cards back to the deck.
    startTime = Time.time;
    endTime = startTime + Hand.ShiftTime;
    while (Time.time < endTime) {
      var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
      for (var i = 0; i < Hand.Cards.Count; i++) {
        Hand.Cards[i].transform.position = Vector3.Lerp(startPositions[i], transform.position, iLerp);
      }
      yield return null;
    }
    for (var i = Hand.Cards.Count - 1; i >= 0; i--) {
      DestroyImmediate(Hand.transform.GetChild(0).gameObject);
    }
    Hand.Cards.Clear();
  }

  /// <summary>
  /// Draw top card of the deck and give it to the player.
  /// </summary>
  private void OnMouseDown() {
    // If max cards have been drawn, do nothing.
    if (numCards >= MaxHandSize) {
      return;
    }
    var card = CardPrefabs.CreateCardGameObject(Deck.Draw());
    card.transform.position = transform.position;
    card.transform.rotation = transform.rotation;
    var euler = card.transform.rotation.eulerAngles;
    euler.y -= 180;
    var newRotation = new Quaternion() {
      eulerAngles = euler
    };
    card.transform.rotation = newRotation;
    Hand.AddCard(card);
  }
}
