﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStateManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectPlayer;
    [SerializeField] private AudioClip deathEffect;
    [SerializeField] private AudioClip loopEffect;
    [SerializeField] private AudioClip levelCompletedEffect;

    private void Start()
    {
        GameManager.Instance.soundManager = this;
    }
    public void PlayLevelCompleted()
    {
        effectPlayer.clip = levelCompletedEffect;
        effectPlayer.Play();
    }
    public void PlayDeath()
    {
        effectPlayer.clip = deathEffect;
        effectPlayer.Play();
    }
    public void LoopEffect()
    {
        effectPlayer.clip = loopEffect;
        effectPlayer.Play();
    }
}
