using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {
	public GameObject[] bases;

	public float BaseStartinghealth;
	public float FireRate;
  public float MissileCount;
  public float MissileSpeed;
  public float MissileExplosionRadius;
  public float MissileExplosionSpeed;

  // Use this for initialization
  void Start () {
		bases = GameObject.FindGameObjectsWithTag("Base");

    foreach (var baseObject in bases) {
      Base baseObjectComponet = baseObject.GetComponent<Base>();
      baseObjectComponet.Initialize(BaseStartinghealth, MissileCount, MissileExplosionRadius, MissileExplosionSpeed, MissileSpeed, FireRate);
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
