using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject[] windowUI;
    private GameObject currentlyActiveWindow;


    public GameObject gameOverUI;

    private PlayerMove playerMove;

    //버튼 그룹 제어 선언
    public RectTransform buttonLayoutGroup;
    public RectTransform informPanelGroup;
    public GameObject win_inGroupButton;
    public GameObject win_outGroupButton;
    public GameObject inGroupButton;
    public GameObject outGroupButton;

    private int maxActivePoint = 4;
    private int minActivePoint = 0;
    private bool changeBar = false;

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
    public Text TurnText;
    public Text NumCardText;

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
        playerMove = FindObjectOfType<PlayerMove>();
        cardProcessing = FindObjectOfType<CardProcessing>();


        energyFillMaterial.SetFloat("_FillLevel", 1f);


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

    void Update()
    {
        NumCardText.text = CardManager.handCardCount.ToString();
        TurnText.text = BattleManager.turncount.ToString();
    }

    public void NTest()
    {
        Player player = cardProcessing.currentPlayer;

        energyFillMaterial.DOFloat(player.playerData.activePoint * 0.25f, "_FillLevel", 1f);
        //energyFillMaterial.SetFloat("_FillLevel", player.playerData.activePoint * 0.2f);
        energyValue.text = player.playerData.activePoint.ToString();
        
        if(player.playerData.activePoint == 1)
        {
            changeBar = true;
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
}
