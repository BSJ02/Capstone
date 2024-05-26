using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public GameObject[] windowUI;
    private GameObject currentlyActiveWindow;


    public GameObject gameOverUI;

    private PlayerMove playerMove;

    //버튼 그룹 제어 선언
    public RectTransform buttonLayoutGroup;
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
        //playerMove = FindObjectOfType<PlayerMove>();
        cardProcessing = FindObjectOfType<CardProcessing>();


        energyFillMaterial.SetFloat("_FillLevel", 1f);
        energyValue.text = maxActivePoint.ToString();


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
        /*
        if(cardProcessing.selectedTarget != null) 
        {
            energyFillMaterial.SetFloat("_FillLevel", cardProcessing.currentPlayer.playerData.activePoint * 0.2f);
            energyValue.text = cardProcessing.currentPlayer.playerData.activePoint.ToString();
        }
      
        */

        if (cardProcessing.currentPlayer!= null && PlayerMove.isMoving == true)
        {
            NTest();
        }
        else if(changeBar == true)
        {
            energyFillMaterial.DOFloat(0f, "_FillLevel", 1f);
            energyValue.text = minActivePoint.ToString();
            changeBar = false;
        }


        /*
        
        
        if(GameManager.Instance.isGameOver)
        {
            GameManager.Instance.isGameOver = false;
            GameOverFade();
            
        }
        */

        //Energy Bar Material




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

        //StartCoroutine(FadeOut());
    }
    /*
    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3.0f);
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;

    }
    */


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

    }

    public void ButtonInformWindowMoveExit()
    {
     
    }
}
