using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip idleClip;
    [SerializeField]
    private AudioClip walkClip;
    [SerializeField]
    private AudioClip runClip;
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private AudioClip dieClip;

    public AudioClip IdleClip => idleClip;
    public AudioClip WalkClip => walkClip;
    public AudioClip RunClip => runClip;
    public AudioClip AttackClip => attackClip;
    public AudioClip HitClip => hitClip;
    public AudioClip DieClip => dieClip;
}
