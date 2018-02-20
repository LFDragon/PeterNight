using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickControl : MonoBehaviour {

    private GameObject[] brick;
    private Rigidbody2D rb2d;
    private Animator anim;

	public readonly float FALL_INTERVAL = 0.8f;
	private readonly int MAX_INDEX_WAVE = 3;
	private readonly int FRAME_X_LEFT = -12;
	private readonly int FRAME_X_RIGHT = 8;
	private readonly float GRAVITY_SCALE_BRICK = 0.5f;
	private readonly float BRICK_LEFT_FORCE = -200f;
    private float timeElaspe = 0f;
    private bool shouldTrigger = true;
    private int curIndex = 0;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = transform.Find("tower").gameObject.GetComponent<Animator>();
		brick = new GameObject[MAX_INDEX_WAVE];
        brick[0] = transform.Find("b1").gameObject;
        brick[1] = transform.Find("b2").gameObject;
        brick[2] = transform.Find("b3").gameObject;

        rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
    }

    void Update() {
		if (transform.position.x < FRAME_X_RIGHT && transform.position.x > FRAME_X_LEFT) {
            if (shouldTrigger) {
                anim.SetTrigger("Shrink");
                shouldTrigger = false;
            } timeElaspe += Time.deltaTime;
			if (timeElaspe >= FALL_INTERVAL && curIndex < MAX_INDEX_WAVE) {
                brick[curIndex].SetActive(true);
                brick[curIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
				brick[curIndex].GetComponent<Rigidbody2D>().gravityScale = GRAVITY_SCALE_BRICK;
				brick[curIndex].GetComponent<Rigidbody2D>().AddForce(new Vector2(BRICK_LEFT_FORCE,0f));

                curIndex++;
                timeElaspe = 0f;
            }
		} else if (transform.position.x < FRAME_X_LEFT) {
            if (!shouldTrigger) {
                anim.SetTrigger("Still");
                shouldTrigger = true;
                curIndex = 0;
				for (int i = 0; i < MAX_INDEX_WAVE; i++) {
                    brick[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    brick[i].transform.position = new Vector2(rb2d.transform.position.x, 2f);
                    brick[i].SetActive(false);
                }
            }
        } if (GameControl.instance.gameOver == true) {
            rb2d.velocity = Vector2.zero;
        }
    }
}
