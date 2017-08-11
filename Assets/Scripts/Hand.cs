using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a hand of Cards, held by the player or other entity.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Hand<T> : CardCollection<T> where T : Card {
  public Hand() : base() { }
  public Hand(T[] cards) : base(cards) { }
  public Hand(ICollection<T> cards) : base(cards) { }
}
