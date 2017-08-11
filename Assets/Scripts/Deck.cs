using System;
using System.Collections.Generic;

/// <summary>
/// Represents a deck of Cards.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Deck<T> : CardCollection<T> where T : Card {
  public Deck() : base() { }
  public Deck(T[] cards) : base(cards) { }
  public Deck(ICollection<T> cards) : base(cards) { }

  /// <summary>
  /// Return numPlayers hands of handSize cards. 
  /// If numPlayers * handSize is greater than Deck size, throw exception.
  /// </summary>
  /// <param name="handSize">Size of each hand to deal.</param>
  /// <param name="numPlayers">[Optional] Number of hands to deal. Default: 1.</param>
  /// <returns>List of numPlayers Hands of handSize Cards.</returns>
  public List<Hand<T>> DealHands(int handSize, int numPlayers=1) {
    if (handSize * numPlayers > Count) {
      throw new InvalidOperationException(String.Format("Can't deal {0} hands of {1} cards with deck of size {2}!", numPlayers, handSize, Count));
    }

    var result = new List<Hand<T>>(numPlayers);
    for (var cardInd = 0; cardInd < handSize; cardInd++) {
      for (var hand = 0; hand < numPlayers; hand++) {
        result[hand].AddCard(Draw());
      }
    }

    return result;
  }
}
