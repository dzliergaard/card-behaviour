using System;

/// <summary>
/// Abstract class representing a card that can be drawn from a deck, hidden, revealed.
/// </summary>
[Serializable]
public abstract class Card : IComparable<Card> {
  /// <summary>
  /// (Required) Name of the card.
  /// </summary>
  public string Name { get; set; }
  /// <summary>
  /// (Optional) A description of the card.
  /// </summary>
  public string Description {
    get {
      return null;
    }
    set { }
  }

  public abstract int CompareTo(Card other);

  /// <summary>
  /// Behavior that occurs when a card is "played" in the game.
  /// </summary>
  public virtual void Play() { }
}
