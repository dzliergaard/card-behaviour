/// <summary>
/// Behaviour for an object representing a deck of playing cards.
/// </summary>
public class DrawDeckBehaviour : DeckBehaviour {
  /// <summary>
  /// Draw top card of the deck and give it to the player.
  /// </summary>
  private void OnMouseDown() {
    Hands[0].AddCard(DrawCard());
  }
}
