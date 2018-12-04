using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : movement {

	
	// Update is called once per frame
	protected override void Update () {
		input = new Vector3(
      Input.GetAxisRaw("Horizontal"),
      Input.GetAxisRaw("Vertical"),
      0
    );
    base.Update();
	}
}
