using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetcodeUIController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button hostButton;

    #endregion

    #region Unity Funcs

    private void Start()
    {
        serverButton.onClick.AddListener(StartServer);
        clientButton.onClick.AddListener(StartClient);
        hostButton.onClick.AddListener(StartHost);
    }

    private void OnDestroy()
    {
        serverButton.onClick.RemoveListener(StartServer);
        clientButton.onClick.RemoveListener(StartClient);
        hostButton.onClick.RemoveListener(StartHost);
    }

    #endregion

    #region Server-Client-Host Starting Process

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        
        UIController.OnNetcodeUiSelected?.Invoke();
    }
    
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    

    #endregion    
}
