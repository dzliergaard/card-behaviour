using System;
using System.Collections.Generic;

public enum PokerHandRank {
  HighCard,
  Pair,
  TwoPair,
  ThreeOfAKind,
  Straight,
  Flush,
  FullHouse,
  FourOfAKind,
  StraightFlush,
  RoyalFlush
}

/// <summary>
/// Represents a poker hand of 5 cards.
/// </summary>
public class PokerHand {
  public Hand<PlayingCard> Hand;

  public List<PlayingCard> Cards {
    get { return Hand.Cards; }
  }
  public PokerHandRank Rank;

  /// <summary>
  /// Calculates the hand value with the given cards.
  /// </summary>
  /// <param name="cards"></param>
  public PokerHand(ICollection<PlayingCard> cards, PokerHandRank rank) {
    if (cards == null || cards.Count < 5) {
      throw new InvalidOperationException(string.Format("Cannot build poker hand with {0} cards.", cards == null ? 0 : cards.Count));
    }

    Hand = new Hand<PlayingCard>(cards);
    Rank = rank;
  }
}
