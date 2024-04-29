using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    public GameObject[] windowUI;
    public GameObject objectUIPanel;
    private GameObject currentlyActiveWindow;

    public static UIManager instance;

    public Animator[] animators;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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


    public void Fade(string animationName)
    {
        animators[0].Play(animationName);
    }

    public void Stage(string animationName)
    {
        // 레이어 인덱스는 기본값으로 설정하고, 로그를 사용하여 확인
        int layerIndex = 0; // 기본 레이어 인덱스

        // 애니메이션 상태 이름과 레이어 인덱스를 로그로 출력하여 디버깅
        Debug.Log($"Playing animation '{animationName}' on layer {layerIndex}");

        // 지정된 애니메이션을 재생
        animators[1].Play(animationName, layerIndex);
    }

    public void PlayerTurn(string animationName)
    {
        animators[2].Play(animationName);
    }

    public void MonsterTurn(string animationName)
    {
        animators[3].Play(animationName);
    }

    
}
