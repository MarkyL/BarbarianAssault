using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioClip rock;
    [SerializeField]
    private AudioClip arrow;
    [SerializeField]
    private AudioClip fireball;
    [SerializeField]
    private AudioClip die;
    [SerializeField]
    private AudioClip newGame;
    [SerializeField]
    private AudioClip gameover;
    [SerializeField]
    private AudioClip buildTower;
    [SerializeField]
    private AudioClip hit;

    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }

    public AudioClip Fireball
    {
        get
        {
            return fireball;
        }
    }

    public AudioClip Die
    {
        get
        {
            return die;
        }
    }

    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }
    }

    public AudioClip Gameover
    {
        get
        {
            return gameover;
        }
    }

    public AudioClip BuildTower
    {
        get
        {
            return buildTower;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
}
