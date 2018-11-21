using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

  public float explosionRadius;
  public float explosionSpeed;

  private bool expanding = true;

  public void Initialize(float explosionRadius, float explosionSpeed)
  {
    this.explosionRadius = explosionRadius;
    this.explosionSpeed = explosionSpeed;
  }

  // Use this for initialization
  void Start () {
    transform.localScale = new Vector2(0,0);
	}
	
	// Update is called once per frame
	void Update () {
    float deltaScale = Time.deltaTime * explosionSpeed;

    Vector3 newScale = transform.localScale;
    newScale.x = newScale.x + (expanding ? deltaScale : -deltaScale * 2);
    newScale.y = newScale.y + (expanding ? deltaScale : -deltaScale * 2);

    transform.localScale = newScale;

    if (transform.localScale.x >= explosionRadius || transform.localScale.y >= explosionRadius) {
      expanding = false;
    }

    if (!expanding && (transform.localScale.x <= 0 || transform.localScale.y <= 0)) {
      Destroy(this.gameObject);
    }
  }
}
