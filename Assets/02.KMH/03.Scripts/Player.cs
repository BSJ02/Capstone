using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    Idle,
    Moving,
    Attack
}

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerState playerState;
    public int activePoint = 3;

    public Button turnExit;

    public void ExitPlayerBehaivor()
    {
        BattleManager.instance.MonsterTurn();
    }

    

}
