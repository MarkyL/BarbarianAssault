using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameStatus
{
    next, play, gameover, win
}

public class GameManaging : Singleton<GameManaging> {
    const float spawnDelay = 1f;
    const int STARTING_MONEY = 10;

    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
    [SerializeField]
    private Text totalMoneyLabel;
    [SerializeField]
    private Image GameStatusImage;
    [SerializeField]
    private Text nextWaveBtnLabel;
    [SerializeField]
    private Text escapedLabel;
    [SerializeField]
    private Text waveLabel;
    [SerializeField]
    private Text GameStatusLabel;
    [SerializeField]
    private Button GameStatusBtn;
    [SerializeField]
    private int waveNumber = 0;

    [SerializeField]
    private Text playBtnLbl;
    [SerializeField]
    private Button playBtn;

    private int totalMoney = STARTING_MONEY;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int enemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;
    private AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    void Start()
    {
        playBtn.gameObject.SetActive(false);
        //GameStatusBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();
        playBtnPressed();
    }

    void Update()
    {
        handleEscape();
    }

    IEnumerator spawn()
    {
        if (totalEnemies > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < totalEnemies; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    //Debug.Log("i = " + i + ", EnemyList.Count = " + EnemyList.Count + ", maxEnemiesOnScreen = " + totalEnemies
                    //    + ", totalEnemies = " + totalEnemies);

                    Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
                yield return new WaitForSeconds(spawnDelay);
            }
            
            StartCoroutine(spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnRegister(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
        isWaveOver();
    }

    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void isWaveOver()
    {
        escapedLabel.text = "Escaped " + TotalEscaped + "/10";
        //Debug.Log("roundEscaped = " + roundEscaped + ", TotalKilled = " + TotalKilled + ", totalEnemies = " + totalEnemies);
        if ((roundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    private void handleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        } else if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("onF1Pressed");
            SceneManager.LoadScene("Menu");
        }
        
    }

    public void showMenu()
    {

        switch (currentState)
        {
            case gameStatus.gameover:
                GameStatusBtn.gameObject.SetActive(true);
                GameStatusLabel.text = "Gameover";
                audioSource.PlayOneShot(SoundManager.Instance.Gameover);
                playBtnLbl.text = "Play again";
                playBtn.gameObject.SetActive(true);
                break;
            case gameStatus.next:
                playBtnLbl.text = "Continue";
                GameStatusBtn.gameObject.SetActive(true);
                GameStatusLabel.text = "Wave " + (waveNumber + 2) + " next.";
                StartCoroutine(hideGameStatusBtn());
                break;
            case gameStatus.play:
                playBtnLbl.text = "Play";
                playBtn.gameObject.SetActive(true);
                break;
            case gameStatus.win:
                playBtnLbl.text = "Play";
                GameStatusBtn.gameObject.SetActive(true);
                GameStatusLabel.text = "You Won!";
                playBtn.gameObject.SetActive(true);
                break;
        }
        
    }

    IEnumerator hideGameStatusBtn()
    {
        yield return new WaitForSeconds(3);

        GameStatusBtn.gameObject.SetActive(false);
        playBtnPressed();
    }

    public void playBtnPressed()
    {
        //Debug.Log("playBtnPressed, gameStatus = " + currentState);
        GameStatusBtn.gameObject.SetActive(false);
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                waveNumber = 0;
                enemiesToSpawn = 0;
                TotalMoney = STARTING_MONEY;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLabel.text = TotalMoney.ToString();
                escapedLabel.text = "Escaped " + TotalEscaped + "/10";
                GameStatusBtn.gameObject.SetActive(false);
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        roundEscaped = 0;
        waveLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(spawn());
        playBtn.gameObject.SetActive(false);
    }

    public gameStatus CurrentState
    {
        get
        {
            return currentState;
        }
    }
    public int WaveNumber
    {
        get
        {
            return waveNumber;
        }
        set
        {
            waveNumber = value;
        }
    }
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }
}
