using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerStats : NetworkBehaviour
{
    [Networked]
    public NetworkString<_32> PlayerName { get; set; } // Player name property

    IEnumerator Start()
    {
        yield return new WaitUntil(() => this.isActiveAndEnabled);
        PlayerName = BasicSpawner.instance.nameManager.GetRandomName();
    }
}
