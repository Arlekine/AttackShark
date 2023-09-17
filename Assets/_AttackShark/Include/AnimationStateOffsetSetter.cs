using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStateOffsetSetter : MonoBehaviour
{
    [SerializeField] private string[] _cycleOffsetParameterNames = new []{"offset"};
    [Range(0, 1)] [SerializeField] private float _cycleOffsetMax = 0.3f;

    private void Start()
    {
        var animator = GetComponent<Animator>();
        foreach (var cycleOffsetParameterName in _cycleOffsetParameterNames)
        {
            animator.SetFloat(cycleOffsetParameterName, Random.Range(0f, _cycleOffsetMax));
        }
    }
}
