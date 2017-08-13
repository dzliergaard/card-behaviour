using System;
using System.Linq;

/// <summary>
/// Implementation of Card representing a standard playing card,
/// with a suit [spade, diamond, club, heart] and value [2-10, J, Q, K].
/// </summary>
[Serializable]
public class PlayingCard : Card {
  public static int MIN_VALUE = 2;
  public static int MAX_VALUE = 14;
  public enum CardSuit {
    Spade,
    Diamond,
    Club,
    Heart
  }
  public enum CardValue {
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14
  }

  public static CardSuit[] Suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToArray();
  public static CardValue[] Values = Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToArray();

  public readonly CardSuit Suit;
  public readonly CardValue Value;

  public PlayingCard(CardSuit suit, CardValue value) {
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
    return String.Format("{0} of {1}s", GetValueString(Value, true) + Suit.ToString());
  }

  private static string GetValueString(CardValue value, bool longString=false) {
    if ((int)value < (int)CardValue.Two || (int)value > (int)CardValue.Ace) {
      throw new IndexOutOfRangeException("No string code for card with value " + value);
    }
    // For 2-10, just return the number.
    // For 1, return "A", 11: J, 12: Q, 13: K.
    switch (value) {
    case CardValue.Ace:
      return longString ? value.ToString() : "A";
    case CardValue.Jack:
      return longString ? value.ToString() : "J";
    case CardValue.Queen:
      return longString ? value.ToString() : "Q";
    case CardValue.King:
      return longString ? value.ToString() : "K";
    default:
      return ((int)value).ToString();
    }
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
