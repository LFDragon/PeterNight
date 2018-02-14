using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveControl : MonoBehaviour {

	private GameObject[] waves;
	private Rigidbody2D rb2d;

	public float shootInterval = 0.8f;
	private float timeElaspe = 0f;
	private int curIndex = 0;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		waves = new GameObject[2];
		waves[0] = transform.Find("Soundwave1").gameObject;
		waves[1] = transform.Find("Soundwave2").gameObject;

		rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < 12 && transform.position.x > -12) {
			timeElaspe += Time.deltaTime;
			if (timeElaspe >= shootInterval && curIndex < 3) {
				waves [curIndex].SetActive (true);
				waves [curIndex].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
				waves [curIndex].GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-200f, 0));
				waves [curIndex].GetComponent<Rigidbody2D> ().gravityScale = 0f;
				curIndex++;
				if (curIndex == 2) {
					curIndex = 0;
				}
				timeElaspe = 0f;
			}
		} else if (transform.position.x < -12) {

				for (int i = 0; i < 2; i++) {
					waves[i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
					waves[i].transform.position = new Vector2 (rb2d.transform.position.x - 1.7f, rb2d.transform.position.y);

			}
		}

		// If the game is over, stop scrolling.
		if (GameControl.instance.gameOver == true)
		{
			rb2d.velocity = Vector2.zero;
			for (int i = 0; i < 2; i++) {
				waves[i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
				waves[i].transform.position = new Vector2 (rb2d.transform.position.x - 1.7f, rb2d.transform.position.y);
				waves[i].SetActive(false);
			}
		}
	}
}
