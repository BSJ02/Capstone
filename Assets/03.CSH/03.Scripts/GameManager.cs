using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{

    //UI
    public GameObject[] windowUI;
    public GameObject objectUIPanel;
    private GameObject currentlyActiveWindow;

    public Text objectNameText;
    public Text objectHealthText;

    public Slider bgm_slider;
    public Slider efs_slider;

    public GameObject optionUI;

    public GameObject[] cardList;

    //게임 턴 판별 private 으로 바꿔주기 
    public bool isGameOver = false; // 게임 오버 상태

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

        bgm_slider = bgm_slider.GetComponent<Slider>();
        efs_slider = efs_slider.GetComponent<Slider>();

        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        bgm_slider.onValueChanged.AddListener(ChangeEfsSound);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionUI != null)
            {
                bool isActive = optionUI.activeSelf;
                optionUI.SetActive(!isActive);
            }
        }
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
                // 오브젝트 크기를 원래 크기로 변경
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

    /* 기능


    public void Start, Save ...
     */

    public void LoadSceneButton()
    {
        LoadingSceneController.Instance.LoadScene("03.Select");
    }

    public void GameExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; ;
#else
        Application.Quit();
#endif
    }


    void ChangeBgmSound(float value)
    {
        SoundManager.instance.backgroundMusicSource.volume = value;
    }

    void ChangeEfsSound(float value)
    {
        SoundManager.instance.soundEffectsSource.volume = value;
    }

    public void OptionWindow()
    {
        if (optionUI != null)
        {
            bool isActive = optionUI.activeSelf;
            optionUI.SetActive(!isActive);
        }
    }
}
