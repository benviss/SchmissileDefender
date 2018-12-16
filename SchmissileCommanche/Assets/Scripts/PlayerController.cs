using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public GameplayManager gameManager;

	// Use this for initialization
	void Start () {
    gameManager = FindObjectOfType<GameplayManager>();
	}
	
	// Update is called once per frame
	void Update () {
    if (gameManager.gameRunning && Input.GetButtonDown("Fire1")) {
      gameManager.FireAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    if (Input.GetKeyDown("r")) {
      gameManager.RestartGame();
    }

    if (Input.GetKeyDown("1")) {
      gameManager.RoundNumber++;
    }
  }
}
