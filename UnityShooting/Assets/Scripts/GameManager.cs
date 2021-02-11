using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;




public class GameManager : MonoBehaviour
{

    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    

    public string[] enemyObjs;
    public Transform[] spawnPoints;


    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;

    public Text scoreText;
    public Text finalScoreText;





    public Text[] scoreData;

    //public static int Score = 0;


   


    //private int savedScore = 0;
    //private string KeyString = "HighScore";



    public Image[] lifeImage;
    public Image[] boomImage;

    public GameObject gameOverSet;
    
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    


    void Awake()
    {
        //savedScore = PlayerPrefs.GetInt(KeyString, 0);
       // highscoreText.text = "High Score:" + savedScore.ToString("0");

        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        StageStart();
    }

    

    public void StageStart()
    {
        // Stage UI Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "STAGE " + stage;
        clearAnim.GetComponent<Text>().text = "STAGE " + stage + "\nCLEAR";

        // Enemy Spawn File Read
        ReadSpawnFile();

        // Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        // Clear UI Load
        clearAnim.SetTrigger("On");

        

        // Fade Out
        fadeAnim.SetTrigger("Out");

        // Player Repos
        player.transform.position = playerPos.position;

        // Stage Increament
        stage++;
        if (stage > 2)
            Invoke("GameOver", 6);

        
        else
            Invoke("StageStart", 5);
    }

    void ReadSpawnFile()  //딜레이,몹,위치(포인트)
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            // 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // 텍스트 파일 닫기
        stringReader.Close();

        // 첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;


    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();

           
            curSpawnDelay = 0;
        }

        // UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

       // scoreText.text = "Score:" + Score.ToString("0");

       // if (Score > savedScore)
       // {
        //    PlayerPrefs.SetInt(KeyString, Score);
       // }
    }

    //public void ScoreUp()
    //{
    //    Score++;
    //}


    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;

            case "M":
                enemyIndex = 1;
                break;

            case "L":
                enemyIndex = 2;
                break;

            case "B":
                enemyIndex = 3;
                break;
        }


        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);

        enemy.transform.position = spawnPoints[enemyPoint].position;

         


        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;


        if (enemyPoint == 5 || enemyPoint == 6)  // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }

        else if (enemyPoint == 7 || enemyPoint == 8) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);

            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else // Front Spawn
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));

        }

        // 리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        //  다음 리스폰 딜레이 갱신

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)  // UI Life Init Disable
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index<life; index++) // UI Life Active
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; index++)  // UI Boom Init Disable
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++) // UI Boom Active
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 1f);        
    }

    void RespawnPlayerExe()
    {      
        player.transform.position = Vector3.down * 4.1f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }


    

    

    public void Save()
    {
        Player playerLogic = player.GetComponent<Player>();

        if(playerLogic.score < PlayerPrefs.GetInt("BestScore"))
        {
            if (playerLogic.score < PlayerPrefs.GetInt("SecondScore"))
            {
                if (playerLogic.score < PlayerPrefs.GetInt("ThirdScore"))
                    return;

                PlayerPrefs.SetInt("ThirdScore", playerLogic.score);
                return;
            }
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetInt("SecondScore", playerLogic.score);
            return;
        }

        if (playerLogic.score == PlayerPrefs.GetInt("BestScore"))
            return;
        PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
        PlayerPrefs.SetInt("SecondScore", PlayerPrefs.GetInt("BestScore"));
        PlayerPrefs.SetInt("BestScore", playerLogic.score);
    }

    public void GameOver()
    {
        Save();
        Load();

        scoreText.enabled = false;
        Player playerLogic = player.GetComponent<Player>();
        finalScoreText.text = "Score:" + playerLogic.score.ToString();

        if (playerLogic.score == PlayerPrefs.GetInt("BestScore")) ;

        
        gameOverSet.SetActive(true);


    }

    public void Load()
    {
       

        scoreData[0].text = PlayerPrefs.GetInt("BestScore").ToString() + "점";
        scoreData[1].text = PlayerPrefs.GetInt("SecondScore").ToString() + "점";
        scoreData[2].text = PlayerPrefs.GetInt("ThirdScore").ToString() + "점";

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

   

}