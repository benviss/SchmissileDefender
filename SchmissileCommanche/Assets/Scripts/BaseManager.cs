using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {
	public GameObject[] bases;
	private List<Base> baseComponents = new List<Base>();

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
      baseComponents.Add(baseObjectComponet);
    }
    Debug.Log("Finished initializing bases");
  }

  // Update is called once per frame
  void Update () {
    List<Base> basesToRemove = new List<Base>();
    //foreach (Base item in baseComponents) {
    //  if (!item.IsAlive()) {
    //    basesToRemove.Add(item);
    //  }
    //}
    //foreach (var item in basesToRemove) {
    //  baseComponents.Remove(item);
    //}

    if (Input.GetButtonDown("Fire1")){
			SelectBaseToFire();
		}
	}

	void SelectBaseToFire(){
		Base currClosestBase = null;
		Vector2 targetFirePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		foreach (Base curBase in baseComponents) {
      if (curBase == null) {
        continue;
      }
			if (currClosestBase == null || Vector2.Distance(targetFirePosition, curBase.transform.position) < Vector2.Distance(targetFirePosition, currClosestBase.transform.position))	{
        if (curBase.CanFire()) {
          currClosestBase = curBase;
        }
      }
		}
    if (currClosestBase != null) {
      currClosestBase.Fire(targetFirePosition);
    }
  }

  public GameObject GetRandomBase()
  {
    return bases[Random.Range(0, bases.Length)];
  }
}
