using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Checks if a hand meets the definition of a straight.
/// </summary>
public class RoyalFlushChecker : PokerHandRankChecker {
  public static RoyalFlushChecker Instance = new RoyalFlushChecker();

  public Dictionary<PlayingCard.CardSuit, List<PlayingCard>> SuitCards;
  private List<PlayingCard> cardsInStraight = new List<PlayingCard>();
  private List<PokerHand> flushHands = new List<PokerHand>();

  public override PokerHand CheckHand(List<PlayingCard> cards, int handSize) {
    // Reduce down to cards of a royal flush and check for straight flush.
    var royalCards = cards.Where(card => (int)card.Value > (int)PlayingCard.CardValue.Ten).ToList();

    if (!EnoughCards(royalCards, handSize)) {
      return null;
    }

    var hand = StraightFlushChecker.Instance.CheckHand(royalCards, handSize);
    if (hand != null) {
      hand.Rank = PokerHandRank.RoyalFlush;
    }
    return hand;
  }
}
