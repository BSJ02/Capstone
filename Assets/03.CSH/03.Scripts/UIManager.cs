using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject[] windowUI;
    private GameObject currentlyActiveWindow;

    public GameObject gameOverUI;

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

        cardProcessing = FindObjectOfType<CardProcessing>();
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        

    }

    void Update()
    {


      
            energyFillMaterial.SetFloat("_FillLevel", cardProcessing.currentPlayer.playerData.activePoint * 0.2f);
            energyValue.text =cardProcessing.currentPlayer.playerData.activePoint.ToString();
        /*
        
        if(GameManager.Instance.isGameOver)
        {
            GameManager.Instance.isGameOver = false;
            GameOverFade();
            
        }
        */

        //Energy Bar Material

        
        

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


    public void SetEnergyText()
    {
        
    }
}
