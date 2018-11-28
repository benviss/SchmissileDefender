using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	private Vector2 startingPos;

  public Vector2 targetPosition;
  public Vector2 lastPosition;

  public GameObject explosionPrefab;
  public float explosionRadius;
  public float explosionSpeed;

  public void Initialize(Vector2 targetPosition, float explosionRadius, float explosionSpeed)
  {
    this.targetPosition = targetPosition;
    this.explosionRadius = explosionRadius;
    this.explosionSpeed = explosionSpeed;
  }

  void Start(){
		startingPos = transform.position;
    lastPosition = transform.position;
  }

  private void Update()
  {
    //if (Vector2.Distance(lastPosition, targetPosition) < Vector2.Distance(transform.position, targetPosition)) {
    //  Destroy(this.gameObject);
    //}
    if (Vector2.Distance(transform.position, targetPosition) < .1) {
      Explode();
    }
    //lastPosition = transform.position;
  }

  public void Explode()
  {
    GameObject explosionObject = (GameObject)Instantiate(
     explosionPrefab,
     transform.position,
     transform.rotation);

    explosionObject.layer = this.gameObject.layer;
    Explosion explosion = explosionObject.GetComponent<Explosion>();
    explosion.Initialize(explosionRadius, explosionSpeed);

    Destroy(this.gameObject);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer != this.gameObject.layer) {
      Destroy(this.gameObject);
    }
  }
  //private void O(Collision2D collision)
  //{

  //}
}
