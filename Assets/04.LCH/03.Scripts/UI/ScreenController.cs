using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    FadeInOut fadeInOut;

    private void Start()
    {
        fadeInOut = FindObjectOfType<FadeInOut>();

        fadeInOut.FadeIn();
    }
}
