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
    public Player player;

    public Text remainingDistance;

    public GameObject[] windowUI;
    public GameObject objectUIPanel;
    private GameObject currentlyActiveWindow;


    public Text objectNameText;
    public Text objectHealthText;

    public GameObject[] cardList;


    //���� �� �Ǻ� private ���� �ٲ��ֱ� 
    public bool isGameOver = false; // ���� ���� ����
    private GameObject gameOverUI;

    public bool gameturn = true; // true �÷��̾� �� / false �� ��
    public GameObject playerturn;
    public GameObject enemyturn;


    //�� ���̵� �� / �ƿ�
    private CanvasGroup cg;
    public float fadeTime = 1f;
    float accumTime = 0f;
    private Coroutine fadeCor;

    //bool �÷��̾� ��





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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }

    private void Update()
    {
        //remainingDistance.text = player.playerData.activePoint.ToString();
        if (isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void OnGameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
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

    public void ShowTurn()
    {
        if (gameturn == true)
        {
            playerturn.SetActive(true);
        }
        else
        {
            enemyturn.SetActive(true);
        }
        
    }

   

    public void StartFadeIn()
    {

        if (gameturn == true)
        {
            cg = playerturn.GetComponent<CanvasGroup>();
        }
        else
        {
            cg = enemyturn.GetComponent<CanvasGroup>();
        }

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
        while(accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 1f;

        StartCoroutine(FadeOut());
    }

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


        /* ���


        public void Start, Save ...
         */
    

}
