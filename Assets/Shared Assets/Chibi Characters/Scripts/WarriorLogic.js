#pragma strict

//var actions : String[];
//var indexAction : int;
//var statusGUI : GUIText;
var speed : float = 1.5;
private var warriorController : CharacterController;
private var jumpSpeed : float = 5;
private var moveDirection : Vector3 = Vector3.zero;
private var boostIncrement = 1;
private var canAnimate : boolean;
private var gravity = 15;
private var run : boolean = false;
//private var rotateY : float;

function Start() {
//	indexAction = 0;
//	statusGUI.text = actions[indexAction];
	warriorController = GetComponent(CharacterController);
	GetComponent.<Animation>().wrapMode = WrapMode.Loop;
	GetComponent.<Animation>()["jump-01"].wrapMode = WrapMode.Once;
	GetComponent.<Animation>()["attack-02"].wrapMode = WrapMode.Once;
//	GetComponent.<Animation>()["Crouching and aiming"].wrapMode = WrapMode.Once;
//	GetComponent.<Animation>()["Standing with single fire"].wrapMode = WrapMode.Once;
//	GetComponent.<Animation>()["Idle crouching with single fire"].wrapMode = WrapMode.Once;
}

function Update() {
	print(canAnimate);
	if(Input.GetButtonDown("Boost")) {
		run = true;
	}
	else if(Input.GetButtonUp("Boost")) {
		run = false;
	}
	
	if(!Input.GetButtonDown("Attack1")) {
		canAnimate = true;
	}
	
//	if(Input.GetButtonDown("Switch")) {
//		indexAction++;
//		if(indexAction == actions.length)
//			indexAction = 0;
//		statusGUI.text = actions[indexAction];
//	}
	
	if(warriorController.isGrounded == true) {
		if(Input.GetAxis("Vertical") > 0.02 && !Input.GetKey("left ctrl")) {
			GetComponent.<Animation>()["walk-01"].speed = 1;
			if(run) {
				GetComponent.<Animation>().CrossFade("run-01");
				GetComponent.<Animation>()["run-01"].speed = 1;
				boostIncrement = 3;
			}
			else {
				GetComponent.<Animation>().CrossFade("walk-01");
				boostIncrement = 1;
			}
		}
		else if(Input.GetAxis("Vertical") < -0.02 && !Input.GetKey("left ctrl")) {
			GetComponent.<Animation>()["walk-01"].speed = -1;
			GetComponent.<Animation>().CrossFade("walk-01");
		} else {
			if(Input.GetButton("Attack1") && canAnimate) {
				DoAttack1();
			} else if(Input.GetButton("Attack2") && canAnimate) {
				DoAttack2();
			} else {
				IdleAnimation();
				boostIncrement = 1;
			}
		}
		
		if(!Input.GetButtonDown("Attack1")) {
			moveDirection = Vector3(0, 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= boostIncrement * speed;
		}
		
		if(Input.GetButtonDown("Jump")) {
			moveDirection.y = jumpSpeed;
			GetComponent.<Animation>()["jump-01"].speed = 2;
			GetComponent.<Animation>().CrossFade("jump-01");
		}
	}
	
//	if(Input.GetButtonDown("Attack1") && canAnimate) {
//		DoAction();
//	}
	
	transform.eulerAngles.y += Input.GetAxis("Horizontal") * 5; // Use keyboard to control character turning
//	rotateY = (Input.GetAxis("Mouse X") * 300) * Time.deltaTime; // Use mouse horizontal position to control character turning
//	warriorController.transform.Rotate(0, rotateY, 0);
	moveDirection.y -= gravity * Time.deltaTime;
	warriorController.Move(moveDirection * Time.deltaTime);
}

function IdleAnimation() {
	GetComponent.<Animation>().CrossFade("idle-03");
}

function DoAttack1() {
	GetComponent.<Animation>()["attack-02"].speed = 1;
	GetComponent.<Animation>().CrossFade("attack-02");
	canAnimate = false;
}

function DoAttack2() {
	GetComponent.<Animation>()["attack-06"].speed = 1;
	GetComponent.<Animation>().CrossFade("attack-06");
	canAnimate = false;
}