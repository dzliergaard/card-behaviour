using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Base class for a collection of cards, such as a deck or a player's hand.
/// </summary>
public class CardCollection<T> where T : Card {
  public List<T> Cards;

  public int Count { get { return Cards.Count; } }
  public int Size { get { return Count; } }

  private int randIndex { get { return UnityEngine.Random.Range(0, Count); } }

  public CardCollection() {
    Cards = new List<T>();
  }
  
  public CardCollection(T[] cards) {
    Cards = new List<T>(cards);
  }

  public CardCollection(ICollection<T> cards) {
    Cards = new List<T>(cards);
  }

  /// <summary>
  /// Draw a card from the deck. If pos is not provided, draws first card.
  /// If pos is less than 0 or greater than size of collection, returns the last card (provided there is at least 1).
  /// Removes the card from the collection.
  /// </summary>
  /// <param name="pos">[Optional] Position to draw. Default: "top" (front) of collection.</param>
  /// <returns>Card at position pos (default 0).</returns>
  /// <throws name="InvalidOperationException">If collection is empty.</throws>
  public T Draw(int pos=0) {
    CheckEmpty();

    if (pos < 0 || pos >= Count) {
      pos = Count - 1;
    }

    var card = Cards[pos];
    Cards.Remove(card);
    return card;
  }

  /// <summary>
  /// Draw a random card from the deck.
  /// </summary>
  /// <returns></returns>
  public T Random() {
    return Draw(randIndex);
  }

  /// <summary>
  /// Randomly rearrange the cards using a standard step shuffle.
  /// </summary>
  public void Shuffle() {
    for (var i = 0; i < Count - 1; i++) {
      var index = randIndex;
      var thisCard = Cards[i];
      Cards[i] = Cards[index];
      Cards[index] = thisCard;
    }
  }

  /// <summary>
  /// Insert a card into the collection.
  /// </summary>
  /// <param name="card">Card to insert.</param>
  /// <param name="random">[Optional] If true, put card in random position of deck. Default: false.</param>
  public void AddCard(T card, bool random=false) {
    if (!random) {
      Cards.Add(card);
      return;
    }
    Cards.Insert(randIndex, card);
  }

  private void CheckEmpty() {
    if (Cards.Count == 0) {
      throw new InvalidOperationException("Cannot draw card from an empty Card collection!");
    }
  }
}
