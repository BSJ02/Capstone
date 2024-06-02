using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public GameObject atkWarrior;
    public GameObject hpWarrior;
    public GameObject wizard;
    public GameObject archer;

    public PlayerSelectList playerSelectList; // ScriptableObject 

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerSelectList.players.Clear();
        playerSelectList.playerList.Clear();
    }

    public void HpWarriorSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(hpWarrior);
            playerSelectList.playerList.Add(0);

            Debug.Log("=== Player List ===");
            foreach (GameObject obj in playerSelectList.players)
            {
                Debug.Log(obj.name);
            }
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void AtkWarriorSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(atkWarrior);
            playerSelectList.playerList.Add(1);

            Debug.Log("=== Player List ===");
            foreach (GameObject obj in playerSelectList.players)
            {
                Debug.Log(obj.name);
            }
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

            Debug.Log("=== Player List ===");
            foreach (GameObject obj in playerSelectList.players)
            {
                Debug.Log(obj.name);
            }
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

            Debug.Log("=== Player List ===");
            foreach (GameObject obj in playerSelectList.players)
            {
                Debug.Log(obj.name);
            }
        }
        else
        {
            Debug.Log("The maximum number is 2.");
        }
    }

    public void Initialize()
    {
        playerSelectList.players.Clear();
        playerSelectList.playerList.Clear();
        Debug.Log("Initialization Complete");
    }

    public void GameStart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
