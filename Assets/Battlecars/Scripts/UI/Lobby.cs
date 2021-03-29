using Battlecars.Networking;
using System.Collections.Generic;
using UnityEngine;
namespace Battlecars.UI
{
    public class Lobby : MonoBehaviour
    {
        private List<LobbyPlayerSlot> leftTeamSlots = new List<LobbyPlayerSlot>();
        private List<LobbyPlayerSlot> rightTeamSlots = new List<LobbyPlayerSlot>();

        [SerializeField] private GameObject leftTeamHolder;
        [SerializeField] private GameObject rightTeamHolder;

        // Flipping bool that determins which column the connected player will be added to.
        private bool assigningToLeft = true;

        private BattlecarsPlayerNet localPlayer;

        public void AssignPlayerToLobbySlot(BattlecarsPlayerNet _player, bool _left, int _slotId)
        {
            // Get the correct slot list depending on the left param.
            List<LobbyPlayerSlot> slots = _left ? leftTeamSlots : rightTeamSlots;
            // Assign the player to the relevant slot in this list.
            slots[_slotId].AssignPlayer(_player);
        }

        public void OnPlayerConnected(BattlecarsPlayerNet _player)
        {
            bool assigned = false;

            // If the player is the localPlayer, assign it.
            if (_player.isLocalPlayer) localPlayer = _player;

            List<LobbyPlayerSlot> slots = assigningToLeft ? leftTeamSlots : rightTeamSlots;

            // Loop through each item in the list and run a lambda w/ the item at that index.
            slots.ForEach(slot =>
            {
                if (assigned) return;
                else if (!slot.IsTaken)
                {
                    slot.AssignPlayer(_player);
                    slot.SetSide(assigningToLeft);
                    assigned = true;
                }
            });

            for (int i = 0; i < leftTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = leftTeamSlots[i];
                if (slot.IsTaken) localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
            }

            for (int i = 0; i < rightTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = rightTeamSlots[i];
                if (slot.IsTaken) localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
            }

            // Flip the flag so that the next one will end up in the other list.
            assigningToLeft = !assigningToLeft;
        }

        private void Start()
        {
            leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
            rightTeamSlots.AddRange(rightTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
        }
    }
}