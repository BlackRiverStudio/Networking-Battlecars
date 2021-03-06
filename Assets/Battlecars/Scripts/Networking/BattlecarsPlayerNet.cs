using Battlecars.Player;
using Battlecars.UI;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Battlecars.Networking
{
    [RequireComponent(typeof(PlayerMotor))]
    public class BattlecarsPlayerNet : NetworkBehaviour
    {
        [SyncVar] public byte playerId;
        [SyncVar] public string username = "";

        private Lobby lobby;
        private bool hasJoinedLobby = false;

        #region Network'd

        #region Set Username
        public void SetUsername(string _name)
        {
            // Only localPlayers can call Commands as localPlayers are the only ones who have the authority to talk to the server.
            if (isLocalPlayer) CmdSetUsername(_name);
        }
        [Command] public void CmdSetUsername(string _name) => username = _name;
        #endregion

        #region Assign Player To Lobby Slot
        public void AssignPlayerToSlot(bool _left, int _slotId, byte _playerId)
        {
            // Only localPlayers can call Commands as localPlayers are the only ones who have the authority to talk to the server.
            if (isLocalPlayer) CmdAssignPlayerToLobbySlot(_left, _slotId, _playerId);
        }
        [Command] public void CmdAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId) => RpcAssignPlayerToLobbySlot(_left, _slotId, _playerId);
        [ClientRpc] public void RpcAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId)
        {
            // If this is running on the host client, ignore this call.
            if (BattlecarsNetworkManager.Instance.IsHost) return;

            // Find the lobby in the scene and set the player to the correct slot.
            StartCoroutine(AssignPlayerToLobbySlotDelayed(BattlecarsNetworkManager.Instance.GetPlayerForId(_playerId), _left, _slotId));
        }
        private IEnumerator AssignPlayerToLobbySlotDelayed(BattlecarsPlayerNet _player, bool _left, int _slotId)
        {
            // Keep trying to get the lobby until it's not null.
            Lobby lobby = FindObjectOfType<Lobby>();
            while (lobby == null)
            {
                yield return null;
                lobby = FindObjectOfType<Lobby>();
            }

            // Lobby successfully got, so assign the player.
            lobby.AssignPlayerToLobbySlot(_player, _left, _slotId);
        }
        #endregion

        #endregion

        private void Start()
        {
            // If we are the localPlayer, setup the movement script
            if (isLocalPlayer)
            {
                PlayerMotor playerMotor = gameObject.GetComponent<PlayerMotor>();
                playerMotor.Setup();
            }
        }

        private void Update()
        {
            // Determine if we are on the host client.
            if (BattlecarsNetworkManager.Instance.IsHost)
            {
                // Attempt to get the lobby if we haven't already joined the lobby.
                if (lobby == null && !hasJoinedLobby) lobby = FindObjectOfType<Lobby>();

                // Attempt to join the lobby if we haven't already and the lobby is set.
                if (lobby != null && !hasJoinedLobby)
                {
                    hasJoinedLobby = true;
                    lobby.OnPlayerConnected(this);
                }
            }
        }

        public override void OnStartClient() => BattlecarsNetworkManager.Instance.AddPlayer(this);

        // Runs only if the object is connect to the localPlayer.
        public override void OnStartLocalPlayer() => SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);

        // Runs when the client is disconnected from the server.
        public override void OnStopClient()
        {
            // Remove the playerID from the server.
            BattlecarsNetworkManager.Instance.RemovePlayer(playerId);
        }
    }
}