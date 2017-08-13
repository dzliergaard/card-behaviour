using System.Collections.Generic;

/// <summary>
/// Checks if a hand contains a flush.
/// </summary>
public class FlushChecker : PokerHandRankChecker {
  public static FlushChecker Instance = new FlushChecker();

  public Dictionary<PlayingCard.CardSuit, List<PlayingCard>> SuitCards;

  protected FlushChecker() {
    SuitCards = new Dictionary<PlayingCard.CardSuit, List<PlayingCard>>();
    foreach (var suit in PlayingCard.Suits) {
      SuitCards[suit] = new List<PlayingCard>();
    }
  }

  public override PokerHand CheckHand(List<PlayingCard> cards, int handSize) {
    if (!EnoughCards(cards, handSize)) {
      return null;
    }

    foreach (var suit in PlayingCard.Suits) {
      SuitCards[suit].Clear();
    }

    List<PlayingCard> mostInSuit = null;
    foreach (var card in cards) {
      SuitCards[card.Suit].Add(card);

      if (mostInSuit == null || mostInSuit.Count < SuitCards[card.Suit].Count) {
        mostInSuit = SuitCards[card.Suit];
      }
    }

    if (mostInSuit == null || mostInSuit.Count < handSize) {
      return null;
    }

    return new PokerHand(mostInSuit.GetRange(0, handSize), PokerHandRank.Flush);
  }
}
