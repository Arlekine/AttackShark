using UnityEngine;

public class SingleSoundPlayer : SoundPlayer
{
    [SerializeField] private AudioClip _sound;

    public override void Play()
    {
        AudioManager.PlaySound(_sound);
    }
}