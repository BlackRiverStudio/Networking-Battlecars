using Battlecars.Networking;
using UnityEngine;
using UnityEngine.UI;
namespace Battlecars.UI
{
    public class LobbyPlayerSlot : MonoBehaviour
    {
        public bool IsTaken => Player != null;
        public BattlecarsPlayerNet Player { get; private set; } = null;
        public bool IsLeft { get; private set; } = false;

        [SerializeField] private Text nameDisplay;
        [SerializeField] private Button playerButton;

        // Set the player in this slot to the passed player.
        public void AssignPlayer(BattlecarsPlayerNet _player) => Player = _player;

        public void SetSide(bool _left) => IsLeft = _left;

        private void Update()
        {
            // If the slot is empty, then the button shouldn't be active.
            playerButton.interactable = IsTaken;

            // If the player is set, then display their name, otherwise display "Awaiting player..."
            nameDisplay.text = IsTaken ? GetPlayerName() : "Awaiting player...";
        }

        private string GetPlayerName() => string.IsNullOrEmpty(Player.username) ? $"Player {Player.playerId + 1}" : Player.username;
    }
}