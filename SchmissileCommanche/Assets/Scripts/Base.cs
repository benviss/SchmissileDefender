using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

  public float baseHealth;
  private float remainingMissiles;
  private float missileSpeed;
  private float fireRate;
  private float missileExplosionRadius;
  private float missileExplosionSpeed;

  public GameObject missilePrefab;
  private float nextFire;

  private Renderer mRenderer;
  private float reloadAnimationTime;

  public void Initialize(float baseHealth, float remainingMissiles, float missileExplosionRadius, float missileExplosionSpeed, float missileSpeed, float fireRate)
  {
    this.baseHealth = baseHealth;
    this.remainingMissiles = remainingMissiles;
    this.missileExplosionRadius = missileExplosionRadius;
    this.missileExplosionSpeed = missileExplosionSpeed;
    this.missileSpeed = missileSpeed;
    this.fireRate = fireRate;
  }

  // Use this for initialization
  void Start () {
    mRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
    if (nextFire > Time.time) {
      mRenderer.material.color = Color.Lerp(Color.red, Color.green, reloadAnimationTime);

      if (reloadAnimationTime < 1) {
        reloadAnimationTime += Time.deltaTime / fireRate;
      }
    }
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

    missile.layer = this.gameObject.layer;

    Missile missileComponent = missile.GetComponent<Missile>();
    missileComponent.Initialize(targetPos, missileExplosionRadius, missileExplosionSpeed);

    missile.GetComponent<Rigidbody2D>().velocity = difference * missileSpeed;

    nextFire = Time.time + fireRate;
    reloadAnimationTime = 0;
  }

  void Hit(float damage)
  {
    baseHealth -= damage;
    if (baseHealth <= 0) {
      Die();
    }
  }

  public bool CanFire()
  {
    return (remainingMissiles > 0 && nextFire < Time.time);
  }

  public bool IsAlive()
  {
    return baseHealth > 0;
  }

  //private void OnCollisionEnter2D(Collision2D collision)
  //{
  //  if (collision.gameObject.tag.Equals("Explosion")) {
  //    Destroy(this.gameObject);
  //  }
  //}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer != this.gameObject.layer) {
      Hit(9001);
    }
  }

  private void Die()
  {
    mRenderer.material.color = Color.clear;
  }
}
