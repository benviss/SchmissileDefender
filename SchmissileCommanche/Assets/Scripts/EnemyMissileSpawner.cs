using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileSpawner : MonoBehaviour {
  public float StartingMissileCount;
  public float MinFireRate;
  public float MaxFireRate;

  public float cMinFireRate;
  public float cMaxFireRate;

  public float MissileSpeed;
  public float MissileExplosionRadius;
  public float MissileExplosionSpeed;

  public GameObject missilePrefab;

  private float nextfire;

  private bool missileBarrageActive;
  private float remainingMissiles;

  public float MaxX = 13;
  public float MinX = -13;

  private BaseManager baseManager;

  public void Initialize(float missileExplosionRadius, float missileExplosionSpeed, float missileSpeed, float minFireRate, float maxFireRate, GameObject baseManagerObject)
  {
    this.MissileExplosionRadius = missileExplosionRadius;
    this.MissileExplosionSpeed = missileExplosionSpeed;
    this.MissileSpeed = missileSpeed;
    this.MinFireRate = minFireRate;
    this.MaxFireRate = maxFireRate;

    baseManager = baseManagerObject.GetComponent<BaseManager>();
  }

	// Update is called once per frame
	void Update () {
    if (missileBarrageActive) {
      if (remainingMissiles <= 0) {
        if (transform.childCount <= 0) {
          missileBarrageActive = false;
        }
      } else if (Time.time > nextfire) {
        FireAtRandomBase();
      }
    }
	}

  public void StartMissileBarrage(int missileCount, float modifier)
  {
    float modifiedMissileSpeed = (modifier > 1) ? MissileSpeed * (1 + (modifier / 100)): MissileSpeed;
    cMinFireRate = MinFireRate;
    cMaxFireRate = MaxFireRate;
    for (int i = 0; i < modifier; i++) {
      if (i > 1 && i % 3 == 0) {
        cMinFireRate = cMinFireRate * 0.75f;
        cMaxFireRate = cMaxFireRate * 0.75f;
      }
    }

    remainingMissiles = missileCount;
    nextfire = Time.time + 3f;
    missileBarrageActive = true;
  }

  void FireAtRandomBase()
  {
    //Base targetBase = baseObjects[Random.Range(0, baseObjects.Count)];
    GameObject targetBase = baseManager.GetRandomAliveBase();
    if (targetBase == null) {
      if (baseManager.AllBasesDead()) {
        missileBarrageActive = false;
        Debug.Log("missile barrage ended, this should be changed. missile spawner should be cancelled before it has chance to fire missile if all bases are dead, though it makes sense why maybe one missile could get fired off before its actually cancelled");
        return;
      }
      else {
        Debug.Log("Base was returned null from base manager in GetRandomBase");
        return;
      }
    }
    
    remainingMissiles--;

    Vector2 fireFromPosition = new Vector2(Random.Range(MinX, MaxX), transform.position.y);

    //calculate random position within explosion radius of target base
    Vector2 targetPosition = new Vector2(Random.Range(targetBase.transform.position.x - MissileExplosionRadius, targetBase.transform.position.x + MissileExplosionRadius), targetBase.transform.position.y);

    Vector3 difference = targetPosition - fireFromPosition;
    difference.Normalize();

    float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

    GameObject missile = (GameObject)Instantiate(
      missilePrefab,
      fireFromPosition,
      Quaternion.Euler(0f, 0f, rotation_z - 90), this.transform);

    missile.layer = this.gameObject.layer;

    missile.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);

    Missile missileComponent = missile.GetComponent<Missile>();


    missileComponent.Initialize(targetPosition, MissileExplosionRadius, MissileExplosionSpeed);

    missile.GetComponent<Rigidbody2D>().velocity = difference * MissileSpeed;

    nextfire = Time.time + Random.Range(cMinFireRate, cMaxFireRate);
  }

  public bool MissileBarrageActive()
  {
    return missileBarrageActive;
  }

  public void CancelMissileBarrage()
  {
    missileBarrageActive = false;
  }
}
