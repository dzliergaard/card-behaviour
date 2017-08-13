using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour for an object representing the deck in Ultimate Texas Holdem.
/// </summary>
public class TexasDeckBehaviour : DeckBehaviour {
  private static readonly string[] phaseStrings = new string[] {
    "Reveal Flop",
    "Reveal River",
    "Reveal Hands",
    "Next Hand"
  };
  public int NumPlayers;
  public int PlayerPosition;
  public HandBehaviour DealerHand;
  public HandBehaviour TableCards;
  public Transform PlayerHandsArea;
  public HandBehaviour PlayerMatPrefab;
  public Button ContinueButton;

  private int HandPhase = 0;

  float interval {
    get {
      return PlayerMatPrefab.GetComponentInChildren<MeshRenderer>()
          .bounds.extents.x * 2f;
    }
  }

  /// <summary>
  /// Start a game of Ultimate Texas Holdem.
  /// </summary>
  /// <param name="numPlayers">Number of players.</param>
  /// <param name="playerPosition">Position of human player among players.</param>
  public void StartGame(int numPlayers, int playerPosition) {
    gameObject.SetActive(true);
    NumPlayers = numPlayers;
    PlayerPosition = playerPosition;
    Hands = new HandBehaviour[numPlayers];
    if (NumPlayers % 2 == 1) {
      CreateOddHands();
    } else {
      CreateEvenHands();
    }
    Hands[PlayerPosition].IsPlayerHand = true;

    StartCoroutine(DealHands());
  }

  /// <summary>
  /// Deal cards first to each player, then to the dealer, 
  /// then five to the play area.
  /// </summary>
  private IEnumerator DealHands() {
    // Deal to each player.
    foreach (var hand in Hands) {
      yield return hand.DealCards(DrawCards(2));
    }
    // Deal to dealer.
    yield return DealerHand.DealCards(DrawCards(2));
    // Deal to play area.
    yield return TableCards.DealCards(DrawCards(5));

    ContinueButton.interactable = true;
  }

  protected override IEnumerator SuckUpHands() {
    ContinueButton.interactable = false;
    yield return base.SuckUpHands();
    yield return SuckUpHandCards(DealerHand);
    yield return SuckUpHandCards(TableCards);
    Deck.Shuffle();
    yield return DealHands();
    ContinueButton.GetComponentInChildren<Text>().text = phaseStrings[HandPhase];
    ContinueButton.interactable = true;
  }

  public void ContinueHand() {
    if (HandPhase == 0) {
      StartCoroutine(RevealFlop());
      HandPhase++;
    } else if (HandPhase == 1) {
      StartCoroutine(RevealRiver());
      HandPhase++;
    } else if (HandPhase == 2) {
      StartCoroutine(RevealAllHands());
      HandPhase++;
    } else {
      HandPhase = 0;
      Reset();
    }
  }

  public IEnumerator RevealFlop() {
    ContinueButton.interactable = false;
    yield return TableCards.FlipCards(0, 3);
    ContinueButton.interactable = true;
    ContinueButton.GetComponentInChildren<Text>().text = phaseStrings[HandPhase];
  }

  public IEnumerator RevealRiver() {
    ContinueButton.interactable = false;
    yield return TableCards.FlipCards(3);
    ContinueButton.interactable = true;
    ContinueButton.GetComponentInChildren<Text>().text = phaseStrings[HandPhase];
  }

  public IEnumerator RevealAllHands() {
    ContinueButton.interactable = false;
    foreach (var hand in Hands) {
      if (!hand.IsPlayerHand) {
        yield return hand.FlipCards();
      }
    }
    yield return DealerHand.FlipCards();
    ContinueButton.interactable = true;
    ContinueButton.GetComponentInChildren<Text>().text = phaseStrings[HandPhase];
  }

  private void CreateEvenHands() {
    var middle = NumPlayers / 2;
    var interv = interval;
    var offset = interv - 2 * interv * middle;
    for (var i = 0; i < NumPlayers; i++) {
      var hand = GameObject.Instantiate<HandBehaviour>(
          PlayerMatPrefab, PlayerHandsArea);
      hand.transform.Translate(-offset, 0, 0);
      offset += 2 * interv;
      Hands[i] = hand;
    }
  }

  private void CreateOddHands() {
    var middle = NumPlayers / 2;
    var interv = interval;
    for (var i = 0; i < NumPlayers; i++) {
      var offset = (i - middle) * interv * 2;
      var hand = GameObject.Instantiate<HandBehaviour>(
          PlayerMatPrefab, PlayerHandsArea);
      hand.transform.Translate(-offset, 0, 0);
      offset += 2 * interv;
      Hands[i] = hand;
    }
  }
}
