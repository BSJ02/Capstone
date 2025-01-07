using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSelectList", menuName = "Data/PlayerSelectList", order = 1)]
public class PlayerSelectList : ScriptableObject
{
    public List<GameObject> players = new List<GameObject>();
    public List<int> playerList = new List<int>();
}