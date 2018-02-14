using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsPool : MonoBehaviour {

	public enum Obstacle {
		Tower, 
		Bat,
        TowerWithBrick,
		BatWithWave
	};

	public enum Collective {
		Star = 800
	}

    public GameObject towerPrefab;
    public GameObject batPrefab;
    public GameObject starPrefab;
	public GameObject towerWithBrickPrefab;
	public GameObject batWithWavePrefab;

	private GameObject[] towers;
	private GameObject[] bats;
	private GameObject[] towerWithBricks;
	private GameObject[] batWithWaves;
	private GameObject[] stars;

	private Vector2 objectPoolPosition = new Vector2 (-15,-25);     //A holding position for our unused columns offscreen.
	private float spawnXPosition = 15f;

	//Main attributes.
	public int obsPoolSize = 5;
	public float spawnRate = 2f;
	public readonly int obsTypeCountTotal = 4;
	public readonly int colTypeCountTotal = 1;

	//Costum attributes.
    private float spawnRateMin = 1.5f;
	private float spawnRateMax = 4f;
    private float batYMin = -2f;
    private float batYMax = 1.5f;
	private float starYMin = -2f;
	private float starYMax = 1.5f;

	//DO NOT CHANGE. DEFAULT ATTRIBUTES.
	private float curScore = 0f;
	public int[] curObsLocArr;
	public int[] curColLocArr;
	//public int[] typeToPoolSize
	public int obsTypeCountCur = 1;
    private float timeSinceLastSpawned;
	private ArrayList curObsTypeArr = new ArrayList();

	void GenerateObstacles(GameObject obsObj, GameObject[] obsObjArr) {
		for (int i = 0; i < obsPoolSize; i++) {
			obsObjArr[i] = (GameObject)Instantiate(obsObj, objectPoolPosition, Quaternion.identity);
		}
	}

	void SetupObstacles(GameObject[] obsObjArr, Vector2 verAttr, int curObsTypeIndex) {
		int currentLocation = curObsLocArr [curObsTypeIndex];
		obsObjArr [currentLocation].transform.position = verAttr;
		currentLocation++;
		if (currentLocation >= obsPoolSize) {
			currentLocation = 0;
		}
		curObsLocArr [curObsTypeIndex] = currentLocation;
	}

	void SetupCollectives(GameObject[] colObjArr, Vector2 verAttr, int curColTypeIndex) {
		curColTypeIndex -= 800;
		int currentLocation = curColLocArr [curColTypeIndex];
		colObjArr [currentLocation].transform.position = verAttr;
		currentLocation++;
		if (currentLocation >= obsPoolSize) {
			currentLocation = 0;
		}
		curColLocArr [curColTypeIndex] = currentLocation;
	}

    void Start() {
        timeSinceLastSpawned = 0f;

		curObsLocArr = new int[obsTypeCountTotal];
		curColLocArr = new int[colTypeCountTotal];

		towers = new GameObject[obsPoolSize];
		bats = new GameObject[obsPoolSize];
		towerWithBricks = new GameObject[obsPoolSize];
		stars = new GameObject[obsPoolSize];
		batWithWaves = new GameObject[obsPoolSize];

		GenerateObstacles (towerPrefab, towers);
		GenerateObstacles (batPrefab, bats);
		GenerateObstacles (towerWithBrickPrefab, towerWithBricks);
        GenerateObstacles (starPrefab, stars);
		GenerateObstacles (batWithWavePrefab, batWithWaves);
    }

    //This spawns columns as long as the game is not over.
    void Update() {
        timeSinceLastSpawned += Time.deltaTime;

		curScore = GameControl.instance.score;

		if (curScore <= 15f) {
			obsTypeCountCur = 1;
			curObsTypeArr.Add (Obstacle.BatWithWave);
		} else if (curScore <= 30f) {
			obsTypeCountCur = 1;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Obstacle.Bat);
		} else if (curScore <= 45f) {
			obsTypeCountCur = 1;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Obstacle.TowerWithBrick);
		} else if (curScore <= 60f) {
			obsTypeCountCur = 1;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Collective.Star);
		} else {
			obsTypeCountCur = 4;
			curObsTypeArr.Clear ();
			curObsTypeArr.Add (Obstacle.Tower);
			curObsTypeArr.Add (Obstacle.Bat);
			curObsTypeArr.Add (Obstacle.TowerWithBrick);
			curObsTypeArr.Add (Collective.Star);
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
			case (int)Obstacle.TowerWithBrick:
				SetupObstacles (towerWithBricks, new Vector2(spawnXPosition, 0f), (int)Obstacle.TowerWithBrick);
				break;
			case (int)Obstacle.BatWithWave:
				SetupObstacles (batWithWaves, new Vector2(spawnXPosition, Random.Range (batYMin, batYMax)), (int)Obstacle.BatWithWave);
				break;
			case (int)Collective.Star:
                SetupCollectives (stars, new Vector2(spawnXPosition, Random.Range(starYMin, starYMax)), (int)Collective.Star);
                break;
			}
        }
    }
}
