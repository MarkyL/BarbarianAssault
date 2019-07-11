using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmt;
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private Animator anim;
    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private float navigationTime = 0;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        GameManaging.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (wayPoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                //UnityEngine.Debug.Log("0=" + wayPoints[0].transform.position);
                
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, 0.8f * navigationTime);
                }
                else
                {
                    // next stop is exitPoint.
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, 0.8f * navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //UnityEngine.Debug.Log("onTriggerEnter2D");
        if (other.tag == "Checkpoint") {
            target += 1;
        }
        else if (other.tag == "Finish")
        {
            GameManaging.Instance.TotalEscaped++;
            GameManaging.Instance.RoundEscaped++;
            GameManaging.Instance.UnRegister(this);
        } else if (other.tag == "Projectile")
        {
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            if (newP != null)
            {
                enemyHit(newP.AttackStrength);
                Destroy(other.gameObject);
            }
        }
    }

    public void enemyHit(int hitPoints)
    {
        if (healthPoints - hitPoints > 0)
        {
            anim.Play("Hurt");
            healthPoints -= hitPoints;
            GameManaging.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
        }
        else
        {
            die();
        }
    }

    public void die()
    {
        isDead = true;
        anim.SetTrigger("didDie");
        GameManaging.Instance.TotalKilled++;
        GameManaging.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Die);
        enemyCollider.enabled = false;
        GameManaging.Instance.addMoney(rewardAmt);
        GameManaging.Instance.isWaveOver();
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
}
