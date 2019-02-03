using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Stage2ObjectController : MonoBehaviour {

	public float speed;
	public GameObject stage1CorrectOption;
	private Rigidbody rb;
	public Text titleText;
	public Text hintText;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
//		titleText.text = "Setter method topic";
//		titleText.gameObject.SetActive (true);
//		hintText.gameObject.SetActive (true);
		speed = StaticGameInfo.speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		mobileMovement();
		//keyboardMovement();
		//joystickMovement();
	}

	void joystickMovement() {
		if (!stage1CorrectOption.activeSelf) {
			float moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			float moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rb.AddForce (movement * speed);
		}
	}

	void mobileMovement() {
		if (!stage1CorrectOption.activeSelf) {
			Vector3 dir = Vector3.zero;
			// we assume that device is held parallel to the ground
			// and Home button is in the right hand
			// remap device acceleration axis to game coordinates:
			//  1) XY plane of the device is mapped onto XZ plane
			//  2) rotated 90 degrees around Y axis
			dir.x = -Input.acceleration.y;
			dir.z = Input.acceleration.x;

			// clamp acceleration vector to unit sphere
			if (dir.sqrMagnitude > 1)
				dir.Normalize();

			// Make it move 10 meters per second instead of 10 meters per frame...
			dir *= Time.deltaTime;

			// Move object
			rb.transform.Translate(dir * speed);
			// rb.AddForce(dir * speed);
		}
	}

	void keyboardMovement() {
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		rb.AddForce (movement * speed);
		if (!stage1CorrectOption.activeSelf) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate (Vector3.forward * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate (Vector3.back * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.Translate (Vector3.left * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				transform.Translate (Vector3.right * Time.deltaTime * speed);
			}
		}
	}

}
