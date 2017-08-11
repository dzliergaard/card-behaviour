using System;

/// <summary>
/// Represents a deck of standard playing cards.
/// </summary>
[Serializable]
public class PlayingCardDeck : Deck<PlayingCard> {
  /// <summary>
  /// Creates a standard deck of 52 cards with 4 suits.
  /// </summary>
  /// <param name="shuffled">[Optional] If true, initializes the deck in a random order. Default: false.</param>
  public PlayingCardDeck(bool shuffled=false) {
    for (int i = PlayingCard.MIN_VALUE; i <= PlayingCard.MAX_VALUE; i++) {
      foreach (var suit in PlayingCard.Suits) {
        AddCard(new PlayingCard(suit, i), shuffled);
      }
    }
  }
}