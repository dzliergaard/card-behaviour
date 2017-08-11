using System;
using System.Linq;

/// <summary>
/// Implementation of Card representing a standard playing card,
/// with a suit [spade, diamond, club, heart] and value [2-10, J, Q, K].
/// </summary>
[Serializable]
public class PlayingCard : Card {
  public static int MIN_VALUE = 1;
  public static int MAX_VALUE = 13;
  public enum CardSuit {
    Spade,
    Diamond,
    Club,
    Heart
  }
  public static CardSuit[] Suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToArray();

  public readonly CardSuit Suit;
  public readonly int Value;

  public PlayingCard(CardSuit suit, int value) {
    Suit = suit;
    Value = value;
  }

  public override int CompareTo(Card other) {
    if (other == null) {
      return 1;
    }
    if (!(other is PlayingCard)) {
      throw new InvalidOperationException(String.Format("Cannot compare card of type {0} to PlayingCard.", other.GetType()));
    }

    return Value.CompareTo((other as PlayingCard).Value);
  }

  public override bool Equals(object other) {
    if (!(other is PlayingCard)) {
      return false;
    }
    var thatCard = other as PlayingCard;
    return thatCard.Value == Value && thatCard.Suit == Suit;
  }

  public override int GetHashCode() {
    return Suit.GetHashCode() * 7 + Value.GetHashCode();
  }

  public static bool operator ==(PlayingCard a, PlayingCard b) {
    if (a == null) {
      return b == null;
    }
    return a.Equals(b);
  }

  public static bool operator !=(PlayingCard a, PlayingCard b) {
    return !(a == b);
  }

  /// <summary>
  /// Returns abbreviated string representation of the playing card.
  /// </summary>
  /// <returns>Short representation of card, such as "3C" (3 of clubs) or "JD" (jack of diamonds).</returns>
  public override string ToString() {
    return GetValueString(Value) + GetSuitString(Suit);
  }

  public string ToLongString() {
    return String.Format("{0} of {1}s", GetLongValueString(Value) + Suit.ToString());
  }

  private static string GetValueString(int value) {
    if (value < MIN_VALUE || value > MAX_VALUE) {
      throw new IndexOutOfRangeException("No string code for card with value " + value);
    }
    // For 2-10, just return the number.
    // For 1, return "A", 11: J, 12: Q, 13: K.
    switch (value) {
    case 1:
      return "A";
    case 11:
      return "J";
    case 12:
      return "Q";
    case 13:
      return "K";
    }
    return value.ToString();
  }

  private static string GetLongValueString(int value) {
    if (value < MIN_VALUE || value > MAX_VALUE) {
      throw new IndexOutOfRangeException("No string code for card with value " + value);
    }
    // For 2-10, just return the number.
    // For 1, return "Ace", 11: Jack, 12: Queen, 13: King.
    switch (value) {
    case 1:
      return "Ace";
    case 11:
      return "Jack";
    case 12:
      return "Queen";
    case 13:
      return "King";
    }
    return value.ToString();
  }

  private static string GetSuitString(CardSuit suit) {
    switch (suit) {
    case CardSuit.Spade:
      return "S";
    case CardSuit.Diamond:
      return "D";
    case CardSuit.Club:
      return "C";
    case CardSuit.Heart:
      return "H";
    }
    throw new InvalidOperationException("Did not find suit string for CardSuit " + suit);
  }
}
