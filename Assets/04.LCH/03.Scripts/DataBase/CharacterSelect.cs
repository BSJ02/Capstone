using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public Character character;

    private void OnMouseUpAsButton()
    {
        DataManager.instance.character = character;    
    }

}
