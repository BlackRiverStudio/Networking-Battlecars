using Battlecars.Networking;
using UnityEngine;
using UnityEngine.UI;
namespace Battlecars.UI
{
    public class LobbyPlayerSlot : MonoBehaviour
    {
        public bool IsTaken => player != null;

        [SerializeField] private Text nameDisplay;
        [SerializeField] private Button playerButton;

        private BattlecarsPlayerNet player = null;

        public void AssignPlayer(BattlecarsPlayerNet _player) => player = _player;

        private void Update()
        {
            // If the slot is empty, then the button shouldn't be active.
            playerButton.interactable = IsTaken;

            // If the player is set, then display their name, otherwise display "Awaiting player..."
            nameDisplay.text = IsTaken ? player.username : "Awaiting player...";
        }
    }
}