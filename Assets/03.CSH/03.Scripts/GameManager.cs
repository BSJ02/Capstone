using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public PlayerData playerData;
    public Player player;

    public Text remainingDistance;

    //UI

    [SerializeField] Texture2D cursorImg;

    public GameObject[] windowUI;
    public GameObject objectUIPanel;
    private GameObject currentlyActiveWindow;


    public Text objectNameText;
    public Text objectHealthText;

    public GameObject[] cardList;


    //���� �� �Ǻ� private ���� �ٲ��ֱ� 
    public bool isGameOver = false; // ���� ���� ����

    


    private static GameManager instance;


    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }



    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }

    private void Start()
    {
        UnityEngine.Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    private void Update()
    {
        //remainingDistance.text = player.playerData.activePoint.ToString();
        //if(playerData.Hp == 0 && !isGameOver)
        //{
        //    isGameOver = true;
        //}

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

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
        //SceneManager.LoadScene(MainScene);
    }

    /* ���


    public void Start, Save ...
     */

    public void LoadSceneButton()
    {
        LoadingSceneController.Instance.LoadScene("SelectTest");
    }

    public void GameExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; ;
#else
        Application.Quit();
#endif
    }

}
