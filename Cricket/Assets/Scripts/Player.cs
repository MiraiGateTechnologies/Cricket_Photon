using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{

    private NetworkCharacterController _cc;
    private TextMeshProUGUI _messages;
    private TextMeshProUGUI _myName;
    private TextMeshProUGUI _oppName;
    public TextMeshProUGUI oppScore;
    //public UIHandler uiHandlerOpponent;
    public string playerName;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => this.isActiveAndEnabled);
        yield return new WaitUntil(() =>  BasicSpawner.instance.isGameStart == true) ;
        //oppScore = GameObject.FindGameObjectWithTag("OppScore").GetComponent<TextMeshProUGUI>();
        // Find the text object to display messages

        _messages = GameObject.FindGameObjectWithTag("RpcMessage").GetComponent<TextMeshProUGUI>();
        _myName = GameObject.FindGameObjectWithTag("RpcMyName").GetComponent<TextMeshProUGUI>();
        _oppName = GameObject.FindGameObjectWithTag("RpcOppPlayerName").GetComponent<TextMeshProUGUI>();

        //PlayerStats playerStats = GetComponent<PlayerStats>();

        //if (Object.HasInputAuthority)
        //    RPC_SendName(playerStats.PlayerName.ToString());

        //
        //    //
        //}
        //else 
        //{
        //    PlayerStats playerStats = GetComponent<PlayerStats>();
        //    UIHandler.Instance.UpdateOpponentName(playerStats.PlayerName.ToString());
        //    //RPC_SendName(playerStats.PlayerName.ToString());
        //}
        StartCoroutine(SendScore());
    }
 
    private void Update()
    {
        //if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        //{
        //    string message = "Hey Mate!";
            
        //   // RPC_SendName(playerName);
        //}
    }
    int totalScore;
    IEnumerator SendScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("total"+UIHandler.Instance.total);
            PlayerStats playerStats = GetComponent<PlayerStats>();

            if (Object.HasInputAuthority)
                RPC_SendName(playerStats.PlayerName.ToString());


            if (Object.HasInputAuthority)
            {
                RPC_SendMessage("" + UIHandler.Instance.total);
            }
            if (Object.HasInputAuthority)
            {
                RPC_SendBallCount("" + UIHandler.Instance.currentBallsCountInSession);
            }
        }
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendBallCount(string balls, RpcInfo info = default)
    {
        RPC_RelayBallCount(balls, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayBallCount(string balls, PlayerRef messageSource)
    {

        if (messageSource == Runner.LocalPlayer)
        {
            UIHandler.Instance.myBallCount = int.Parse(balls);
        }
        else
        {
            UIHandler.Instance .oppBallCount = int.Parse(balls);
        }

    }


    #region name
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayName(string name, PlayerRef messageSource)
    {

        if (messageSource == Runner.LocalPlayer)
        {

            //playerName = BasicSpawner.instance.nameManager.GetRandomName();
            //UIHandler.Instance.UpdatePlayerName(name);
            //UIHandler uiHandlerOpp = FindAnyObjectByType<UIHandler>();
            //uiHandlerOpp.UpdateOpponentName(name);
            _myName.text = name;
        }
        else
        {
            _oppName.text = name;

        }

       
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendName(string message, RpcInfo info = default)
    {
        RPC_RelayName(message, info.Source);
    }
    #endregion

    #region score

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        
        if (messageSource == Runner.LocalPlayer)
        {
            //message = message$"You: {}\n";

        }
        else
        {
            //message = $"Some other player said: {message}\n";
            _messages.text = message;
            //UIHandler.Instance.UpdateOpponentScore(UIHandler.Instance.total);

        }

       
    }
    #endregion


    private void DisplayMessage(string playerName, string message)
    {
        // Display the message locally
        _messages.text += message;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            
            oppScore.text = data.totalScore.ToString();
            //data.direction.Normalize();
            //_cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}
