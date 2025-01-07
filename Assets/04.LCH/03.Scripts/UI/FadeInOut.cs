using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image Panel;

    public float fadeTime;
    float time = 0f;


    #region Fade Methods
    public void FadeOut()
    {
        StartCoroutine(ChangeDarkScreen());
    }

    public void FadeIn()
    {
        StartCoroutine(ChangeWhiteScreen());
    }

    IEnumerator ChangeDarkScreen()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        GameManager.instance.LoadScene();
        yield return null;
    }

    IEnumerator ChangeWhiteScreen()
    {
        time = 0f;
        Color alpha = Panel.color;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        Panel.gameObject.SetActive(false);
        yield return null;
    }
    #endregion
}
