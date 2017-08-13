using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game options for an Ultimate Texas Holdem table.
/// </summary>
public class GameOptionsController : MonoBehaviour {
  private static Dropdown.OptionData FirstPositionOption = new Dropdown.OptionData("First");
  private static Dropdown.OptionData SecondPositionOption = new Dropdown.OptionData("Second");
  private static Dropdown.OptionData ThirdPositionOption = new Dropdown.OptionData("Third");

  public Dropdown NumPlayersDropdown;
  public Dropdown PositionDropdown;
  public TexasDeckBehaviour GameController;
  public int NumPlayers = 1;
  public int Position;

  /// <summary>
  /// Sets the number of players for the game.
  /// </summary>
  public void SelectNumPlayers() {
    NumPlayers = NumPlayersDropdown.value + 1;
    if (NumPlayers == 1) {
      PositionDropdown.options = new List<Dropdown.OptionData>() {
        FirstPositionOption
      };
    } else if (NumPlayers == 2) {
      PositionDropdown.options = new List<Dropdown.OptionData>() {
        FirstPositionOption,
        SecondPositionOption
      };
    } else {
      PositionDropdown.options = new List<Dropdown.OptionData>() {
        FirstPositionOption,
        SecondPositionOption,
        ThirdPositionOption
      };
    }
    Position = Mathf.Min(NumPlayers - 1, PositionDropdown.value);
  }

  public void SelectPosition() {
    Position = PositionDropdown.value;
  }

  public void StartGame() {
    GameController.StartGame(NumPlayers, Position);
    gameObject.SetActive(false);
  }
}
