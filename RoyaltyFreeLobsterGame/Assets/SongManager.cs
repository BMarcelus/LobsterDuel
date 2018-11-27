using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {

  public bool newSong = false;
  private static SongManager instance = null;
  public static SongManager Instance {
    get { return instance; }
  }
 void Awake() {
     if (instance != null && instance != this) {
        if(newSong) {
          Destroy(instance);
          instance = this;
        } else {
          Destroy(this.gameObject);
        }
        return;
     } else {
        instance = this;
     }
     DontDestroyOnLoad(this.gameObject);
 }
}
