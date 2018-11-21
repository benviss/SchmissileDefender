using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	private Vector2 startingPos;

  public Vector2 targetPosition;
  public Vector2 lastPosition;

  void Start(){
		startingPos = transform.position;
    lastPosition = transform.position;
  }

  private void Update()
  {
    //if (Vector2.Distance(lastPosition, targetPosition) < Vector2.Distance(transform.position, targetPosition)) {
    //  Destroy(this.gameObject);
    //}
    Debug.Log(Vector2.Distance(transform.position, targetPosition));
    if (Vector2.Distance(transform.position, targetPosition) < .1) {
      Destroy(this.gameObject);
    }
    //lastPosition = transform.position;
  }
}
