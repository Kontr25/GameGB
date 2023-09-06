using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    private const string _authGuidKey = "auth_guid_key";
    [SerializeField] private Button _connectButton;
    [SerializeField] private TMP_Text _connectedStatusText;

    private bool _isConnected = false;
    private LoginWithCustomIDRequest request;
    private string id;

    private void Start()
    {
        _connectButton.onClick.AddListener(OnConnectButtonClick);
        
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "7B6D0";
        }
        
        CheckConnectedStatus();

        var needCreation = PlayerPrefs.HasKey(_authGuidKey);
        id = PlayerPrefs.GetString(_authGuidKey, Guid.NewGuid().ToString());
        
        request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = !needCreation };
    }

    private void OnDestroy()
    {
        _connectButton.onClick.RemoveListener(OnConnectButtonClick);
    }

    private void OnConnectButtonClick()
    {
        if(!_isConnected) Connect();
    }

    private void Connect()
    {
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            PlayerPrefs.SetString(_authGuidKey, id);
            OnLoginSuccess(result);
        }, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        _isConnected = true;
        CheckConnectedStatus();
        Debug.Log("Congratulations, you made successful API call!");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        CheckConnectedStatus();
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void CheckConnectedStatus()
    {
        if (_isConnected)
        {
            _connectedStatusText.text = "Connected!";
            _connectedStatusText.color = Color.green;
            _connectButton.gameObject.SetActive(false);
        }
        else
        {
            _connectedStatusText.text = "Disconnected";
            _connectedStatusText.color = Color.red;
        }
    }
}
