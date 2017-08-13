using System;
using System.Collections.Generic;

/// <summary>
/// Base class for checking pairs, threes, fours, two pairs, and full houses.
/// </summary>
public abstract class SetChecker : PokerHandRankChecker {
  protected List<PlayingCard> HighSet = new List<PlayingCard>();
  protected List<PlayingCard> SecondHighSet = new List<PlayingCard>();

  public int SetSizeOne;
  public int SetSizeTwo;
  public PokerHandRank Rank;

  protected SetChecker(PokerHandRank rank, int sizeOne, int sizeTwo = 0) {
    Rank = rank;
    SetSizeOne = sizeOne;
    SetSizeTwo = sizeTwo;
  }

  public override PokerHand CheckHand(List<PlayingCard> cards, int handSize) {
    if (!EnoughCards(cards, handSize)) {
      return null;
    }

    GetCardRanks(cards);

    // Find the largest and second largest set value.
    foreach (var pair in RankCards) {
      if (pair.Value.Count > HighSet.Count) {
        SecondHighSet = HighSet;
        HighSet = pair.Value;
      } else if (pair.Value.Count > SecondHighSet.Count) {
        SecondHighSet = pair.Value;
      }
    }
    // If first or second set isn't large enough to qualify, we're done.
    if (HighSet == null || HighSet.Count < SetSizeOne) {
      return null;
    }
    if (SetSizeTwo > 0 && (SecondHighSet == null || SecondHighSet.Count < SetSizeTwo)) {
      return null;
    }

    // Add high sets to cards in hand.
    var cardsInHand = new List<PlayingCard>();

    // If sets are same size, higher set is one of higher value.
    if (HighSet.Count == SecondHighSet.Count) {
      if (HighSet[0].Value < SecondHighSet[1].Value) {
        cardsInHand.AddRange(SecondHighSet);
        cardsInHand.AddRange(HighSet);
      } else {
        cardsInHand.AddRange(HighSet);
        cardsInHand.AddRange(SecondHighSet);
      }
    }

    // No need for RankCounts or sets anymore.
    RankCards.Clear();
    HighSet.Clear();
    SecondHighSet.Clear();

    // If we have HandSize or more cards, return hand as is.
    // Otherwise, we need to take non-set high cards until our hand is correct size.
    if (cardsInHand.Count > handSize) {
      return new PokerHand(cardsInHand.GetRange(0, 5), Rank);
    }
    if (cardsInHand.Count == handSize) {
      return new PokerHand(cardsInHand, Rank);
    }

    foreach (var card in cards) {
      if (cardsInHand.Contains(card)) {
        continue;
      }
      cardsInHand.Add(card);
      if (cardsInHand.Count == handSize) {
        return new PokerHand(cardsInHand, Rank);
      }
    }

    // Should never get here.
    throw new InvalidOperationException("Could not build poker hand from cards provided.");
  }
}

/// <summary>
/// Checks if a hand has four of a kind.
/// </summary>
public class FourOfAKindChecker : SetChecker {
  public static FourOfAKindChecker Instance = new FourOfAKindChecker();

  public FourOfAKindChecker() : base(PokerHandRank.FourOfAKind, 4) { }
}

/// <summary>
/// Checks if a hand has a full house.
/// </summary>
public class FullHouseChecker : SetChecker {
  public static FullHouseChecker Instance = new FullHouseChecker();

  public FullHouseChecker() : base(PokerHandRank.FullHouse, 3, 2) { }
}

/// <summary>
/// Checks if a hand has three of a kind.
/// </summary>
public class ThreeOfAKindChecker : SetChecker {
  public static ThreeOfAKindChecker Instance = new ThreeOfAKindChecker();

  public ThreeOfAKindChecker() : base(PokerHandRank.ThreeOfAKind, 3) { }
}

/// <summary>
/// Checks if a hand has two pairs.
/// </summary>
public class TwoPairChecker : SetChecker {
  public static TwoPairChecker Instance = new TwoPairChecker();

  public TwoPairChecker() : base(PokerHandRank.TwoPair, 2, 2) { }
}

/// <summary>
/// Checks if a hand has a pair.
/// </summary>
public class PairChecker : SetChecker {
  public static PairChecker Instance = new PairChecker();

  public PairChecker() : base(PokerHandRank.Pair, 2) { }
}
