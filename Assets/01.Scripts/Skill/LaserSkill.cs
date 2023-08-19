using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] private AudioClip laserStartAudioClip;
    [SerializeField] private AudioClip laserBoomAudioClip;

    public void Set(Vector3 startPos, Vector3 endPos)
    {
        AudioManager.Instance.PlayAudio(laserStartAudioClip);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => AudioManager.Instance.PlayAudio(laserBoomAudioClip));
        seq.AppendInterval(0.4f);
        seq.Append(DOTween.To(() => lineRenderer.endWidth, x => lineRenderer.endWidth = x, 0, 0.2f));
        seq.AppendCallback(() => Destroy(gameObject));
    }
}
