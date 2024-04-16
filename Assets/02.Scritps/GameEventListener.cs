using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enemy EventManager
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    // 몬스터가 활성화 되면 GameEvent에 등록
    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    // 몬스터가 비활성화 되면 GameEvent에서 해제
    public void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    // GameEvent에 등록한 함수 호출
    public void OnEventRaised()
    {
        Response.Invoke();
    }

}