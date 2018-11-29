using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHighlighter : MonoBehaviour {

  public Color highlightColor;
  public Color defaultColor;
  private SpriteRenderer[] renderers;
  private Vector3 startScale;
	// Use this for initialization
	void Start () {
		renderers = GetComponentsInChildren<SpriteRenderer>();
    startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseOver() {
    transform.localScale*=2;
    foreach(SpriteRenderer r in renderers) {
      r.color = highlightColor;
    }
  }
  void OnMouseExit() {
    foreach(SpriteRenderer r in renderers) {
      r.color = defaultColor;
    }
  }
}
