using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBackgroundMusic("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
