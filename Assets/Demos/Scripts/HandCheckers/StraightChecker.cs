using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Checks if the cards contains a straight.
/// </summary>
public class StraightChecker : PokerHandRankChecker {
  public static StraightChecker Instance = new StraightChecker();

  private List<PlayingCard> cardsInStraight = new List<PlayingCard>();

  public override PokerHand CheckHand(List<PlayingCard> cards, int handSize) {
    if (!EnoughCards(cards, handSize)) {
      return null;
    }

    cardsInStraight.Clear();
    GetCardRanks(cards);

    // Go through ranks and see if there are 5 in a row.
    foreach (var value in PlayingCard.Values.Reverse()) {
      if (RankCards.ContainsKey(value)) {
        // Add a card of this rank to the building straight.
        cardsInStraight.Add(RankCards[value][0]);
        if (cardsInStraight.Count == handSize) {
          // Found a long enough straight!
          return new PokerHand(cardsInStraight, PokerHandRank.Straight);
        }
      } else {
        cardsInStraight.Clear();
      }
    }

    // If we are one short and end at 2, check for an ace.
    if (cardsInStraight.Count == handSize - 1) {
      if (cardsInStraight.Last().Value == PlayingCard.CardValue.Two) {
        if (RankCards.ContainsKey(PlayingCard.CardValue.Ace)) {
          cardsInStraight.Add(RankCards[PlayingCard.CardValue.Ace][0]);
          return new PokerHand(cardsInStraight, PokerHandRank.Straight);
        }
      }
    }

    // Did not find HandSize cards in a row.
    return null;
  }
}

