using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking; 

public class NetworkKeyBindings : MonoBehaviour
{
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.Singleton; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartHost();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartServer();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartClient();
        }
    }

    void StartHost()
    {
        if (networkManager != null)
        {
            networkManager.StartHost();
            Debug.Log("Host Started");
        }
    }

    void StartServer()
    {
        if (networkManager != null)
        {
            networkManager.StartServer();
            Debug.Log("Server Started");
        }
    }

    void StartClient()
    {
        if (networkManager != null)
        {
            networkManager.StartClient();
            Debug.Log("Client Started");
        }
    }
}
