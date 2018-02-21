using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkControl : MonoBehaviour {

    private Rigidbody2D rb2d;
    public GameObject Firework;
    public float firerate = 2f;
    private bool fired = false;
    private float timeElapsed = 0f;

    //public float FIRE_INTERVAL = 1f;
    public float fireposition = 8f;
    private float LEFT_X = -10f;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
    }

    void Update() {
         
        if (GameControl.instance.gameOver)
        {
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            if (!fired)
            {
                if (transform.position.x <= fireposition && transform.position.x > LEFT_X)
                {
                    Instantiate(Firework, new Vector2(transform.position.x, -2), Quaternion.identity);
                    fired = true;
                }
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= firerate)
                {
                    fired = false;
                    timeElapsed = 0f;
                }
            }
        }
    }

}
