﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyWithHeal : MonoBehaviour {

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		//rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
	}

	// Update is called once per frame
	void Update () {
        if (transform.position.x < GameControl.instance.Boundary_RIGHT 
            && transform.position.x > GameControl.instance.Boundary_LEFT) {
            transform.position = new Vector2 (transform.position.x + GameControl.instance.scrollSpeed * Time.deltaTime, 
                Mathf.Sin (Time.time * 2) * 2);
		} else {
			transform.GetChild (0).gameObject.SetActive (true);
		}
	}
}
