using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsPool : MonoBehaviour {

    public GameObject towerPrefab;                                    //The column game object.
    public GameObject batPrefab;
    public int obsPoolSize = 5;                                  //How many columns to keep on standby.
    public float spawnRatemin = 1f;                                    //How quickly columns spawn.
    public float spawnRatemax = 5f;
    private float spawnRate = 2f;
    public float batYmin = -2f;
    public float batYmax = 1.5f;

    private GameObject[] towers;                                   //tower pool
    private GameObject[] bats;
    private ArrayList obsList = new ArrayList();
    private int currentTow = 0;                                  //Index of the current column in the collection.
    private int currentBat = 0;
    public int kindcount = 2;

    private Vector2 objectPoolPosition = new Vector2 (-15,-25);     //A holding position for our unused columns offscreen.
    private float spawnXPosition = 15f;

    private float timeSinceLastSpawned;


    void Start()
    {
        timeSinceLastSpawned = 0f;

        //Initialize the columns collection.
        //obstacles = new GameObject[obsPoolSize];
        //Loop through the collection... 
//        for(int i = 0; i < obsPoolSize; i++)
//        {
//            //...and create the individual columns.
//            //if(i<obsPoolSize/2) obstacles[i] = (GameObject)Instantiate(towerPrefab, objectPoolPosition, Quaternion.identity);
//            //else (GameObject)Instantiate(batPrefab, objectPoolPosition, Quaternion.identity);
//            if(i<obsPoolSize/2) obsList.Add((GameObject)Instantiate(towerPrefab, objectPoolPosition, Quaternion.identity));
//            else obsList.Add((GameObject)Instantiate(batPrefab, objectPoolPosition, Quaternion.identity));
//        }

        towers = new GameObject[obsPoolSize];
        bats = new GameObject[obsPoolSize];

        for (int i = 0; i < obsPoolSize; i++)
        {
            towers[i] = (GameObject)Instantiate(towerPrefab, objectPoolPosition, Quaternion.identity);
        }
        for (int i = 0; i < obsPoolSize; i++)
        {
            bats[i] = (GameObject)Instantiate(batPrefab, objectPoolPosition, Quaternion.identity);
        }
    }


    //This spawns columns as long as the game is not over.
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate) 
        {   
            timeSinceLastSpawned = 0f;
            spawnRate = Random.Range(spawnRatemin, spawnRatemax);

            int kind = Random.Range(0, kindcount);

            //...then set the current column to that position.
            //((GameObject)obsList[currentObs]).transform.position = new Vector2(spawnXPosition, 0f);
            if (kind == 0)
            {
                towers[currentTow].transform.position = new Vector2(spawnXPosition, 0f);
                currentTow++;
                if (currentTow >= obsPoolSize)
                    currentTow = 0;
            }
            else if (kind == 1)
            {
                bats[currentBat].transform.position = new Vector2(spawnXPosition, Random.Range(batYmin,batYmax));
                currentBat++;
                if (currentBat >= obsPoolSize)
                    currentBat = 0;
            }

            //Increase the value of currentColumn. If the new size is too big, set it back to zero

//            if (currentObs >= obsPoolSize) 
//            {
//                currentObs = 0;
//            }
        }
    }
}
