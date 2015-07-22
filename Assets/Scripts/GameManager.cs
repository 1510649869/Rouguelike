using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {


    public float turnDely = .1f;
    public float levelStartDely = 2f;
    public static GameManager instance = null;
    public BoardManager boardScripts;
    public int playerFoodPoints = 100;
    //[HideInInspector]
    public bool playerTurn = true;

    private int level = 1;
    private List<Enemy> enemies;
    private bool emeiesMoving;
    private bool doingSetup;

    private Text levelText;
    private GameObject levelImage;


    void Awake()
    {
        if(instance==null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);	
        enemies = new List<Enemy>();
        DontDestroyOnLoad(gameObject);
        boardScripts = GetComponent<BoardManager>();
        InitGame();
    }


    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("levelImage");
        levelText = GameObject.Find("levelText").GetComponent<Text>();
        levelText.text = "Day  " + level;
        Debug.Log(level);
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDely);
        enemies.Clear();
        boardScripts.SetupScene(level);
    }

    public void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    public void GameOver()
    {
        levelText.text = "After" + level + "days,you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

	void Update () {
	    if(emeiesMoving||playerTurn||doingSetup)
        {
            return;
        }
        StartCoroutine(EnemysMove());
	}
    public void AddEnemyToList(Enemy scripts)
    {
        enemies.Add(scripts);
    }
    IEnumerator EnemysMove()
    {
        emeiesMoving = true;
        yield return new WaitForSeconds(turnDely);
        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(turnDely);
        }
        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].MoveTime);
        }
        playerTurn = true;
        emeiesMoving = false;
    }
}
