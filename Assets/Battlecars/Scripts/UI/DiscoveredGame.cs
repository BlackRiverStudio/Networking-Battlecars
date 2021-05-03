using Battlecars.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Battlecars.UI
{
    [RequireComponent(typeof(Button))]
    public class DiscoveredGame : MonoBehaviour
    {
        [SerializeField] private Text ipDisplay;

        private BattlecarsNetworkManager networkManager;

        public void Setup(DiscoveryResponse _response, BattlecarsNetworkManager _manager)
        {
            ipDisplay.text = $"/n {_response.endpoint.Address}";
            networkManager = _manager;

            Button butt = gameObject.GetComponent<Button>();
            butt.onClick.AddListener(JoinGame);
        }

        private void JoinGame()
        {
            // When we click the button, connect to the server displayed on said button.
            networkManager.networkAddress = ipDisplay.text;
            networkManager.StartClient();
        }
    }
}