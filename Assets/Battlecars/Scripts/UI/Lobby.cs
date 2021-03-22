using Battlecars.Networking;
using System.Collections;
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

        public void OnPlayerConnected(BattlecarsPlayerNet _player)
        {
            bool assigned = false;

            List<LobbyPlayerSlot> slots = assigningToLeft ? leftTeamSlots : rightTeamSlots;

            // Loop through each item in the list and run a lambda w/ the item at that index.
            slots.ForEach(slot =>
            {
                if (assigned) return;
                else if (!slot.IsTaken)
                {
                    slot.AssignPlayer(_player);
                    assigned = true;
                }
            });

            // Flip the flag so that the next one will end up in the other list.
            assigningToLeft = !assigningToLeft;
        }

        private void Start()
        {
            leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
            rightTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
        }
    }
}