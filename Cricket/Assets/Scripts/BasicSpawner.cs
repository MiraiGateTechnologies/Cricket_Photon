using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

using Fusion.Photon.Realtime;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public static BasicSpawner instance;

    
    public UIHandler uIHandler;
    public bool isGameStart = false;
    public GameObject ui;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public NetworkRunner _runner;

    public int joinedPlayerCount;

    public NameManager nameManager;
    private Dictionary<PlayerRef, string> playerNames = new Dictionary<PlayerRef, string>();
    public string playerName;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 0f;
    }



    async void StartGame(GameMode mode)
    {
       
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
       

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
           // SessionName = "CricketLive",
            PlayerCount = 2,
            IsOpen = true,
            IsVisible = true,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });;
        if (mode == GameMode.Host)
        {
            SetRandomPlayerName(_runner.LocalPlayer);
        }
        else if (mode == GameMode.Client)
        {
            SetRandomPlayerName(_runner.LocalPlayer);
        }
    }
    private void SetRandomPlayerName(PlayerRef player)
    {
        if (nameManager != null)
        {

            playerName = nameManager.GetRandomName();
            //Debug.Log("Random name for player " + player.RawEncoded + ": " + randomPlayerName);

        }
    }
    public void Host()
    {
        
        StartGame(GameMode.Host);
        UIHandler.Instance.coins -= 500;
        UIHandler.Instance.UpdateCoins(UIHandler.Instance.coins);
    }

    public void Join()
    {
        StartGame(GameMode.Client);
        UIHandler.Instance.coins -= 500;
        UIHandler.Instance.UpdateCoins(UIHandler.Instance.coins);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
           
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
           
        }


        //if (!playerNames.ContainsKey(player))
        //{
        //    // Fetch or request player name (e.g., through UI input)
        //    // For now, let's assume we have a method GetPlayerName(PlayerRef player) that retrieves the name

        //    //playerNames.Add(player, playerName);

        //    // Update UI with opponent names
        //    //UpdateOpponentNamesUI();
        //    NetworkObject playerObject = runner.GetPlayerObject(runner.LocalPlayer);
        //    if (playerObject != null)
        //    {
        //        PlayerStats playerStat = playerObject.GetComponent<PlayerStats>();
        //        if (playerStat != null)
        //        {
        //            string playerName = playerStat.PlayerName.ToString();

        //            // Update the local player's name
        //            if (player == _runner.LocalPlayer)
        //            {
        //                // Update local player's name in UI or wherever necessary
        //                Debug.Log("Local player's name: " + playerName);
        //                UIHandler.Instance.UpdateOpponentName(playerName);
        //            }
        //            else
        //            {
        //                //string playerName = playerStat.PlayerName.ToString();
        //                playerNames.Add(player, playerName);
        //            }


        //            // Update UI with opponent names
        //            Invoke("UpdateOpponentNamesUI", 1f);
        //        }
        //    }
        //}
        joinedPlayerCount += 1;
        if (runner.Config.Simulation.PlayerCount == joinedPlayerCount)
        {
            ui.SetActive(false);
            isGameStart = true;
            _runner.ProvideInput = true;
            Time.timeScale = 1f;
            //Invoke("unpuse", 3f);
        }
    }

    public void unpuse()
    {
        Time.timeScale = 1f;
    }
    void UpdateOpponentNamesUI()
    {
        
        foreach (var kvp in playerNames)
        {
            if (kvp.Key != _runner.LocalPlayer)
            {
                Debug.Log("Opponent name: " + kvp.Value);
                UIHandler.Instance.UpdateOpponentName(kvp.Value);
            }
        }
    }
    void IterateDictionary()
    {
        if (_runner.LocalPlayer != null)
        {
            //uIHandler = _runner.GetPlayerObject(_runner.LocalPlayer).GetComponent<UIHandler>();

        }
        foreach (var kvp in _spawnedCharacters)
        {
            PlayerRef playerRef = kvp.Key;
            NetworkObject networkObject = kvp.Value;

            // Use playerRef and networkObject as needed
            Debug.Log("PlayerRef: " + playerRef.ToString() + ", NetworkObject: " + networkObject.ToString());
            NetworkObject playerObject = _runner.GetPlayerObject(_runner.LocalPlayer);
            PlayerStats playerStat = playerObject.GetComponent<PlayerStats>();
            if (playerStat != null)
            {
                UIHandler.Instance.UpdatePlayerName(playerStat.PlayerName.ToString());
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //    var data = new NetworkInputData();
        //    if (UIHandler.Instance.isNumberCick)
        //    {
        //        data.totalScore = UIHandler.Instance.total;
        //        input.Set(data);
        //        UIHandler.Instance.isNumberCick = false;
        //    }
    }
public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) 
    {
        Debug.Log("OnConnectedToServer");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    
   
}
