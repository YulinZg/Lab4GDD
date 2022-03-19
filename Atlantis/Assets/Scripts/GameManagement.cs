using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }
    public int buildingNumber { get; set; } = 6;

    private void Awake()
    {
        Instance = this;
  
    }
    public enum Stage
    {
        none,
        wait,
        smallWave,
        bigWave,
        relex
    }

    [Header("UI")]
    public GameObject introPanel;
    public GameObject losePanel;
    public Text intro;
    public Text howToPlay;
    public Text hint;
    public Text score;
    public Text gameLoopCounter;
    public Text stageIntro;
    public string showDialog;
    public string howToPlayDialog;

    private bool isFirstPage;
    private bool isTyping;

    [Header("GameFlow")]
    private int scoreCounter = 0;
    private int numOfTenThousand = 0;
    public Stage currentStage;
    private EnemyController enemyController;
    private float gameLoopTime;

    [Header("Building")]
    public GameObject turret;
    public GameObject[] buildings;
    public Queue<Vector3> buildingsPos = new Queue<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(gameLoop(3));
        
        Time.timeScale = 1;
        introPanel.SetActive(true);
        StartCoroutine(typeText(showDialog, 5f, 1));
        enemyController = gameObject.GetComponent<EnemyController>();
        stageIntro.enabled = false;
        gameLoopCounter.enabled = false;
        //foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
        //{
        //    buildings.Add(building);
        //}
        //foreach (var t in buildingsTransform)
        //{
        //    Debug.Log(t.position);
        //}

    }

    private IEnumerator typeText(string dialogText, float duration, int pageNum)
    {
        isTyping = true;
        isFirstPage = (pageNum == 1);

        if (isFirstPage)
        {
            foreach (char c in dialogText.ToCharArray())
            {
                intro.text += c;
                yield return new WaitForSeconds(duration / dialogText.ToCharArray().Length);
            }
            hint.text = "Press Enter To Continue";
        }
        else
        {
            foreach (char c in dialogText.ToCharArray())
            {
                howToPlay.text += c;
                yield return new WaitForSeconds(duration / dialogText.ToCharArray().Length);
            }
            hint.text = "Press Enter To Start";

        }   
        isTyping = false;

    }

    private void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.R))
        //    reBuild();
        if (Input.GetKeyDown(KeyCode.Return) && isTyping && isFirstPage)
        {
            StopAllCoroutines();
            intro.text = showDialog;
            hint.text = "Press Enter To Continue";
            isTyping = false;
            
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !isTyping && isFirstPage)
        {
            intro.enabled = false;
            hint.text = "Press Enter To Skip";
            StartCoroutine(typeText(howToPlayDialog, 5f, 2));
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isTyping && !isFirstPage)
        {
            StopAllCoroutines();
            howToPlay.text = howToPlayDialog;
            hint.text = "Press Enter To Start";
            isTyping = false;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !isTyping && !isFirstPage)
        {
            //enemyController.enabled = true;
            currentStage = Stage.wait;
            introPanel.SetActive(false);
        }
        gameLoopControl();
    }

    private void gameLoopControl()
    {
        switch (currentStage)
        {
            case Stage.wait:
                //enemyController.enabled = false;
                switchStage(3, Stage.smallWave, "Small wave of Enemy is coming", true);
                break;
            case Stage.smallWave:
                //enemyController.enabled = true;
                enemyController.intervalBasic = 3;
                switchStage(20, Stage.bigWave, "Big wave of Enemy is coming", true);
                break;
            case Stage.bigWave:
                //enemyController.enabled = true;
                enemyController.intervalBasic = 1;
                switchStage(20, Stage.relex, "Next round will rebuild your buildings (10000 score for one building)", false);
                break;
            case Stage.relex:
                while(numOfTenThousand > 0)
                {
                    reBuild();
                    numOfTenThousand--;
                }
                switchStage(8, Stage.smallWave, "Small wave of Enemy is coming", true);
                break;
            default:
                break;
        }
    }

    private void switchStage(int waitTime, Stage nextStage, string showText, bool nextStageEnemyControllerEnable)
    {
        gameLoopTime += Time.deltaTime;
        if(waitTime - Mathf.Floor(gameLoopTime) <= 3)
        {
            gameLoopCounter.enabled = true;
            enemyController.enabled = false;
            stageIntro.enabled = true;
            stageIntro.text = showText;
            gameLoopCounter.text = (waitTime - Mathf.Floor(gameLoopTime)).ToString();
        }
        if (gameLoopTime >= waitTime)
        {
            currentStage = nextStage;
            enemyController.enabled = nextStageEnemyControllerEnable;
            gameLoopCounter.enabled = false;
            stageIntro.enabled = false;
            gameLoopTime = 0;
        }

    }
    public void increaseScore(int point)
    {
        score.text = (int.Parse(score.text) + point).ToString();
        scoreCounter += point;
        if(scoreCounter >= 10000)
        {
            scoreCounter -= 10000;
            numOfTenThousand += 1;
        }
    }

    public int getScore()
    {
        return int.Parse(score.text);
    }
    public void reBuild()
    {
        if (!GameObject.FindGameObjectWithTag("Turret"))
            Instantiate<GameObject>(turret, new Vector3(0, -2, 0), Quaternion.identity).tag = "Turret";
        else if(GameObject.FindGameObjectWithTag("Turret") && buildingNumber < 6)
        {
            buildingNumber++;
            Vector3 temp = buildingsPos.Dequeue();
            if (temp.x == -6)
                Instantiate(buildings[0], temp, Quaternion.Euler(0, 0, 180f));
            else if (temp.x == -4)
                Instantiate(buildings[1], temp, Quaternion.Euler(0, 0, 180f));
            else if (temp.x == -2)
                Instantiate(buildings[2], temp, Quaternion.identity);
            else if (temp.x == 2)
                Instantiate(buildings[3], temp, Quaternion.identity);
            else if (temp.x == 4)
                Instantiate(buildings[4], temp, Quaternion.Euler(0, 0, 180f));
            else if (temp.x == 6)
                Instantiate(buildings[5], temp, Quaternion.Euler(0,0,180f));
        }
        
    }
    public void lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void quit()
    {
        Application.Quit();
        //Debug.Log(111);
    }
    public void restart()
    {
        SceneManager.LoadScene(0);
    }
}
