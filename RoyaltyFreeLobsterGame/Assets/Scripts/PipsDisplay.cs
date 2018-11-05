using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipsDisplay : MonoBehaviour {

  public GameObject[] pips;
  private int level;

  void updateDisplay() {
    for(int i = 0; i < pips.Length; ++i) {
      pips[i].SetActive(i<level);
    }
  }
  public void setLevel(int l) {
    level = l;
    updateDisplay();
  }
}
