using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : SoundPlayer
{
    [SerializeField] private AudioClip[] _sounds;
    
    public override void Play()
    {
        var randomSound = _sounds[Random.Range(0, _sounds.Length)];
        AudioManager.PlaySound(randomSound);
    }
}