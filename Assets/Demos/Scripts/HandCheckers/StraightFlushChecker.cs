using System.Collections.Generic;

/// <summary>
/// Checks if a hand meets the definition of a straight.
/// </summary>
public class StraightFlushChecker : PokerHandRankChecker {
  public static StraightFlushChecker Instance = new StraightFlushChecker();

  public Dictionary<PlayingCard.CardSuit, List<PlayingCard>> SuitCards;
  private List<PlayingCard> cardsInStraight = new List<PlayingCard>();
  private List<PokerHand> flushHands = new List<PokerHand>();

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

    // If not enough cards in any suit to hit a flush, we're done.
    if (mostInSuit.Count < handSize) {
      return null;
    }

    // Find straights in any of the suited groups.
    flushHands.Clear();
    foreach (var pair in SuitCards) {
      var hand = StraightChecker.Instance.CheckHand(pair.Value, handSize);
      if (hand != null) {
        flushHands.Add(hand);
      }
    }

    if (flushHands.Count == 0) {
      return null;
    }

    // If only one suit has a flush, return it. Otherwise, find the highest
    // value flush of all hands.
    if (flushHands.Count == 1) {
      return flushHands[0];
    }

    PokerHand highestHand = null;
    foreach (var hand in flushHands) {
      if (highestHand == null || highestHand.Cards[0].Value < hand.Cards[0].Value) {
        highestHand = hand;
      }
    }

    highestHand.Rank = PokerHandRank.StraightFlush;
    return highestHand;
  }
}
