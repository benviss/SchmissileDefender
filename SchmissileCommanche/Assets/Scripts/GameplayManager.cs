using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

  public GameObject baseManagerObject;
  public GameObject enemyManagerObject;

  private BaseManager baseManagerComponent;
  private EnemySpawnManager enemySpawnManagerComponent;

  public GameObject introPanel;
  public GameObject levelPanel;
  public GameObject levelPanelText;
  public GameObject gameOverPanel;

  public int startingMissiles = 5;
  public int BaseMissileModifier = 5;

  public int RoundNumber = 1;

  public bool gameRunning = false;

  private void Start()
  {
    baseManagerComponent = baseManagerObject.GetComponent<BaseManager>();
    enemySpawnManagerComponent = enemyManagerObject.GetComponent<EnemySpawnManager>();

    introPanel.SetActive(true);
  }



  private void Update()
  {
    if (!gameRunning && Input.GetKeyDown(KeyCode.Space)) {
      RestartGame();

      introPanel.SetActive(false);
      gameOverPanel.SetActive(false);
    }

    if (!gameRunning) return;
    //game running management

    if (baseManagerComponent.AllBasesDead()) {
      gameRunning = false;
      gameOverPanel.SetActive(true);
      Debug.Log("Game Over, Man");
      return;
    }

    if (enemySpawnManagerComponent.NoActiveMissileBarrage()) {
      NextRound();
    }
  }

  public void RestartGame()
  {
    gameRunning = false;

    baseManagerObject.GetComponent<BaseManager>().ResetBases();
    enemyManagerObject.GetComponent<EnemySpawnManager>().ResetEnemies();

    NewGame();
  }

  public void NewGame()
  {
    gameRunning = true;
    levelPanel.SetActive(true);
    levelPanelText.GetComponent<Text>().text = "Nights Survived: " + (RoundNumber - 1);
    baseManagerObject.GetComponent<BaseManager>().StartNewGame(startingMissiles * (RoundNumber) + BaseMissileModifier);
    enemyManagerObject.GetComponent<EnemySpawnManager>().StartNewGame(startingMissiles * RoundNumber, RoundNumber);

    BaseMissileModifier--;
  }

  private void NextRound()
  {
    RoundNumber++;
    levelPanelText.GetComponent<Text>().text = "Nights Survived: " + (RoundNumber - 1);

    Debug.Log("Starting Round " + RoundNumber);
    enemySpawnManagerComponent.StartRound(startingMissiles * RoundNumber, RoundNumber);

    int adjustedBaseMissiles = startingMissiles * (RoundNumber) + BaseMissileModifier;
    baseManagerComponent.DistributeNewMissiles(adjustedBaseMissiles);

    BaseMissileModifier--;
  }

  public void FireAtPosition(Vector2 targetFirePosition)
  {
    baseManagerObject.GetComponent<BaseManager>().SelectBaseToFire(targetFirePosition);
  }
}
