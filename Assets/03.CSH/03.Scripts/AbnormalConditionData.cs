using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalConditionData : MonoBehaviour
{
   /* public int asdf(string str, int damage, int turn)
    {
   */
        public Dictionary<string, int> elementValues = new Dictionary<string, int>()
        {
            { "ice", 10 },
            { "poison", 10 },
            { "fire", 10 }
        };
    /*
        turn--;
        if (turn == 0)
        {

            return 0;
        }
        else
        {
            return (int)elementValues[str];
        }
        
    }
    */


    /*public Dictionary<Dictionary<string, int>, int> elementValue = new Dictionary<Dictionary<string, int>, int>()
    {
         { "ice", 10 },
         { "poison", 10 },
         { "fire", 10 }
    };
    */

    public List<string> abConditions = new List<string>(); //Example list;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
