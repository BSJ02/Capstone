using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Knight,
    Fighter,
    Archer,
    Mage
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else if(instance != null) return;
        DontDestroyOnLoad(gameObject);
    }

    public Character character;


}
