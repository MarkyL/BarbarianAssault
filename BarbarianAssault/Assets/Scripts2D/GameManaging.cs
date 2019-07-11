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
    private int totalWaves = 1;
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

    private int maxAllowedEscaped = 3;

    public List<Enemy> EnemyList = new List<Enemy>();

    void Start()
    {
        init();
    }

    public void init()
    {
        Debug.Log("init EnemyList size = " + EnemyList.Count);
        playBtn.gameObject.SetActive(false);
        //GameStatusBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        playBtnPressed(false);
    }

    void Update()
    {
        handleEscape();
    }

    IEnumerator spawn()
    {
        if (currentState != gameStatus.gameover)
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
    }

    public void RegisterEnemy(Enemy enemy)
    {
       
        Debug.Log("Adding an enemy, list size before: "+EnemyList.Count);
        EnemyList.Add(enemy);
        Debug.Log("Added an enemy, list size after: " + EnemyList.Count);
    }

    public void UnRegister(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
        Debug.Log("UnRegister");
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
        escapedLabel.text = "Escaped " + TotalEscaped + "/" + maxAllowedEscaped;
        //Debug.Log("roundEscaped = " + roundEscaped + ", TotalKilled = " + TotalKilled + ", totalEnemies = " + totalEnemies);
        if ((roundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            Debug.Log("isWaveOver");
            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState()
    {
        if (TotalEscaped >= maxAllowedEscaped)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            Debug.Log("waveNumber >= totalWaves");
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void DestroyAllEnemies()
    {
        Debug.Log("DestroyAllEnemies - " + EnemyList.Count);
        foreach (Enemy enemy in EnemyList)
        {
            Debug.Log("destory enemy");
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    private void handleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentState = gameStatus.gameover;
            playBtnPressed(true);
            StartCoroutine(unloadSceneWithData(false));
        }

    }

    IEnumerator unloadSceneWithData(bool hasWon)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync("Level1");
        GameManager.instance.freezeEnemies(false);
        GameManager.instance.toggleSceneVisibilityState(true);

        Debug.Log("unloadSceneWithData - " + hasWon);
        if (hasWon)
        {
            GameManager.instance.HasWon2D = hasWon;
        }
        yield return null;
    }

    public void showMenu()
    {
        Debug.Log("showMenu");
        switch (currentState)
        {
            case gameStatus.gameover:
                GameStatusBtn.gameObject.SetActive(true);
                GameStatusLabel.text = "Gameover";
                audioSource.PlayOneShot(SoundManager.Instance.Gameover);
                playBtnLbl.text = "Try again";
                playBtn.gameObject.SetActive(true);
                //unloadSceneWithData(false);
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
                GameStatusLabel.text = "You Won!";
                GameStatusBtn.gameObject.SetActive(true);
                playBtnLbl.text = "Back to game";
                playBtn.gameObject.SetActive(true);
                //unloadSceneWithData(true);
                break;
        }
        
    }

    IEnumerator hideGameStatusBtn()
    {
        yield return new WaitForSeconds(3);

        GameStatusBtn.gameObject.SetActive(false);
        playBtnPressed(false);
    }

    public void playBtnPressed(bool userClickedEscape)
    {
        GameStatusBtn.gameObject.SetActive(false);
        Debug.Log("playBtnPressed, currentState = " + currentState);
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            case gameStatus.gameover:
                totalEnemies = 3;
                TotalEscaped = 0;
                waveNumber = 0;
                enemiesToSpawn = 0;
                TotalMoney = STARTING_MONEY;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLabel.text = TotalMoney.ToString();
                escapedLabel.text = "Escaped " + TotalEscaped + "/" + maxAllowedEscaped;
                GameStatusBtn.gameObject.SetActive(false);
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);

                if(!userClickedEscape)
                {
                    currentState = gameStatus.play;
                }
                break;
            case gameStatus.win:
                Debug.Log("win");
                TowerManager.Instance.DestroyAllTowers();
                StartCoroutine(unloadSceneWithData(true));
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        roundEscaped = 0;
        Debug.Log("wave number = " + waveNumber);
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
