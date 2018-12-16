using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {
	public GameObject[] bases;
	private List<Base> baseComponents = new List<Base>();

  public float BaseStartinghealth;
	public float FireRate;
  public int MissileCount;
  public float MissileSpeed;
  public float MissileExplosionRadius;
  public float MissileExplosionSpeed;

  // Use this for initialization
  public void StartNewGame (int startingMissiles) {
    bases = null;
		bases = GameObject.FindGameObjectsWithTag("Base");

    foreach (var baseObject in bases) {
      Base baseObjectComponet = baseObject.GetComponent<Base>();
      baseObjectComponet.Initialize(BaseStartinghealth, MissileExplosionRadius, MissileExplosionSpeed, MissileSpeed, FireRate);
      baseComponents.Add(baseObjectComponet);
    }
    Debug.Log("Finished initializing bases");

    DistributeNewMissiles(startingMissiles);
  }

  // Update is called once per frame
  void Update () {

	}

	public void SelectBaseToFire(Vector2 targetFirePosition)  {
		Base currClosestBase = null;
		foreach (Base curBase in baseComponents) {
      if (curBase == null) {
        continue;
      }
			if (currClosestBase == null || Vector2.Distance(targetFirePosition, curBase.transform.position) < Vector2.Distance(targetFirePosition, currClosestBase.transform.position))	{
        if (curBase.IsAlive() && curBase.CanFire()) {
          currClosestBase = curBase;
        }
      }
		}
    if (currClosestBase != null) {
      currClosestBase.Fire(targetFirePosition);
    }
  }

  public GameObject GetRandomAliveBase()
  {
    List<int> baseComponentsKeys = new List<int>();
    for (int i = 0; i < baseComponents.Count; i++) {
      if (baseComponents[i].IsAlive()) {
        baseComponentsKeys.Add(i);
      }
    }
    if (baseComponentsKeys.Count > 0) {
      return bases[baseComponentsKeys[Random.Range(0, baseComponentsKeys.Count)]];
    }
    else {
      return null;
    }
  }

  public bool AllBasesDead()
  {
    foreach (var item in baseComponents) {
      if (item.IsAlive()) {
        return false;
      }
    }
    return true;
  }

  public void DistributeNewMissiles(int newMissiles)
  {
    int remainingMissilesToAdd = newMissiles;

    List<int> baseComponentsKeys = new List<int>();
    for (int i = 0; i < baseComponents.Count; i++) {
      if (baseComponents[i].IsAlive()) {
        baseComponentsKeys.Add(i);
      }
    }

    int idx = 0;
    while (remainingMissilesToAdd > 0) {
      if (idx == baseComponentsKeys.Count) idx = 0;
      baseComponents[baseComponentsKeys[idx]].AddMissiles(1);
      remainingMissilesToAdd--;
      idx++;
    }
  }

  public void ResetBases()
  {
    foreach (GameObject item in bases) {
      foreach (Transform baseChild in item.transform) {
        Destroy(baseChild.gameObject);
      }
    }

    baseComponents.Clear();
  }
}
