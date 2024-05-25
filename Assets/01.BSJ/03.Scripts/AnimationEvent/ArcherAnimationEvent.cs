using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationEvent : MonoBehaviour
{
    private CardManager cardManager;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        particleController = FindObjectOfType<ParticleController>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
