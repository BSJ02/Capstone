using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Card;

public class WeaponController : MonoBehaviour
{
    [Header("# Player Character")] public GameObject player;

    private List<GameObject> leftWeaponList = new List<GameObject>();
    private List<GameObject> rightWeaponList = new List<GameObject>();

    [Header("# Weapon Type")]
    public List<GameObject> swordList;
    public List<GameObject> axeList;
    public List<GameObject> bowList;
    public List<GameObject> hammerList;
    public List<GameObject> wandList;
    public List<GameObject> shieldList;

    private Transform leftWeaponTransform;
    private Transform rightWeaponTransform;

    private bool leftHand = false;
    private bool rightHand = false;

    private Dictionary<WeaponType, int> equipWeapon = new Dictionary<WeaponType, int>();

    private string adress = "root/pelvis/spine_01/spine_02/spine_03/";
    private string adress_Left = "clavicle_l/upperarm_l/lowerarm_l/hand_l/weapon_l";
    private string adress_Right = "clavicle_r/upperarm_r/lowerarm_r/hand_r/weapon_r";

    private void Awake()
    {
        leftWeaponTransform = transform.Find(adress + adress_Left);
        rightWeaponTransform = transform.Find(adress + adress_Right);
    }

    private void Start()
    {
        // 왼쪽 무기 리스트 저장
        foreach (Transform childTransform in leftWeaponTransform)
        {
            
            if (childTransform.CompareTag("Weapon"))
            {
                leftWeaponList.Add(childTransform.gameObject);
                AddWeaponByTypeList(childTransform.gameObject);
            }
        }

        // 오른쪽 무기 리스트 저장
        foreach (Transform childTransform in rightWeaponTransform)
        {
            if (childTransform.CompareTag("Weapon"))
            {
                rightWeaponList.Add(childTransform.gameObject);
                AddWeaponByTypeList(childTransform.gameObject);
            }
        }

        equipWeapon[WeaponType.Sword] = 0;
        equipWeapon[WeaponType.Axe] = 0;
        equipWeapon[WeaponType.Bow] = 0;
        equipWeapon[WeaponType.Hammer] = 0;
        equipWeapon[WeaponType.Wand] = 0;
        equipWeapon[WeaponType.Shield] = 0;
    }

    // 무기 타입별로 저장
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
        else if (gameObject.name.IndexOf("Shield") != -1)
        {
            shieldList.Add(gameObject.gameObject);
        }
    }

    private void SwitchWeaponInHand(List<GameObject> weaponList)
    {
        int randNum = Random.Range(0, weaponList.Count);
        GameObject[] targetWeaponList = null;

        if (randNum < weaponList.Count / 2)
        {
            targetWeaponList = leftWeaponList.ToArray();
            leftHand = true;
        }
        else
        {
            targetWeaponList = rightWeaponList.ToArray();
            rightHand = true;
        }

        if (targetWeaponList != null)
        {
            foreach (GameObject weapon in targetWeaponList)
            {
                if (weapon.activeSelf)
                {
                    weapon.SetActive(false);
                }
            }

            for (int i = 0; i < targetWeaponList.Length; i++)
            {
                weaponList[randNum].SetActive(true);
            }
        }
    }

    public void ChangeToWeapon(WeaponType weaponType)
    {
        if (weaponType == Card.WeaponType.Sword)
        {
            SwitchWeaponInHand(swordList);
        }
        else if (weaponType == Card.WeaponType.Axe)
        {
            SwitchWeaponInHand(axeList);
        }
        else if (weaponType == Card.WeaponType.Bow)
        {
            SwitchWeaponInHand(bowList);
        }
        else if (weaponType == Card.WeaponType.Hammer)
        {
            SwitchWeaponInHand(hammerList);
        }
        else if (weaponType == Card.WeaponType.Wand)
        {
            SwitchWeaponInHand(wandList);
        }
        else if (weaponType == Card.WeaponType.Shield)
        {
            SwitchWeaponInHand(shieldList);
        }
    }
}
