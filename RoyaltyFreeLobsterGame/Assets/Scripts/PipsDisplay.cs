using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipsDisplay : MonoBehaviour {

  public GameObject[] pips;
  // public GameObject back;
  private int level;

  void updateDisplay() {
    for(int i = 0; i < pips.Length; ++i) {
      pips[i].SetActive(i<level);
    }
    // if(!back)return;
    // Vector3 scale = back.transform.localScale;
    // scale.x = 1 + (level-1)*.8f;
    // back.transform.localScale = scale;
    // back.transform.localPosition = new Vector3((scale.x-1)/2,0,0);
  }
  public void setLevel(int l) {
    level = l;
    updateDisplay();
  }
}
