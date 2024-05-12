using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public GameObject atkWarrior;
    public GameObject hpWarrior;
    public GameObject wizard;
    public GameObject archer;

    public PlayerSelectList playerSelectList; // ScriptableObject ÂüÁ¶

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerSelectList.players.Clear();
    }

    public void AtkWarriorSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(atkWarrior);
            playerSelectList.playerList.Add(0);
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void HpWarriorSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(hpWarrior);
            playerSelectList.playerList.Add(1);
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void WizardSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(wizard);
            playerSelectList.playerList.Add(2);
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void ArcherSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(archer);
            playerSelectList.playerList.Add(3);
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("KMH");
    }
}
