﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext;				//A reference to the object that displays the text which appears when the player dies.
    public GameObject hpComponent;
    private GameObject currentHeart;

	public int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -1.5f;
    public float scoreRate = 1f;

    public bool updateStars = false;
    private Transform lastStars;
    private float updateStarsRate = 2f;
    private float timeSinceCollision = 0f;

	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}

	void Update()
	{
		//If the game is over and the player has pressed some input...
		if (gameOver && Input.GetMouseButtonDown(0)) 
		{
			//...reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        if (updateStars)
        {
            timeSinceCollision += Time.deltaTime;
            if (timeSinceCollision >= updateStarsRate)
            {
                timeSinceCollision = 0f;
                updateStars = false;
                foreach (Transform child in lastStars)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
	}

    public void BirdScored(int s)
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score+=s;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}

	public void BirdDied()
	{
		//Activate the game over text.
		gameOvertext.SetActive (true);
		//Set the game to be over.
		gameOver = true;
	}

    public void ReduceHP(int hp)
    {
        switch (hp)
        {
            case 1:
                hpComponent.transform.Find("heart1").gameObject.SetActive(false);
                break;
            case 2:
                hpComponent.transform.Find("heart2").gameObject.SetActive(false);
                break;
            case 3:
                hpComponent.transform.Find("heart3").gameObject.SetActive(false);
                break;
            case 4:
                hpComponent.transform.Find("heart3").gameObject.SetActive(false);
                hpComponent.transform.Find("heart2").gameObject.SetActive(false);
                hpComponent.transform.Find("heart1").gameObject.SetActive(false);
                break;
            default:
                break;    
        }
    }
    public void RenewStars(Transform parent)
    {
        updateStars = true;
        lastStars = parent;
    }
}
