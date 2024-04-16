using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enemy EventManager
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    // ���Ͱ� Ȱ��ȭ �Ǹ� GameEvent�� ���
    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    // ���Ͱ� ��Ȱ��ȭ �Ǹ� GameEvent���� ����
    public void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    // GameEvent�� ����� �Լ� ȣ��
    public void OnEventRaised()
    {
        Response.Invoke();
    }

}