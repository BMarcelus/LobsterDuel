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
  private float scalar = 2f;
  private float offset = 0.8f;
	// Use this for initialization
	void Start () {
		size = graphic.transform.localScale;
    spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    canvases = GetComponentsInChildren<Canvas>();
    if(!IsPlayer()) Destroy(this);
	}

  private bool IsPlayer() {
    if(GetComponentInParent<PlayerHand>() != null) return true;
    Lobster lobster = GetComponent<Lobster>();
    if(lobster && lobster.floorAssigned && lobster.floorAssigned.tag == "PlayerSpot") {
      return true;
    }
    return false;
  }

  void Update() {
    if(GetComponent<Lobster>().enabled) {
      // scalar = 1.2f;
      // offset = 0.2f;
      OnMouseExit();
      active = false;
    }
  }
  
  void OnMouseOver() {
    // if(transform.rotation.eulerAngles.y!=0) return;
    if(!active)return;
    graphic.transform.localScale = size*scalar;
    graphic.transform.localPosition = Vector3.up*size.y*offset;
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
