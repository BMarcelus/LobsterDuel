using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHighlighter : MonoBehaviour {

  public Color highlightColor;
  public Color defaultColor;
  public GameObject arrow;
  private SpriteRenderer[] renderers;
  private Vector3 startScale;
	// Use this for initialization
	void Start () {
    if(arrow==null)arrow = gameObject;
		renderers = arrow.GetComponentsInChildren<SpriteRenderer>();
    startScale = transform.localScale;
    foreach(SpriteRenderer r in renderers) {
      r.color = defaultColor;
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseOver() {
    // transform.localScale*=2;
    foreach(SpriteRenderer r in renderers) {
      r.color = highlightColor;
      // r.transform.localScale*=2;
    }
  }
  void OnMouseExit() {
    foreach(SpriteRenderer r in renderers) {
      r.color = defaultColor;
    }
  }
}
