using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {
	public GameObject[] bases;

	public float fireRate;

  public float startingMissileCount;

  // Use this for initialization
  void Start () {
		bases = GameObject.FindGameObjectsWithTag("Base");

    foreach (var baseObject in bases) {
      Base baseObjectComponenet = baseObject.GetComponent<Base>();
      baseObjectComponenet.remainingMissiles = startingMissileCount;
      baseObjectComponenet.fireRate = fireRate;
    }
    Debug.Log("Finished initializing bases");
  }

  // Update is called once per frame
  void Update () {
		if(Input.GetButtonDown("Fire1")){
			SelectBaseToFire();
		}
	}

	void SelectBaseToFire(){
		GameObject currClosestBase = null;
		Vector2 targetFirePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		foreach (GameObject curBase in bases){
			if (currClosestBase == null || Vector2.Distance(targetFirePosition, curBase.transform.position) < Vector2.Distance(targetFirePosition, currClosestBase.transform.position))	{
        if (curBase.GetComponent<Base>().CanFire()) {
          currClosestBase = curBase;
        }
      }
		}
    if (currClosestBase != null) {
      currClosestBase.GetComponent<Base>().Fire(targetFirePosition);
    }
  }
}
