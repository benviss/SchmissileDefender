using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
  public GameObject[] enemySpawners;
  private List<EnemyMissileSpawner> enemySpawnerComponents = new List<EnemyMissileSpawner>();

  public int StartingMissileCount;
  public float MinFireRate;
  public float MaxFireRate;

  public float MissileSpeed;
  public float MissileExplosionSize;
  public float MissileExplosionSpeed;

  public GameObject baseManagerObject;

  // Use this for initialization
  public void StartNewGame (int missileCount, int roundNumber) {
    enemySpawners = GameObject.FindGameObjectsWithTag("EnemyMissileSpawner");

    foreach (var enemySpawnerObject in enemySpawners) {
      EnemyMissileSpawner enemySpawnerObjectComponet = enemySpawnerObject.GetComponent<EnemyMissileSpawner>();
      enemySpawnerObjectComponet.Initialize(MissileExplosionSize, MissileExplosionSpeed, MissileSpeed, MinFireRate, MaxFireRate, baseManagerObject);
      enemySpawnerComponents.Add(enemySpawnerObjectComponet);
    }
    Debug.Log("Finished initializing EnemySpawners");
    StartRound(missileCount, roundNumber);
  }

  public bool NoActiveMissileBarrage()
  {
    foreach (var item in enemySpawnerComponents) {
      if (item.MissileBarrageActive()) {
        return false;
      }
    }
    return true;
  }

  public void ResetEnemies()
  {
    foreach (GameObject item in enemySpawners) {
      item.GetComponent<EnemyMissileSpawner>().CancelMissileBarrage();
      foreach (Transform enemyChild in item.transform) {
        Destroy(enemyChild.gameObject);
      }
    }
  }

  public void StartRound(int missileCount, int roundNumber)
  {
    foreach (EnemyMissileSpawner item in enemySpawnerComponents) {
      item.StartMissileBarrage(missileCount, roundNumber);
    }
  }
}
