using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	public float upForce;					//Upward force of the "flap".
    private float protectTime = 2f;
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

	void Start(){
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

    void OnTriggerEnter2D(Collider2D other) {
		if (!isCollided && other.tag != "boundary" && other.tag != "star" && other.tag != "Heal") {
			if (GameControl.instance.ReduceHP (1)) {
				hp = 0;
				BirdDie ();
			} else {
				hp = GameControl.instance.getHP();
				anim.SetTrigger("collide");
				isCollided = true;
			}
        } else if(other.tag == "boundary") {
			GameControl.instance.ReduceHP (3);
			hp = 0;
			BirdDie();
        } else if (other.gameObject.CompareTag("star")) {
            other.gameObject.SetActive(false);
            GameControl.instance.BirdScored(5);
            if (GameControl.instance.updateStars == false) {
                GameControl.instance.RenewStars(other.gameObject.transform.parent);
            }
		} else if (other.gameObject.CompareTag("Heal")) {
			other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			GameControl.instance.IncreaseHP (1);
			hp = GameControl.instance.getHP();
		}
    }

    void BirdDie() {
        polycollider.isTrigger = false;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 1f;
        isDead = true;
        GameControl.instance.BirdDied();
    }
}
