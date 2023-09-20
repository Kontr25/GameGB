using System.Collections.Generic;
using DefaultNamespace;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _stateUiText;
    [SerializeField] private RoomCell _roomCellPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _roomCount;

    private int _currentRoomNumber;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private LoadBalancingClient _lbc;
    private const string GAME_MODE_KEY = "gm";
    private const string AI_MODE_KEY = "ai";
    private const string MAP_PROP_KEY = "C0";
    private const string GOLD_PROP_KEY = "C1";
    private TypedLobby _sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
    private TypedLobby _defaultLobby = new TypedLobby("customDefaultLobby", LobbyType.Default);

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);
    }
    
    public void JoinLobby()
    {
        _lbc.OpJoinLobby(_defaultLobby);
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        if(_lbc == null) return;
        
        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = state;
    }

    public void OnConnected()
    {
        
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        JoinLobby();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        
    }

    public void OnCreatedRoom()
    {
        
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        LeaveRoomAfterCreation();
    }
    
    private void LeaveRoomAfterCreation()
    {
        _lbc.OpLeaveRoom(false);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }

    public void OnLeftRoom()
    {
        CreateRooms();
    }

    public void OnJoinedLobby()
    {
        CreateRooms();
        /*var _sqlLobbyFilter = $"{MAP_PROP_KEY} = Map3 ADN {GOLD_PROP_KEY} BETWEEN 300 ADN 500";
        var opJoinRandomRoomParams = new OpJoinRandomRoomParams
        {
            SqlLobbyFilter = _sqlLobbyFilter
        };
        _lbc.OpJoinRandomRoom(opJoinRandomRoomParams);*/
    }
    
    private void CreateRooms()
    {
        if(_currentRoomNumber >= _roomCount) return;
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true,
            CustomRoomPropertiesForLobby = new[] { MAP_PROP_KEY, GOLD_PROP_KEY },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { GOLD_PROP_KEY, 400 }, { MAP_PROP_KEY, "Map3" } }
        };
        
        var enterRoomParams = new EnterRoomParams
        {
            RoomName = "New room " + _currentRoomNumber.ToString(),
            RoomOptions = roomOptions,
            ExpectedUsers = new[] { "23423235" },
            Lobby = _defaultLobby
        };
        
        _lbc.OpCreateRoom(enterRoomParams);
        _currentRoomNumber++;
    }

    public void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }
    
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        
        print("UPDATELIST" + " " + roomList.Count.ToString());
        for(int i=0; i<roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
                RoomCell newRoom = Instantiate(_roomCellPrefab, _content);
                newRoom.Room = info;
            }
        }
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
    }
}
