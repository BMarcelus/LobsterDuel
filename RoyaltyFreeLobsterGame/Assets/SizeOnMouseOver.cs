using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOnMouseOver : MonoBehaviour {

  private Vector3 size;
  public GameObject graphic;
  private bool highlighted = false;
  private SpriteRenderer[] spriteRenderers;
  private Canvas[] canvases;
  public bool active = true;
	// Use this for initialization
	void Start () {
		size = graphic.transform.localScale;
    spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    canvases = GetComponentsInChildren<Canvas>();
    if(!GetComponentInParent<PlayerHand>()) active = false;
	}

  void Update() {
    if(GetComponent<Lobster>().enabled) {
      OnMouseExit();
      active = false;
    }
  }
  
  void OnMouseOver() {
    if(!active)return;
    graphic.transform.localScale = size*1.5f;
    graphic.transform.localPosition = Vector3.up*size.y*.8f;
    if(!highlighted) {
      highlighted = true;
      foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
        spriteRenderer.sortingOrder += 10;
      }
      foreach(Canvas canvas in canvases) {
        canvas.sortingOrder += 10;
      }
    }
  }

  void OnMouseExit() {   
    if(!active)return;
    graphic.transform.localScale = size;
    graphic.transform.localPosition = Vector3.zero;
    if(highlighted) {
      highlighted = false;
      foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
        spriteRenderer.sortingOrder -= 10;
      }
      foreach(Canvas canvas in canvases) {
        canvas.sortingOrder -= 10;
      }
    }
  }
}
