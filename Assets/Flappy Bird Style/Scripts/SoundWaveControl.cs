using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveControl : MonoBehaviour {

	private GameObject[] waves;
	private Rigidbody2D rb2d;
	private float timeElaspe = 0f;
	private int curIndex = 0;
	private readonly int FRAME_X = 12;
	private readonly float WAVE_TO_BAT_X = 1.7f;
	private readonly int MAX_INDEX_WAVE = 2;
	private readonly float WAVE_LEFT_FORCE = -200f;

	public float shootInterval = 0.8f;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>(); // BatWithWave's Rigidbody2D.
		waves = new GameObject[MAX_INDEX_WAVE];
		waves[0] = transform.Find("Soundwave1").gameObject;
		waves[1] = transform.Find("Soundwave2").gameObject;

		rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0); // Make speed of BatWithWave to be the same as background.
	}

	void Update () {
		if (transform.position.x < FRAME_X && transform.position.x > -FRAME_X) {
			// When in the frame scene.
			timeElaspe += Time.deltaTime;
			if (timeElaspe >= shootInterval && curIndex < MAX_INDEX_WAVE + 1) {
				waves [curIndex].SetActive (true);
				waves [curIndex].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
				waves [curIndex].GetComponent<Rigidbody2D> ().AddForce (new Vector2 (WAVE_LEFT_FORCE, 0)); // Make speed of wave faster than background. And left-ward.
				waves [curIndex].GetComponent<Rigidbody2D> ().gravityScale = 0f; // No gravity.
				curIndex++;
				if (curIndex == MAX_INDEX_WAVE) {
					curIndex = 0;
				}
				timeElaspe = 0f;
			}
		} else if (transform.position.x < -12) {
			// When move out of the scene.
			for (int i = 0; i < MAX_INDEX_WAVE; i++) {
				waves[i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
				waves [curIndex].SetActive (false);
				waves[i].transform.position = new Vector2 (rb2d.transform.position.x - WAVE_TO_BAT_X, rb2d.transform.position.y);
			}
		} if (GameControl.instance.gameOver == true) {
			// When game is over.
			rb2d.velocity = Vector2.zero;
			for (int i = 0; i < MAX_INDEX_WAVE; i++) {
				waves[i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
				waves[i].transform.position = new Vector2 (rb2d.transform.position.x - WAVE_TO_BAT_X, rb2d.transform.position.y);
				waves[i].SetActive(false);
			}
		}
	}
}
