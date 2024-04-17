using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	몬스터 관련 이벤트 호출
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObject/GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
	List<GameEventListener> listeners = new List<GameEventListener>();

	public bool isPaused { get; private set; }

	public void Raise()
	{
        for (int i = 0; i <= listeners.Count - 1; i++) // 순서 반대(첫 번째 오브젝트가 마지막 인덱스에 위치)
        {
			listeners[i].OnEventRaised(); // GameEventListener에 등록되어 있는 함수 호출
			
        }
	}

	// listener 등록
	public void RegisterListener(GameEventListener listener)
	{
		listeners.Add(listener);
	}

	// 등록된 listener 해제
	public void UnregisterListener(GameEventListener listener)
	{
		listeners.Remove(listener);
	}

}