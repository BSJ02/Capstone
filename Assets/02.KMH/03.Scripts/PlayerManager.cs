using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject clickedPlayer;

    private PlayerMove playerMove;


    void Start()
    {

    }

    void Update()
    {
        // ClickedPlayer
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerLayerMask))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    //playerMove.playerChoice.SetActive(true);
                    clickedPlayer = hit.collider.gameObject;
                    //playerMove.isActionSelect = true;
                }
            }
        }
    }
}
