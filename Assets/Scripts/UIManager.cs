using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMP_Text statusText;
    public TMP_Text playersText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);

        NetworkManager.Singleton.OnClientConnectedCallback += UpdatePlayers;
        NetworkManager.Singleton.OnClientDisconnectCallback += UpdatePlayers;
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        statusText.text = "Modo HOST";
    }
    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        statusText.text = "Modo CLIENTE";
    }
    void UpdatePlayers(ulong id)
    {
        playersText.text = "Jugadores conectados:\n";

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            playersText.text += "Cliente ID: " + client.ClientId + "\n";
        }
    }
}
