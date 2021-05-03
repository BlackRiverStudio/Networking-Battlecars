using Battlecars.Networking;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Battlecars.UI
{
    public class ConnectionMenu : MonoBehaviour
    {
        [SerializeField] private Button hostButton, connectButton;
        [SerializeField] private Text ipText;

        [SerializeField] private DiscoveredGame gameTemplate;
        [SerializeField] private Transform foundGameHolder;

        [SerializeField] private BattlecarsNetworkManager networkManager;

        private Dictionary<IPAddress, DiscoveredGame> discoveredGames = new Dictionary<IPAddress, DiscoveredGame>();

        private void Start()
        {
            hostButton.onClick.AddListener(() => networkManager.StartHost());
            connectButton.onClick.AddListener(OnClickConnect);

            networkManager.discovery.onServerFound.AddListener(OnDetectServer);
            networkManager.discovery.StartDiscovery();
        }

        private void OnClickConnect()
        {
            networkManager.networkAddress = ipText.text;
            networkManager.StartClient();
        }

        private void OnDetectServer(DiscoveryResponse _response)
        {
            // Here we have recieved a server that is broadcasting on the network.
            if (!discoveredGames.ContainsKey(_response.endpoint.Address))
            {
                // We haven't already found a game with this IP, so make it.
                DiscoveredGame game = Instantiate(gameTemplate, foundGameHolder);
                game.gameObject.SetActive(true);

                // Setup the game using the response and add it to the list.
                game.Setup(_response, networkManager);
                discoveredGames.Add(_response.endpoint.Address, game);
            }

        }
    }
}