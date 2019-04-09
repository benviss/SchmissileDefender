using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

  private float explosionSize;
  private float explosionSpeed;


  private float colorR = 255;
  private float colorB = 40;
  private float minColorG = 0;
  private float maxColorG = 255;

  private SpriteRenderer mSpriteRenderer;

  private bool expanding = true;

  public void Initialize(float explosionSize, float explosionSpeed)
  {
    this.explosionSize = explosionSize;
    this.explosionSpeed = explosionSpeed;
  }

  // Use this for initialization
  void Start () {
    transform.localScale = new Vector2(0,0);
    mSpriteRenderer = this.GetComponent<SpriteRenderer>();
    mSpriteRenderer.color = new Color(colorR / 255f, Random.Range(minColorG, maxColorG) / 255f, colorB / 255f);
  }

  // Update is called once per frame
  void Update () {
    float deltaScale = Time.deltaTime * explosionSpeed;

    Vector3 newScale = transform.localScale;
    newScale.x = newScale.x + (expanding ? deltaScale : -deltaScale * 2);
    newScale.y = newScale.y + (expanding ? deltaScale : -deltaScale * 2);

    transform.localScale = newScale;

    if (transform.localScale.x >= explosionSize || transform.localScale.y >= explosionSize) {
      expanding = false;
    }

    if (!expanding && (transform.localScale.x <= 0 || transform.localScale.y <= 0)) {
      Destroy(this.gameObject);
    }
  }
}
