using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Workspace.Scripts
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance;

        public UnityAction OnAllPlayersReady;
        
        private Dictionary<ulong,bool> _playersReadyDictionary = new Dictionary<ulong, bool>();
        private bool _isLocalPlayerReady;


        public NetworkVariable<bool> _allPlayersReady = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> _playersCanMove = new NetworkVariable<bool>(false);
        private void Awake()
        {
            instance = this;
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _allPlayersReady.OnValueChanged += OnValueChanged;
        }

        private void OnValueChanged(bool previousvalue, bool newvalue)
        {
            if (newvalue)
            {
                OnAllPlayersReady?.Invoke();
            }
        }

        private void OnEnable()
        {
            CarSelectAreaController.OnCarSelected += SetLocalPlayerReady;
        }

        private void OnDisable()
        {
            CarSelectAreaController.OnCarSelected += SetLocalPlayerReady;
        }

        #region Players Ready

        public void SetLocalPlayerReady(int newStatus)
        {
            _isLocalPlayerReady = true;

            UpdatePlayerReadyStatus_ServerRpc(true);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdatePlayerReadyStatus_ServerRpc(bool status, ServerRpcParams rpcParams = default)
        {
            _playersReadyDictionary[rpcParams.Receive.SenderClientId] = status;

            if (CheckAllPlayersReady())
                _allPlayersReady.Value = true;
        }

        private bool CheckAllPlayersReady()
        {
            bool allPlayersReady = true;

            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (_playersReadyDictionary.ContainsKey(clientId) && _playersReadyDictionary[clientId] != false) continue;
                allPlayersReady = false;
                break;

            }

            return allPlayersReady;
        }

        public void PlayersCanMove()
        {
            if(IsServer)
                _playersCanMove.Value = true;
        }

        #endregion
    }
}
