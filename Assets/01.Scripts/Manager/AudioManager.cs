using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioPool poolPrefab;
    private Queue<AudioPool> soundPool = new();

    public void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            AudioPool obj = Instantiate(poolPrefab, transform);
            obj.gameObject.SetActive(false);
            soundPool.Enqueue(obj);
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        if (soundPool.Count == 0)
        {
            AudioPool newObj = Instantiate(poolPrefab, transform);
            newObj.gameObject.SetActive(false);
            soundPool.Enqueue(newObj);
        }
        AudioPool obj = soundPool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.PlayeAudio(clip);
    }

    public void PoolReturn(AudioPool obj)
    {
        obj.gameObject.SetActive(false);
        soundPool.Enqueue(obj);
    }
}
