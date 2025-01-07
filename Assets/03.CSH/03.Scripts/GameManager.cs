using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // UI ������Ʈ ���� ����
    public GameObject[] windowUI;
    private GameObject currentlyActiveWindow;

    public Text objectNameText;
    public Text objectHealthText;

    public GameObject optionUI;

    public GameObject[] cardList;

    public bool isGameOver = false; // ���� ���� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region UI Methods
    // ���� �� �ε�
    public void LoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // �����
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ����
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowLayerWindow(int index)
    {
        if(currentlyActiveWindow != null)
        {
            currentlyActiveWindow.SetActive(false);
        }

        windowUI[index].gameObject.SetActive(true);
        currentlyActiveWindow = windowUI[index];
    }

    public void ExitWindow(int index)
    {
        windowUI[index].gameObject.SetActive(false);
    }

    // �ɼ� â ����
    public void OpenOptionWindow()
    {
        optionUI?.SetActive(true);
    }

    // �ɼ� â �ݱ�
    public void CloseOptionWindow()
    {
        optionUI?.SetActive(false);
    }

    public void cardUIEffect(bool boolean)
    {
        if (boolean == true)
        {
            for (int i = 0; i < cardList.Length; i++)
            {
                cardList[i].transform.localScale *= 1.2f;
            }
        }
        else
        {
            for (int i = 0; i < cardList.Length; i++)
            {
                // ������Ʈ ũ�⸦ ���� ũ��� ����
                cardList[i].transform.localScale = new Vector3(18f, 18f, 18f);
            }
        }
    }
    #endregion
}
