using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public GameObject Warrior;
    public GameObject Archer;
    public GameObject Wizard;
    public GameObject Paladin;

    public PlayerSelectList playerSelectList; // ScriptableObject 

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // playerSelectList.players.Clear();
        // playerSelectList.playerList.Clear();
    }

    public void GameStart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void WarriorSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(Warrior);
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

    public void ArcherSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(Archer);
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
            playerSelectList.players.Add(Wizard);
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

    public void PaladinSelect()
    {
        if (playerSelectList.players.Count < 2)
        {
            playerSelectList.players.Add(Paladin);
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
}
