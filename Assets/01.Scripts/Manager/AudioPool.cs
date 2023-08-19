using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    internal void PlayeAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        StartCoroutine(Pool());
    }

    IEnumerator Pool()
    {
        yield return new WaitForSeconds(source.clip.length * 1.2f);
        AudioManager.Instance.PoolReturn(this);
    }
}
