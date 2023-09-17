using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class AudioManager
{
    public enum StaticSound
    {
        UIClick
    }

    private static Dictionary<StaticSound, AudioSource> _staticSources = new Dictionary<StaticSound, AudioSource>();
    private static List<AudioSource> _freeSources = new List<AudioSource>();
    private static List<AudioSource> _busySources = new List<AudioSource>();
    private static Transform _sounds2DParent;

    public static void PlaySound(StaticSound sound)
    {
        if (_staticSources.ContainsKey(sound) == false)
            _staticSources[sound] = CreateNewSource($"StaticSound-{sound}");

        var clip = Resources.Load<AudioClip>(sound.ToString());
        _staticSources[sound].PlayOneShot(clip);
    }

    public static void PlaySound(AudioClip clip)
    {
        if (_freeSources.Count == 0)
            _freeSources.Add(CreateNewSource(clip.name));

        var source = _freeSources[0];
        source.PlayOneShot(clip);
        _freeSources.Remove(source);
        _busySources.Add(source);
    }

    private static void ReturnSourceToFreeAfterPause(AudioSource source, float pause)
    {
        DOTween.Sequence().AppendInterval(pause).AppendCallback(() =>
        {
            _busySources.Remove(source);
            _freeSources.Add(source);
        });
    }

    private static AudioSource CreateNewSource(string name)
    {
        if (_sounds2DParent == null)
            _sounds2DParent = new GameObject("2D Sounds").transform;

        var soundGO = new GameObject(name);
        soundGO.transform.parent = _sounds2DParent;
        return soundGO.AddComponent<AudioSource>();
    }
}
