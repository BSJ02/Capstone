using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;

public class UIManager : MonoBehaviour
{
    public GameObject[] windowUI;
    private GameObject currentlyActiveWindow;

    public Camera mainCamera;

    //data players
    public PlayerData warriorData;
    public PlayerData wizardData;

    public GameObject gameOverUI;

    //버튼 그룹 제어 선언
    public RectTransform buttonLayoutGroup;
    public RectTransform informPanelGroup;
    public GameObject win_inGroupButton;
    public GameObject win_outGroupButton;
    public GameObject inGroupButton;
    public GameObject outGroupButton;

    public ScriptableObject playerData;

    public CardProcessing cardProcessing;
   

    //턴 페이드 인 / 아웃
    private CanvasGroup cg;
    public float fadeTime = 1f;
    float accumTime = 0f;
    private Coroutine fadeCor;

    private static UIManager instance;

    //HP Bar material
    public Text energyValue;
    public Material energyFillMaterial;


    //Data Display
    /*
    public Text hpText;
    public Text distanceText;
    */

    //text data
    public Text TurnText;
    public Text NumCardText;

    public GameObject warriorDataText;
    public GameObject wizardDataText;

    public GameObject warriorProfile;
    public GameObject wizardProfile;

    public Text[] warriorTexts;
    public Text[] wizardTexts;

    public Material warriorHpFillMaterial;
    public Material wizardHpFillMaterial;


    public static UIManager Instance
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



    void Awake()
    {
        if (instance == null)
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
        cardProcessing = FindObjectOfType<CardProcessing>();
        wizardDataText.SetActive(true);
    }

    void Update()
    {
        if (cardProcessing.currentPlayerObj != null)
        {
            wizardTexts[0].text = cardProcessing.currentPlayer.playerData.Hp.ToString();
            wizardTexts[1].text = cardProcessing.currentPlayer.playerData.Damage.ToString();
            wizardTexts[2].text = cardProcessing.currentPlayer.playerData.Armor.ToString();
            wizardTexts[3].text = cardProcessing.currentPlayer.playerData.CriticalHit.ToString();

            switch (cardProcessing.currentPlayerObj.name)
            {
                case "Wizard":
                    wizardProfile.SetActive(true);
                    warriorProfile.SetActive(false);
                    break;
                case "Warrior(HP)":
                    warriorProfile.SetActive(true);
                    wizardProfile.SetActive(false);
                    break;
            }
        }
    }

    public void ShowLayerWindow(int index)
    {
        if (currentlyActiveWindow != null)
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

    public void GameOverFade()
    {
        gameOverUI.SetActive(true);
        cg = gameOverUI.GetComponent<CanvasGroup>();

        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }


    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.2f);
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 1f;
    }

    public void ButtonLayoutGroupBarMove()
    {
        buttonLayoutGroup.DOAnchorPos(new Vector3(710, 470, 0), 1);
        inGroupButton.SetActive(false);
        outGroupButton.SetActive(true);
    }

    public void ButtonLayoutGroupBarMoveExit()
    {
        buttonLayoutGroup.DOAnchorPos(new Vector3(1180, 470, 0), 1);
        inGroupButton.SetActive(true);
        outGroupButton.SetActive(false);
    }

    public void ButtonInformWindowMove()
    {
        informPanelGroup.DOAnchorPos(new Vector3(-1100, -337, 0), 1);
        win_inGroupButton.SetActive(false);
        win_outGroupButton.SetActive(true);
    }

    public void ButtonInformWindowMoveExit()
    {
        informPanelGroup.DOAnchorPos(new Vector3(-799, -337, 0), 1);
        win_inGroupButton.SetActive(true);
        win_outGroupButton.SetActive(false);
    }
    public void ExittoLobby()
    {
        SceneManager.LoadScene("01.Lobby");
    }
}
