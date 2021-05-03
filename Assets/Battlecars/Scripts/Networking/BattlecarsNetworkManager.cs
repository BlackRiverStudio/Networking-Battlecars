using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Battlecars.Networking
{
    public class BattlecarsNetworkManager : NetworkManager
    {
        /// <summary>A reference to the Battlecars version of the NetworkManager singleton.</summary>
        public static BattlecarsNetworkManager Instance => singleton as BattlecarsNetworkManager;

        /// <summary>Whether or not this NetworkManager is the host.</summary>
        public bool IsHost { get; private set; } = false;

        public BattlecarsNetworkDiscovery discovery;

        private Dictionary<byte, BattlecarsPlayerNet> players = new Dictionary<byte, BattlecarsPlayerNet>();

        /// <summary>Runs only when connecting to an online scene as a host.</summary>
        public override void OnStartHost()
        {
            IsHost = true;
            discovery.AdvertiseServer();
        }

        /// <summary>Attempts to return a player corresponding to the passed ID. If no player found, returns null.</summary>
        public BattlecarsPlayerNet GetPlayerForId(byte _playerId)
        {
            players.TryGetValue(_playerId, out BattlecarsPlayerNet player);
            return player;
        }

        // Runs when a client connects to the server. This function is responsible for creating the player
        // object, and placing it in the scene. It is also responsible for making sure the connection is aware
        // of what their player object is.
        public override void OnServerAddPlayer(NetworkConnection _conn)
        {
            // Give us the next spawn position, depending on the spawn mode.
            Transform spawnPos = GetStartPosition();

            // Spawn a player, and try to use the spawnPos.
            GameObject playerObj = spawnPos != null ? Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation) : Instantiate(playerPrefab);

            // Assign the player's ID and add them to the server based on the connection.
            AssignPlayerId(playerObj);
            NetworkServer.AddPlayerForConnection(_conn, playerObj);
        }

        /// <summary>Removes the player with the corresponding ID from the dictonary.</summary>
        /// <param name="_id"></param>
        public void RemovePlayer(byte _id)
        {
            // If the player is present in the dictionary, remove them.
            if (players.ContainsKey(_id)) players.Remove(_id);
        }

        public void AddPlayer(BattlecarsPlayerNet _player)
        {
            if (!players.ContainsKey(_player.playerId)) players.Add(_player.playerId, _player);
        }

        protected void AssignPlayerId(GameObject _playerObj)
        {
            byte id = 0;
            // Generate a list that is sorted by key's value.
            List<byte> playerIDs = players.Keys.OrderBy(x => x).ToList();
            // Loop through all keys in the previous list, and assign the id.
            foreach (byte key in playerIDs) if (id == key) id++;

            // Get the playerNet component from the gameObject and assign it's playerId.
            BattlecarsPlayerNet player = _playerObj.GetComponent<BattlecarsPlayerNet>();
            player.playerId = id;
            players.Add(id, player);
        }
    }
}