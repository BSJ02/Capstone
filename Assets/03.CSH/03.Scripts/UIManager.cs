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

    void Update()
    {
        warriorTexts[0].text = warriorData.Hp.ToString();
        warriorTexts[1].text = warriorData.Damage.ToString();
        warriorTexts[2].text = warriorData.Armor.ToString();
        warriorTexts[3].text = warriorData.CriticalHit.ToString();

        wizardTexts[0].text = wizardData.Hp.ToString();
        wizardTexts[1].text = wizardData.Damage.ToString();
        wizardTexts[2].text = wizardData.Armor.ToString();
        wizardTexts[3].text = wizardData.CriticalHit.ToString();




        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치에서 레이캐스트를 쏩니다
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 레이캐스트가 맞은 오브젝트 확인
                if (hit.transform != null)
                {
                    // 맞은 오브젝트가 특정 태그를 가진 경우 처리
                    if (hit.transform.name == "Warrior(HP)")
                    {
                        if (warriorDataText != null)
                        {
                            warriorDataText.SetActive(true);
                            wizardDataText.SetActive(false);

                            warriorProfile.SetActive(true);
                            wizardProfile.SetActive(false);
                        }
                    }

                    if (hit.transform.name == "Wizard")
                    {
                        if (wizardDataText != null)
                        {
                            wizardDataText.SetActive(true);
                            warriorDataText.SetActive(false);

                            wizardProfile.SetActive(true);
                            warriorProfile.SetActive(false);

                        }
                    }
                }
            }
        }

     

        

        NumCardText.text = CardManager.handCardCount.ToString();
        TurnText.text = BattleManager.turncount.ToString();
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
