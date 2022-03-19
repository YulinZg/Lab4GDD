using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    public enum Stage
    {
        none,
        wait,
        smallWave,
        bigWave
    }

    [Header("UI")]
    public GameObject menuPanel;
    public GameObject introPanel;
    public GameObject losePanel;
    public GameObject scorePanel;
    public Text intro;
    public Text hint;
    public Text scoreText;
    public Text waveCounter;
    public Text stageIntro;
    public Text highestScore;
    public Text loseScore;
    public Text loseHighestScore;
    public string dialog;

    private bool isTyping;
    private bool isMenu = true;
    private bool isIntro = true;

    [Header("GameFlow")]
    public Stage currentStage;
    public float smallWaveTime;
    public float bigWaveTime;
    public int wave { get; private set; } = 1;
    public bool isLose { get; private set; } = false;
    public bool leftAlt { get; set; } = false;
    public bool rightAlt { get; set; } = false;

    private int score = 0;
    private int point = 0;
    private int rebuildChance = 0;
    private EnemyController enemyController;
    private float gameLoopTimer;
    private float scoreTimer;

    [Header("Building")]
    public GameObject turret;
    public GameObject[] buildings;
    public Stack<int> buildingNumbers = new Stack<int>();
    public int buildingCount { get; set; } = 6;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("score"))
            highestScore.text = "Highest Score: " + PlayerPrefs.GetInt("score");
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMenu && isIntro)
        {
            if (Input.GetKeyDown(KeyCode.Return) && isTyping)
            {
                StopAllCoroutines();
                intro.text = dialog;
                hint.text = "Press Enter To Continue";
                isTyping = false;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && !isTyping)
            {
                isIntro = false;
                introPanel.SetActive(false);
                scorePanel.SetActive(true);
                currentStage = Stage.wait;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && isMenu)
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.Q))
            Quit();
        if (!isLose && !isIntro)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1.0f)
            {
                IncreaseScore(50);
                scoreTimer = 0;
            }
            GameLoopControl();
        }
    }

    private IEnumerator TypeText(string text, float duration)
    {
        isTyping = true;
        foreach (char c in text.ToCharArray())
        {
            intro.text += c;
            yield return new WaitForSeconds(duration / text.ToCharArray().Length);
        }
        hint.text = "Press Enter To Continue";
        isTyping = false;
    }

    private void GameLoopControl()
    {
        switch (currentStage)
        {
            case Stage.wait:
                while (rebuildChance > 0)
                {
                    Rebuild();
                    rebuildChance--;
                }
                SwitchStage(3f, Stage.smallWave, true);
                break;
            case Stage.smallWave:
                enemyController.interval = 3;
                SwitchStage(smallWaveTime, Stage.bigWave, true);
                break;
            case Stage.bigWave:
                enemyController.interval = 1;
                SwitchStage(bigWaveTime, Stage.wait, false);
                break;
            default:
                break;
        }
    }

    private void SwitchStage(float stageTime, Stage nextStage, bool ifSpawnEnemyNextStage)
    {
        gameLoopTimer += Time.deltaTime;
        if(stageTime - gameLoopTimer <= 3f)
        {
            enemyController.enabled = false;
            if (currentStage == Stage.wait)
            {
                stageIntro.enabled = true;
                stageIntro.text = "Wave " + wave;
                waveCounter.enabled = true;
                waveCounter.text = (Mathf.Floor(stageTime - gameLoopTimer) + 1).ToString();
            }
            else if (currentStage == Stage.smallWave)
            {
                stageIntro.enabled = true;
                stageIntro.text = "Warning: A group of enemy is coming!";
            }
        }
        if (gameLoopTimer >= stageTime)
        {
            if (currentStage == Stage.wait)
                wave++;
            else if (currentStage == Stage.bigWave)
                IncreaseScore(1000 * buildingCount);
            currentStage = nextStage;
            enemyController.enabled = ifSpawnEnemyNextStage;
            waveCounter.enabled = false;
            stageIntro.enabled = false;
            gameLoopTimer = 0;
        }
    }

    public void IncreaseScore(int num)
    {
        score += num;
        point += num;
        scoreText.text = "Score: " + score;
        if(point >= 10000)
        {
            rebuildChance++;
            point -= 10000;
        }
    }

    public void Rebuild()
    {
        if (!GameObject.FindGameObjectWithTag("Turret"))
        {
            GameObject turrentInstance = Instantiate(turret, new Vector3(0, -2, 0), Quaternion.identity);
            turrentInstance.tag = "Turret";
            turrentInstance.GetComponent<Turret>().leftAlt = leftAlt;
            turrentInstance.GetComponent<Turret>().rightAlt = rightAlt;
        }
        else if(GameObject.FindGameObjectWithTag("Turret") && buildingCount < 6)
        {
            buildingCount++;
            GameObject building = buildings[buildingNumbers.Pop()];
            Instantiate(building, building.transform.position, building.transform.rotation);
        }
    }

    public void Lose()
    {
        isLose = true;
        enemyController.enabled = false;
        waveCounter.enabled = false;
        stageIntro.enabled = false;
        scorePanel.SetActive(false);
        losePanel.SetActive(true);
        SetHighestScore();
        loseScore.text = "Your Score is: " + score;
        loseHighestScore.text = "The Highest Score is: " + PlayerPrefs.GetInt("score");
    }

    public void StartGame()
    {
        isMenu = false;
        menuPanel.SetActive(false);
        introPanel.SetActive(true);
        StartCoroutine(TypeText(dialog, 5.0f));
    }

    public void Restart()
    {
        SetHighestScore();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        SetHighestScore();
        Application.Quit();
    }

    private void SetHighestScore()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            if (score > PlayerPrefs.GetInt("score"))
                PlayerPrefs.SetInt("score", score);
        }
        else
            PlayerPrefs.SetInt("score", score);
    }
}
