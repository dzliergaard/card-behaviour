using System;
using System.Collections.Generic;
using UnityEngine;

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
    var index = (int)card.Value - 1;
    if (card.Value == PlayingCard.CardValue.Ace) {
      index = 0;
    }
    GameObject model;
    switch (card.Suit) {
    case PlayingCard.CardSuit.Spade:
      model = SpadeModels[index];
      break;
    case PlayingCard.CardSuit.Diamond:
      model = DiamondModels[index];
      break;
    case PlayingCard.CardSuit.Club:
      model = ClubModels[index];
      break;
    case PlayingCard.CardSuit.Heart:
      model = HeartModels[index];
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
