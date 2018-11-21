using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

  public float baseHealth;
  public float remainingMissiles = 0;

  public GameObject missilePrefab;
  public float missileSpeed;

  public float fireRate;
  private float lastFire;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

  public void Fire(Vector3 targetPos)
  {
    remainingMissiles--;
    Vector3 difference = targetPos - transform.position;
    difference.Normalize();

    float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

    GameObject missile = (GameObject)Instantiate(
      missilePrefab,
      transform.position,
      Quaternion.Euler(0f, 0f, rotation_z - 90));

    missile.GetComponent<Rigidbody2D>().velocity = difference * missileSpeed;
    missile.GetComponent<Missile>().targetPosition = targetPos;

    lastFire = Time.time;
  }

  void Hit(float damage)
  {
    baseHealth -= damage;
  }

  public bool CanFire()
  {
    return (remainingMissiles > 0 && lastFire + fireRate < Time.time);
  }
}
