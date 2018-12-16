using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

  public GameObject baseManagerObject;
  public GameObject enemyManagerObject;

  private BaseManager baseManagerComponent;
  private EnemySpawnManager enemySpawnManagerComponent;

  public int startingMissiles = 5;
  public int BaseMissileModifier = 5;

  public int RoundNumber = 1;

  public bool gameRunning = false;

  private void Start()
  {
    baseManagerComponent = baseManagerObject.GetComponent<BaseManager>();
    enemySpawnManagerComponent = enemyManagerObject.GetComponent<EnemySpawnManager>();
    NewGame();
  }

  private void Update()
  {
    if (baseManagerComponent.AllBasesDead()) {
      Debug.Log("Game Over, Man");
    }
    else if (enemySpawnManagerComponent.NoActiveMissileBarrage()) {
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
    baseManagerObject.GetComponent<BaseManager>().StartNewGame(startingMissiles * (RoundNumber) + BaseMissileModifier);
    enemyManagerObject.GetComponent<EnemySpawnManager>().StartNewGame(startingMissiles * RoundNumber, RoundNumber);

    BaseMissileModifier--;
  }

  public void EndGame()
  {
    gameRunning = false;
  }

  private void NextRound()
  {
    RoundNumber++;
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
