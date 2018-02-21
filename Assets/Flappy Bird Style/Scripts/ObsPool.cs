using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsPool : MonoBehaviour {

	/**
	 * [--Obstacle--]
	 * Tower			0
	 * Bat				1
	 * TowerWithBrick	2
	 * BatWithWave		3
     * TowerWFirework   4
	 * 
	 * [--Collective--]
	 * 9SquareStar		800
	 * 
	 * [--Fairy--]
	 * Heal				900
	 */

	public enum Obstacle {
		Tower, 
		Bat,
        TowerWithBrick,
		BatWithWave,
        TowerWithFirework
	};

	public enum Collective {
		Star = 800
	}

	public enum Fairy {
		Heal = 900
	}

    public GameObject towerPrefab;
    public GameObject batPrefab;
    public GameObject starPrefab;
	public GameObject towerWithBrickPrefab;
	public GameObject batWithWavePrefab;
	public GameObject fairyWithHealPrefab;
    public GameObject towerWithFireworkPrefab;

	public int type;

	private GameObject[] towers;
	private GameObject[] bats;
	private GameObject[] towerWithBricks;
	private GameObject[] batWithWaves;
    private GameObject[] towerWithFireworks;
	private GameObject[] stars;
	private GameObject fairyWithHeal;

	private Vector2 objectPoolPosition = new Vector2 (-15,-25);     //A holding position for our unused columns offscreen.
	private float spawnXPosition = 15f;

	//Main attributes.
	public int obsPoolSize = 5;
	public float spawnRate = 2f;
    public readonly int obsTypeCountTotal = 5;
	public readonly int colTypeCountTotal = 1;
	public readonly int fairyTypeCountTotal = 1;

	//Costum attributes.
    private float spawnRateMin = 1.5f;
	private float spawnRateMax = 4f;
    private float batYMin = -2f;
    private float batYMax = 1.5f;
	private float batWaveYMin = -3f;
	private float batWaveYMax = 3.9f;
	private float starYMin = -2f;
	private float starYMax = 1.5f;

	//DO NOT CHANGE. DEFAULT ATTRIBUTES.
	private float curScore = 0f;
	public int[] curObsLocArr;
	public int[] curColLocArr;
	public int[] curFairyLocArr;
	//public int[] typeToPoolSize
	public int obsTypeCountCur = 1;
    public float timeSinceLastSpawned;
	public float timeSinceLastHeal;

	public ArrayList curObsTypeList = new ArrayList();
	public bool needToHeal = false;

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

	void HealMechanism () {
		if (GameControl.instance.getHP () < 3) {
			fairyWithHeal.transform.position = new Vector2 (spawnXPosition, 0f);
		}
	}

    void Start() {
        timeSinceLastSpawned = 0f;
		timeSinceLastHeal = 0f;

		curObsLocArr = new int[obsTypeCountTotal];
		curColLocArr = new int[colTypeCountTotal];
		curFairyLocArr = new int[fairyTypeCountTotal];

		towers = new GameObject[obsPoolSize];
		bats = new GameObject[obsPoolSize];
		towerWithBricks = new GameObject[obsPoolSize];
		stars = new GameObject[obsPoolSize];
		batWithWaves = new GameObject[obsPoolSize];
        towerWithFireworks = new GameObject[obsPoolSize];

		GenerateObstacles (towerPrefab, towers);
		GenerateObstacles (batPrefab, bats);
		GenerateObstacles (towerWithBrickPrefab, towerWithBricks);
        GenerateObstacles (starPrefab, stars);
		GenerateObstacles (batWithWavePrefab, batWithWaves);
        GenerateObstacles (towerWithFireworkPrefab, towerWithFireworks);

		fairyWithHeal = (GameObject)Instantiate(fairyWithHealPrefab, objectPoolPosition, Quaternion.identity);
    }

    //This spawns columns as long as the game is not over.
    void Update() {
		if (needToHeal) {
			timeSinceLastHeal += Time.deltaTime;
		}

        timeSinceLastSpawned += Time.deltaTime;

		curScore = GameControl.instance.score;

		if (curScore == 0f) {
			obsTypeCountCur = 2;
			curObsTypeList.Add (Obstacle.Bat);
			curObsTypeList.Add (Obstacle.Tower);
		} else if (curScore == 15f) {
			curObsTypeList.Clear ();
			obsTypeCountCur = 1;
			curObsTypeList.Add (Obstacle.BatWithWave);
		} else if (curScore == 30f) {
			curObsTypeList.Clear ();
			obsTypeCountCur = 2;
			curObsTypeList.Add (Obstacle.Bat);
			curObsTypeList.Add (Obstacle.Tower);
		} else if (curScore == 45f) {
			curObsTypeList.Clear ();
			obsTypeCountCur = 5;
			curObsTypeList.Add (Obstacle.Tower);
			curObsTypeList.Add (Obstacle.Bat);
			curObsTypeList.Add (Collective.Star);
			curObsTypeList.Add (Fairy.Heal);
            curObsTypeList.Add (Obstacle.TowerWithFirework);
		} else if (curScore == 80f) {
			curObsTypeList.Clear ();
			obsTypeCountCur = 7;
			curObsTypeList.Add (Obstacle.Tower);
			curObsTypeList.Add (Obstacle.Bat);
			curObsTypeList.Add (Obstacle.TowerWithBrick);
			curObsTypeList.Add (Obstacle.BatWithWave);
			curObsTypeList.Add (Collective.Star);
			curObsTypeList.Add (Fairy.Heal);
            curObsTypeList.Add (Obstacle.TowerWithFirework);
		}

		// When detect hp lost, add FairyWithHeal into the pool.
		if (GameControl.instance.getHP () < 3 && !needToHeal) {
			needToHeal = true;
		}
		if (GameControl.instance.getHP () == 3 && needToHeal) {
			needToHeal = false;
		}

        if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate)  {
			
            timeSinceLastSpawned = 0f;
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);

			int typeIndex = Random.Range(0, obsTypeCountCur);
			if (curObsTypeList.Count == 0) {
				return;
			}
			type = (int)curObsTypeList [typeIndex];

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
				SetupObstacles (batWithWaves, new Vector2(spawnXPosition, Random.Range (batWaveYMin, batWaveYMax)), (int)Obstacle.BatWithWave);
				break;
            case (int)Obstacle.TowerWithFirework:
                SetupObstacles (towerWithFireworks, new Vector2(spawnXPosition, -3.2f), (int)Obstacle.TowerWithFirework);
                break;
			case (int)Collective.Star:
                SetupCollectives (stars, new Vector2(spawnXPosition, Random.Range(starYMin, starYMax)), (int)Collective.Star);
                break;
			case (int)Fairy.Heal:
				if (timeSinceLastHeal >= 30f && needToHeal) {
					fairyWithHeal.transform.position = new Vector2 (spawnXPosition, 0f);
					timeSinceLastHeal = 0f;
				} else {
					// When the FairyWithHeal doesn't need to show up, and index was randomed to be here, then, skip this heal and random again immediately.
					timeSinceLastSpawned = spawnRate; 
				}
				break;
			}
        }
    }
}
