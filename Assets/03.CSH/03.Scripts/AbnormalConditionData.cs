using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalConditionData : MonoBehaviour
{

    Dictionary<string, int> elementValues = new Dictionary<string, int>()
    {
         { "ice", 10 },
         { "poison", 10 },
         { "fire", 10 }
    };

    public List<string> abCondition = new List<string>(); //Example list;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
