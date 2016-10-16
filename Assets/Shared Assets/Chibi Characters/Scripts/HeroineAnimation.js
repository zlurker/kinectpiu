#pragma strict

function Update() {
	PlayAttackAnim();
}

function PlayAttackAnim() {
	if(Input.GetKeyDown(KeyCode.F1)) {
		GetComponent.<Animation>().Play("run");
	} else if(Input.GetKeyDown(KeyCode.F2)) {
		GetComponent.<Animation>().Play("cast");
	} else if(Input.GetKeyDown(KeyCode.F3)) {
		GetComponent.<Animation>().Play("jump");
	} else if(Input.GetKeyDown(KeyCode.F4)) {
		GetComponent.<Animation>().Play("run_left");
	} else if(Input.GetKeyDown(KeyCode.F5)) {
		GetComponent.<Animation>().Play("run_right");
	} else if(Input.GetKeyDown(KeyCode.F6)) {
		GetComponent.<Animation>().Play("death");
	} else if(Input.GetKeyDown(KeyCode.F7)) {
		GetComponent.<Animation>().Play("attack1");
	} else if(Input.GetKeyDown(KeyCode.F8)) {
		GetComponent.<Animation>().Play("attack2");
	} else if(Input.GetKeyDown(KeyCode.F9)) {
		GetComponent.<Animation>().Play("attack3");
	} else if(Input.GetKeyDown(KeyCode.F10)) {
		GetComponent.<Animation>().Play("attack4");
	} else if(Input.GetKeyDown(KeyCode.F11)) {
		GetComponent.<Animation>().Play("attack5");
	} else if(Input.GetKeyDown(KeyCode.F12)) {
		GetComponent.<Animation>().Play("hurt");
	} else if(Input.GetKeyDown(KeyCode.Space)) {
		GetComponent.<Animation>().Play("idle");
	}
}