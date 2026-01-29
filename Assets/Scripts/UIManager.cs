using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    //Solución Tarea UT2
    public Button colorButtton;

    public TMP_Text statusText;
    public TMP_Text playersText;
    private MovementPlayer miSumotoriScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        colorButtton.onClick.AddListener(ChangeColor);

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

        
        // Obtener el player local solo si aún no lo tenemos
        if (miSumotoriScript == null &&
            NetworkManager.Singleton.LocalClient != null &&
            NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            var playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
            miSumotoriScript = playerObject.GetComponent<MovementPlayer>();
        }
    }

    void ChangeColor()
    {
        if (miSumotoriScript != null)
        {
            miSumotoriScript.ChangeColorRandomRpc();
        }
    }
}
