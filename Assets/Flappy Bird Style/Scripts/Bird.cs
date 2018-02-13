using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	public float upForce;					//Upward force of the "flap".
    private float protectTime = 1f;
	private bool isDead = false;			//Has the player collided with a wall?
    private float timeElapse = 0f;
    private float protectTimeElapse = 0f;
    private float invincibleTimeElapse = 0f;
    private bool isCollided = false;

	//private Animator anim;					//Reference to the Animator component.
	private Rigidbody2D rb2d;				//Holds a reference to the Rigidbody2D component of the bird.
    private Collider2D polycollider;
    private Animator anim;
    private int hp = 3;
    private bool disableTrigger = false;

	void Start()
	{
		//Get reference to the Animator component attached to this GameObject.
		//anim = GetComponent<Animator> ();
		//Get and store a reference to the Rigidbody2D attached to this GameObject.
		rb2d = GetComponent<Rigidbody2D>();
        polycollider = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        polycollider.isTrigger = true;
	}

	void Update()
    {
        if (isCollided)
        {
            protectTimeElapse += Time.deltaTime;
            if (protectTimeElapse >= protectTime)
            {
                anim.SetTrigger("idle");
                isCollided = false;
                protectTimeElapse = 0f;
                if (disableTrigger)
                    polycollider.isTrigger = false;
            }
        }
		//Don't allow control if the bird has died.
		if (isDead == false) 
		{
			//Look for input to trigger a "flap".
//			if (Input.GetMouseButtonDown(0)) 
//			{
//				//...tell the animator about it and then...
//				anim.SetTrigger("Flap");
//				//...zero out the birds current y velocity before...
//				rb2d.velocity = Vector2.zero;
//				//	new Vector2(rb2d.velocity.x, 0);
//				//..giving the bird some upward force.
//				rb2d.AddForce(new Vector2(0, upForce));
//			}
            if (Input.GetKeyUp(KeyCode.UpArrow)) 
            {
//                if (rb2d.velocity.y < 0)
//                {
//                    rb2d.velocity = Vector2.zero;
//                }
//                else
//                {
                    //...tell the animator about it and then...
                    //anim.SetTrigger("Flap");
                    //  new Vector2(rb2d.velocity.x, 0);
                    //..giving the bird some upward force.
                    rb2d.AddForce(new Vector2(0, upForce));
//                }
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
//                if (rb2d.velocity.y > 0)
//                {
//                    rb2d.velocity = Vector2.zero;
//                }
//                else
//                {
                    //...tell the animator about it and then...
                    //anim.SetTrigger("Flap");
                    //  new Vector2(rb2d.velocity.x, 0);
                    //..giving the bird some upward force.
                    rb2d.AddForce(new Vector2(0, -upForce));
//                }
            }


            timeElapse += Time.deltaTime;
            if (timeElapse >= GameControl.instance.scoreRate)
            {
                GameControl.instance.BirdScored(1);
                timeElapse = 0;
            }
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		rb2d.velocity = Vector2.zero;
		isDead = true;
        GameControl.instance.ReduceHP(hp);
		GameControl.instance.BirdDied ();
	}

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!isCollided)
        {
            if (hp > 1)
            {
                GameControl.instance.ReduceHP(hp);
                hp--;
                anim.SetTrigger("collide");
                isCollided = true;
                if (hp == 1)
                    disableTrigger = true;
            }
        }
    }

}
