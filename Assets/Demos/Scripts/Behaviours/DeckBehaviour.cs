using System.Collections;
using UnityEngine;

/// <summary>
/// Behaviour for an object representing a deck of playing cards.
/// </summary>
public class DeckBehaviour : MonoBehaviour {
  public PlayingCardDeck Deck;
  public PlayingCardPrefabs CardPrefabs;
  public HandBehaviour[] Hands;
  public int MaxHandSize = 7;

  private int numCards {
    get {
      if (Hands.Length == 0) {
        return 0;
      }
      return Hands[0].Cards.Count;
    }
  }

  protected virtual void Awake() {
    Deck = new PlayingCardDeck(true);
  }

  /// <summary>
  /// Return all cards to the deck from the hands.
  /// </summary>
  public void Reset() {
    StartCoroutine(SuckUpHands());
  }

  protected virtual IEnumerator SuckUpHands() {
    foreach (var hand in Hands) {
      yield return SuckUpHandCards(hand);
    }
  }

  protected IEnumerator SuckUpHandCards(HandBehaviour hand) {
    var startPositions = new Vector3[hand.Cards.Count];
    var startRotations = new Quaternion[hand.Cards.Count];
    for (var i = 0; i < hand.Cards.Count; i++) {
      startPositions[i] = hand.Cards[i].transform.position;
      startRotations[i] = hand.Cards[i].transform.rotation;
    }

    // Rotate all cards back to face down.
    var startTime = Time.time;
    var endTime = startTime + hand.ShiftTime;
    var targetEuler = transform.rotation.eulerAngles + new Vector3(0, 180, 0);
    var targetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);
    while (Time.time < endTime) {
      var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
      for (var i = 0; i < hand.Cards.Count; i++) {
        hand.Cards[i].transform.rotation = Quaternion.Lerp(startRotations[i], targetRotation, iLerp);
      }
      yield return null;
    }

    // Suck all cards back to the deck.
    startTime = Time.time;
    endTime = startTime + hand.ShiftTime;
    while (Time.time < endTime) {
      var iLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
      for (var i = 0; i < hand.Cards.Count; i++) {
        hand.Cards[i].transform.position = Vector3.Lerp(startPositions[i], transform.position, iLerp);
      }
      yield return null;
    }
    for (var i = hand.Cards.Count - 1; i >= 0; i--) {
      Deck.AddCard(hand.Cards[i].Card);
      DestroyImmediate(hand.Cards[i].gameObject);
    }
    hand.Cards.Clear();
  }

  /// <summary>
  /// Draw the top card of the deck.
  /// </summary>
  /// <returns>Top card from the deck.</returns>
  public CardBehaviour DrawCard() {
    // If max cards have been drawn, do nothing.
    if (numCards >= MaxHandSize) {
      return null;
    }
    var card = CardPrefabs.CreateCardGameObject(Deck.Draw());
    card.transform.position = transform.position;
    card.transform.rotation = transform.rotation;
    card.transform.Rotate(0, 180, 0);
    return card;
  }

  public CardBehaviour[] DrawCards(int numCards=1) {
    var cards = new CardBehaviour[numCards];
    for (var i = 0; i < numCards; i++) {
      cards[i] = DrawCard();
    }
    return cards;
  }
}
