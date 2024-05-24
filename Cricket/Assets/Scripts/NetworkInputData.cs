using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    //public string playerName { get; set; }
    public int totalScore { get; set; }
    public Vector3 direction;
}
