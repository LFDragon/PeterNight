using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickControl : MonoBehaviour {

    private GameObject[] brick;
    private Rigidbody2D rb2d;
    private Animator anim;

    public float fallinterval = 0.8f;
    private float timeElaspe = 0f;
    private bool shouldTrigger = true;
    private int curIndex = 0;

    // Use this for initialization
    void Start () 
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = transform.Find("tower").gameObject.GetComponent<Animator>();
        brick = new GameObject[3];
        brick[0] = transform.Find("b1").gameObject;
        brick[1] = transform.Find("b2").gameObject;
        brick[2] = transform.Find("b3").gameObject;

        //Start the object moving.
        rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
    }

    void Update()
    {
        if (transform.position.x < 8 && transform.position.x > -12)
        {
            if (shouldTrigger)
            {
                anim.SetTrigger("Shrink");
                shouldTrigger = false;
            }
            timeElaspe += Time.deltaTime;
            if (timeElaspe >= fallinterval && curIndex < 3)
            {
                brick[curIndex].SetActive(true);
                brick[curIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brick[curIndex].GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                brick[curIndex].GetComponent<Rigidbody2D>().AddForce(new Vector2(-200f,0f));

                curIndex++;
                timeElaspe = 0f;
            }
        }
        else if (transform.position.x < -12)
        {
            if (!shouldTrigger)
            {
                anim.SetTrigger("Still");
                shouldTrigger = true;
                for (int i = 0; i < 3; i++) 
                {
                    brick[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    brick[i].transform.position = new Vector2(rb2d.transform.position.x, 2f);
                    brick[i].SetActive(false);
                }
            }
        }
            

        // If the game is over, stop scrolling.
        if (GameControl.instance.gameOver == true)
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}
