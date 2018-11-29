using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransition : MonoBehaviour {

  public string scene;
  void OnTriggerEnter2D(Collider2D collider) {
    if(collider.name == "Craig") {
      SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
  }
}
