using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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


    public void Fade(string animationName)
    {
        animators[0].Play(animationName);
    }

    public void Stage(string animationName)
    {
        // ���̾� �ε����� �⺻������ �����ϰ�, �α׸� ����Ͽ� Ȯ��
        int layerIndex = 0; // �⺻ ���̾� �ε���

        // �ִϸ��̼� ���� �̸��� ���̾� �ε����� �α׷� ����Ͽ� �����
        Debug.Log($"Playing animation '{animationName}' on layer {layerIndex}");

        // ������ �ִϸ��̼��� ���
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
