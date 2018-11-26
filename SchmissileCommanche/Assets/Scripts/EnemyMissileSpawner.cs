using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileSpawner : MonoBehaviour {
  public float StartingMissileCount;
  public float MinFireRate;
  public float MaxFireRate;

  public float MissileSpeed;
  public float MissileExplosionRadius;
  public float MissileExplosionSpeed;

  public GameObject missilePrefab;

  private float nextfire;

  private List<Base> baseObjects = new List<Base>();

  private bool missileBarrageActive;
  private float remainingMissiles;

  public GameObject baseManagerObject;
  private BaseManager baseManager;

  // Use this for initialization
  void Start () {
    baseManager = baseManagerObject.GetComponent<BaseManager>();
    if (baseManager != null) {
      StartMissileBarrage(StartingMissileCount);
    } else {
      Debug.Log("Base Manager was null in Enemy Missile Spawner");
    }
  }
	
	// Update is called once per frame
	void Update () {
    if (missileBarrageActive) {
      if (Time.time > nextfire) {
        FireAtRandomBase();
      }
    }
	}

  public void StartMissileBarrage(float missileCount)
  {
    GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
    foreach (GameObject item in bases) {
      Base baseObject = item.GetComponent<Base>();
      baseObjects.Add(baseObject);
    }

    remainingMissiles = missileCount;
    nextfire = Time.time + MinFireRate;
    missileBarrageActive = true;
  }

  void FireAtRandomBase()
  {
    //Base targetBase = baseObjects[Random.Range(0, baseObjects.Count)];
    GameObject targetBase = baseManager.GetRandomBase();
    if (targetBase == null) {
      Debug.Log("Base was returned null from base manager in GetRandomBase");
      return;
    }
    remainingMissiles--;
    Vector3 difference = targetBase.transform.position - transform.position;
    difference.Normalize();

    float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

    GameObject missile = (GameObject)Instantiate(
      missilePrefab,
      transform.position,
      Quaternion.Euler(0f, 0f, rotation_z - 90));

    Missile missileComponent = missile.GetComponent<Missile>();
    missileComponent.Initialize(targetBase.transform.position, MissileExplosionRadius, MissileExplosionSpeed);

    missile.GetComponent<Rigidbody2D>().velocity = difference * MissileSpeed;

    nextfire = Time.time + Random.Range(MinFireRate, MaxFireRate);
  }
}
