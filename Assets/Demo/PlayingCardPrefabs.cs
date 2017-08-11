using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Store and retrieve all the playing card models.
/// </summary>
[CreateAssetMenu(menuName = "CardPrefabs")]
public class PlayingCardPrefabs : ScriptableObject {
  public List<GameObject> SpadeModels;
  public List<GameObject> DiamondModels;
  public List<GameObject> ClubModels;
  public List<GameObject> HeartModels;

  public CardBehaviour CreateCardGameObject(PlayingCard card) {
    var value = card.Value - 1;
    GameObject model;
    switch (card.Suit) {
    case PlayingCard.CardSuit.Spade:
      model = SpadeModels[value];
      break;
    case PlayingCard.CardSuit.Diamond:
      model = DiamondModels[value];
      break;
    case PlayingCard.CardSuit.Club:
      model = ClubModels[value];
      break;
    case PlayingCard.CardSuit.Heart:
      model = HeartModels[value];
      break;
    default:
      throw new InvalidOperationException("Cannot get card model for card " + card);
    }

    var cardObject = GameObject.Instantiate(model);
    var cardComponent = cardObject.AddComponent<CardBehaviour>();
    cardComponent.Card = card;
    return cardComponent;
  }
}
