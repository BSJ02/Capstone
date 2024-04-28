using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("# Player Character")] public GameObject player;

    private List<GameObject> leftWeaponList;
    private List<GameObject> rightWeaponList;

    [Header("# Weapon Type")]
    public List<GameObject> swordList;
    public List<GameObject> axeList;
    public List<GameObject> bowList;
    public List<GameObject> hammerList;
    public List<GameObject> wandList;
    public List<GameObject> shieldList;

    private Transform leftWeaponTransform;
    private Transform rightWeaponTransform;

    private string adress = "root/pelvis/spine_01/spine_02/spine_03/";
    private string adress_Left = "clavicle_l/upperarm_l/lowerarm_l/hand_l/weapon_l";
    private string adress_Right = "clavicle_r/upperarm_r/lowerarm_r/hand_r/weapon_r";

    private bool leftHand = false;
    private bool rightHand = false;

    private void Awake()
    {
        leftWeaponTransform = transform.Find(adress + adress_Left);
        rightWeaponTransform = transform.Find(adress + adress_Right);
    }

    private void Start()
    {
        // 왼손 무기 리스트 저장
        foreach (Transform childTransform in leftWeaponTransform)
        {
            
            if (childTransform.CompareTag("Weapon"))
            {
                leftWeaponList.Add(childTransform.gameObject);
                AddWeaponByTypeList(childTransform.gameObject);
            }
        }

        // 오른손 무기 리스트 저장
        foreach (Transform childTransform in rightWeaponTransform)
        {
            if (childTransform.CompareTag("Weapon"))
            {
                rightWeaponList.Add(childTransform.gameObject);
                AddWeaponByTypeList(childTransform.gameObject);
            }
        }
    }

    // 리스트
    private void AddWeaponByTypeList(GameObject gameObject)
    {
        if (gameObject.name.IndexOf("Sword") != -1)
        {
            swordList.Add(gameObject.gameObject);
        }
        else if (gameObject.name.IndexOf("Axe") != -1)
        {
            axeList.Add(gameObject.gameObject);
        }
        else if (gameObject.name.IndexOf("Bow") != -1)
        {
            bowList.Add(gameObject.gameObject);
        }
        else if (gameObject.name.IndexOf("Hammer") != -1)
        {
            hammerList.Add(gameObject.gameObject);
        }
        else if (gameObject.name.IndexOf("Wand") != -1)
        {
            wandList.Add(gameObject.gameObject);
        }
        else
        {
            shieldList.Add(gameObject.gameObject);
        }
    }

    private void CheckLeftHandForWeapon(List<GameObject> weaponList)
    {
        int randNum = Random.Range(0, weaponList.Count);

        if (leftHand == false)
        {
            for (int i = 0; i < (float)weaponList.Count / 2; i++)
            {
                weaponList[randNum].SetActive(true);
                leftHand = true;
            }
        }
        else if (rightHand == false)
        {
            for (int i = weaponList.Count / 2; i < swordList.Count; i++)
            {
                weaponList[randNum].SetActive(true);
                rightHand = true;
            }
        }
    }

    public void ChangeToSword()
    {
        int randNum = Random.Range(0, swordList.Count);
        
        if (leftHand == false)
        {
            for (int i = 0; i < (float)swordList.Count / 2; i++)
            {
                swordList[randNum].SetActive(true);
                leftHand = true;
            }
        }
        else if (rightHand == false)
        {
            for (int i = swordList.Count / 2; i < swordList.Count; i++)
            {
                swordList[randNum].SetActive(true);
                rightHand = true;
            }
        }
    }

    public void ChangeToAxe()
    {

    }

    public void ChangeToBow()
    {

    }

    public void ChangeToHammer()
    {

    }

    public void ChangeToWand()
    {

    }

    public bool ContainsWordInName(GameObject gameObject, string word)
    {
        string gameObjectName = gameObject.name;

        return gameObjectName.Contains(word);
    }
}
