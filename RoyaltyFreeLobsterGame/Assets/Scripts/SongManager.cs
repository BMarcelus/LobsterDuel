using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {

  public bool newSong;
  private static SongManager instance = null;
  public static SongManager Instance {
    get { return instance; }
  }
 void Start() {
     if (instance != null && instance != this) {
        AudioSource instanceAudio = instance.GetComponent<AudioSource>();
        AudioSource myAudio = GetComponent<AudioSource>();
        if(instanceAudio.clip != myAudio.clip) {
          instanceAudio.clip = GetComponent<AudioSource>().clip;
          instanceAudio.Play();
        }
        {
          Destroy(this.gameObject);
        }
        return;
     } else {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
     }
 }
}
