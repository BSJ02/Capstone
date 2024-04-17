using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 10.0f;


   void OnMouseDown()
    {
        GameManager.Instance.ShowObjectUI(this.gameObject);
    }
}
