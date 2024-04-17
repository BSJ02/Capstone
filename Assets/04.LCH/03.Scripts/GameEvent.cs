using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	���� ���� �̺�Ʈ ȣ��
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObject/GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
	List<GameEventListener> listeners = new List<GameEventListener>();

	public bool isPaused { get; private set; }

	public void Raise()
	{
        for (int i = 0; i <= listeners.Count - 1; i++) // ���� �ݴ�(ù ��° ������Ʈ�� ������ �ε����� ��ġ)
        {
			listeners[i].OnEventRaised(); // GameEventListener�� ��ϵǾ� �ִ� �Լ� ȣ��
			
        }
	}

	// listener ���
	public void RegisterListener(GameEventListener listener)
	{
		listeners.Add(listener);
	}

	// ��ϵ� listener ����
	public void UnregisterListener(GameEventListener listener)
	{
		listeners.Remove(listener);
	}

}