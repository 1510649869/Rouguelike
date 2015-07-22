using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count
    {
        public int maxmum;
        public int minmum;
        public Count(int min,int max)
        {
            minmum = min;
            maxmum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerwalltiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {

        gridPositions.Clear();
        for(int x=1;x<columns-1;x++)
        {
            for(int y=1;y<rows-1;y++)
            {
                gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }
    void BoardStup()
    {
        boardHolder = new GameObject("Board").transform;
        for(int x=-1;x<columns+1;x++)
        {
            for(int y=-1;y<rows+1;y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x==-1||x==columns||y==-1||y==rows)
                {
                    toInstantiate = outerwalltiles[Random.Range(0, outerwalltiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate,new Vector3(x,y),Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }
    Vector3 RandomPostion()
    {
        int randomIndex = Random.Range(0,gridPositions.Count);
        Vector3 randomPostion=gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);//防止重复生成
        return randomPostion;
    }

    void LayoytObjectAtRandom(GameObject[] tileArray,int minmum,int maxnum)
    {
        int objectCount = Random.Range(minmum, maxnum + 1);
            for(int i=0;i<objectCount;i++)
            {
                Vector3 postion=RandomPostion();
                GameObject tileChoice=tileArray[Random.Range(0,tileArray.Length)];
                Instantiate(tileChoice, postion, Quaternion.identity);
            }
    }

    public void SetupScene(int level)
    {
        BoardStup();
        InitialiseList();
        LayoytObjectAtRandom(wallTiles, wallCount.minmum, wallCount.maxmum);
        LayoytObjectAtRandom(foodTiles, foodCount.minmum, foodCount.maxmum);
        int enemyCount = (int)Mathf.Log(level,2f);
        LayoytObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit,new Vector3(columns-1,rows-1,0f),Quaternion.identity);
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
