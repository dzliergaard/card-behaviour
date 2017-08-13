using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Base class that checks the rank of a set of cards/poker hand.
/// </summary>
public abstract class PokerHandRankChecker {
  private static Dictionary<PokerHandRank, PokerHandRankChecker> Checkers =
      new Dictionary<PokerHandRank, PokerHandRankChecker>() {
          {PokerHandRank.RoyalFlush, RoyalFlushChecker.Instance },
          {PokerHandRank.StraightFlush, StraightFlushChecker.Instance },
          {PokerHandRank.FourOfAKind, FourOfAKindChecker.Instance },
          {PokerHandRank.FullHouse, FullHouseChecker.Instance },
          {PokerHandRank.Flush, FlushChecker.Instance },
          {PokerHandRank.Straight, StraightChecker.Instance },
          {PokerHandRank.ThreeOfAKind, ThreeOfAKindChecker.Instance },
          {PokerHandRank.TwoPair, TwoPairChecker.Instance },
          {PokerHandRank.Pair, PairChecker.Instance }
      };

  private static PokerHandRank[] HandRanks =
      Enum.GetValues(typeof(PokerHandRank)).Cast<PokerHandRank>().Reverse().ToArray();

  protected Dictionary<PlayingCard.CardValue, List<PlayingCard>> RankCards =
      new Dictionary<PlayingCard.CardValue, List<PlayingCard>>();

  public static PokerHand FindBestHand(List<PlayingCard> cards, int handSize) {
    foreach (var rank in HandRanks) {
      var hand = Checkers[rank].CheckHand(cards, handSize);
      if (hand != null) {
        return hand;
      }
    }
    return new PokerHand(cards.GetRange(0, handSize), PokerHandRank.HighCard);
  }

  protected bool EnoughCards(List<PlayingCard> cards, int handSize) {
    return cards.Count >= handSize;
  }

  /// <summary>
  /// Organize the cards by rank into RankCounts.
  /// </summary>
  /// <param name="cards">Cards to sort.</param>
  protected void GetCardRanks(List<PlayingCard> cards) {
    RankCards.Clear();
    cards.ForEach(card => {
      if (RankCards.ContainsKey(card.Value)) {
        RankCards[card.Value].Add(card);
      } else {
        RankCards[card.Value] = new List<PlayingCard>() { card };
      }
    });
  }

  /// <summary>
  /// Checks if the cards can make this type of hand.
  /// </summary>
  /// <param name="cards">Cards to check. Must be at least handSize cards.</param>
  /// <param name="cards">Size of hand to look for.</param>
  /// <returns>A poker hand if 5 of the cards meet the requirements.</returns>
  public abstract PokerHand CheckHand(List<PlayingCard> cards, int handSize);
}
