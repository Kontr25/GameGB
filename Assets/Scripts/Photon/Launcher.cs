using System;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_Text _connectedStatusText;
        [SerializeField] private TMP_Text _connectButtonText;
        [SerializeField] private Color _connectedButtonColor;
        [SerializeField] private Color _disconnectedButtonColor;

        private bool _isConnected = false;
        private LoginWithCustomIDRequest request;
        private string gameVersion = "1";
        
        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            CheckConnectedStatus();
            _connectButton.onClick.AddListener(OnConnectButtonClick);
        }

        private void OnDestroy()
        {
            _connectButton.onClick.RemoveListener(OnConnectButtonClick);
        }

        private void OnConnectButtonClick()
        {
            if(!_isConnected) Connect();
            else Disconnect();
        }
        
        private void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                    PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        private void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        public override void OnConnectedToMaster()
        {
            _isConnected = true;
            CheckConnectedStatus();
            Debug.Log("OnConnectedToMaster");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnected = false;
            CheckConnectedStatus();
            Debug.Log(cause.ToString());
        }
        
        private void CheckConnectedStatus()
        {
            if (_isConnected)
            {
                _connectedStatusText.text = "Connected!";
                _connectedStatusText.color = Color.green;
                _connectButton.image.color = _disconnectedButtonColor;
                _connectButtonText.text = "Disconnect";
            }
            else
            {
                _connectedStatusText.text = "Disconnected";
                _connectedStatusText.color = Color.red;
                _connectButton.image.color = _connectedButtonColor;
                _connectButtonText.text = "Connect";
            }
        }
    }
}