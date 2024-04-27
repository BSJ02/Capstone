using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("# Player Character")] public GameObject player;
    private GameObject weapon_r;
    private GameObject weapon_l;

    public List<GameObject> rightHandWeaponList;
    public List<GameObject> leftHandWeaponList;

    [Header("# Weapon Type")]
    public List<GameObject> swordList;
    public List<GameObject> axeList;
    public List<GameObject> bowList;
    public List<GameObject> hammerList;
    public List<GameObject> wandList;

    private Transform leftWeaponTransform;
    private Transform rightWeaponTransform;

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
        // 왼손 무기 리스트 저장
        foreach (Transform child in leftWeaponTransform)
        {
            Debug.Log("Child object name: " + child.name);
            if (child.CompareTag("Weapon"))
            {
                leftHandWeaponList.Add(child.gameObject);
            }
        }

        // 오른손 무기 리스트 저장
        foreach (Transform child in rightWeaponTransform)
        {
            if (child.CompareTag("Weapon"))
            {
                rightHandWeaponList.Add(child.gameObject);
            }
        }
    }

    public void ChangeToSword()
    {
        
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
