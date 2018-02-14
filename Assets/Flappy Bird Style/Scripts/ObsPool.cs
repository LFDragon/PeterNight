using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsPool : MonoBehaviour {

	public enum Obstacle {
		Tower, 
		Bat,
        Star
	};

    public GameObject towerPrefab;
    public GameObject batPrefab;
    public GameObject starPrefab;
	public int obsPoolSize = 5;
	public float spawnRate = 2f;

    private float spawnRateMin = 1.5f;
	private float spawnRateMax = 4f;
    private float batYMin = -2f;
    private float batYMax = 1.5f;
	private float curScore = 0f;
    private float starYmin = -2f;
    private float starYmax = 1.5f;

    private GameObject[] towers;
    private GameObject[] bats;
    private GameObject[] stars;

    public readonly int obsTypeCountTotal = 3;
	public int[] curLocArr;
	public int obsTypeCountCur = 1;

    private Vector2 objectPoolPosition = new Vector2 (-15,-25);     //A holding position for our unused columns offscreen.
    private float spawnXPosition = 15f;

    private float timeSinceLastSpawned;

	private ArrayList curObsTypeArr = new ArrayList();

	void GenerateObstacles(GameObject obsObj, GameObject[] obsObjArr) {
		for (int i = 0; i < obsPoolSize; i++) {
			obsObjArr[i] = (GameObject)Instantiate(obsObj, objectPoolPosition, Quaternion.identity);
		}
	}

	void SetupObstacles(GameObject[] obsObjArr, Vector2 verAttr, int curObsTypeIndex) {
		int currentLocation = curLocArr [curObsTypeIndex];
		obsObjArr [currentLocation].transform.position = verAttr;
		currentLocation++;
		if (currentLocation >= obsPoolSize) {
			currentLocation = 0;
		}
		curLocArr [curObsTypeIndex] = currentLocation;
	}

    void Start() {
        timeSinceLastSpawned = 0f;

		curLocArr = new int[obsTypeCountTotal];

        towers = new GameObject[obsPoolSize];
        bats = new GameObject[obsPoolSize];
        stars = new GameObject[obsPoolSize];

		GenerateObstacles(towerPrefab, towers);
		GenerateObstacles(batPrefab, bats);
        GenerateObstacles(starPrefab, stars);
    }

    //This spawns columns as long as the game is not over.
    void Update() {
        timeSinceLastSpawned += Time.deltaTime;

		curScore = GameControl.instance.score;

		if (curScore <= 15f) {
			obsTypeCountCur = 1;
            curObsTypeArr.Add (Obstacle.Star);
		} else if (curScore <= 35f) {
			obsTypeCountCur = 1;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Obstacle.Bat);
		} else {
			obsTypeCountCur = 2;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Obstacle.Tower);
			curObsTypeArr.Add (Obstacle.Bat);
		}

        if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate)  {
			
            timeSinceLastSpawned = 0f;
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);

			int typeIndex = Random.Range(0, obsTypeCountCur);
			int type = (int)curObsTypeArr [typeIndex];

			switch (type) {
			case (int)Obstacle.Tower:
				SetupObstacles (towers, new Vector2 (spawnXPosition, 0f), (int)Obstacle.Tower);
				break;
			case (int)Obstacle.Bat: 
				SetupObstacles (bats, new Vector2 (spawnXPosition, Random.Range (batYMin, batYMax)), (int)Obstacle.Bat);
				break;
                case (int)Obstacle.Star:
                    SetupObstacles(stars, new Vector2(spawnXPosition, Random.Range(starYmin, starYmax)), (int)Obstacle.Star);
                    break;
			}
        }
    }
}
