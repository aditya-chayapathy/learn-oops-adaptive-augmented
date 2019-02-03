using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	public GameObject stage1;
	public GameObject stage2;
	public GameObject stage2PickUp;
	public GameObject stage1CorrectOption;
	public GameObject stage1IncorrectOption1;
	public GameObject stage1IncorrectOption2;
	public Text titleText;
	public Text hintText;
	public Text userName;
	public GameObject exitObjects;
	public GameObject arObjects;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		titleText.gameObject.SetActive (true);

		//hack
		userName.text = StaticGameInfo.userName;
		speed = StaticGameInfo.speed;
	}
	
	void FixedUpdate() {
		mobileMovement();
		//keyboardMovement();
		//joystickMovement();
	}

	void joystickMovement() {
		float moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
		float moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}

	void keyboardMovement() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		string activeSceneName = SceneManager.GetActiveScene ().name;
		if (activeSceneName == StaticGameInfo.TASK_3) {
			speed = speed * 3;
		}
		rb.AddForce (movement * speed);
	}

	void mobileMovement() {
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

	void OnTriggerEnter(Collider other) {
		other.isTrigger = false;
		string activeSceneName = SceneManager.GetActiveScene ().name;
		switch (other.gameObject.name) {
			case "Stage1PickUp":
				other.gameObject.SetActive (false);
				stage1.SetActive (true);
				if (activeSceneName.Equals (StaticGameInfo.TASK_1)) {
					SetHint (StaticGameInfo.HINT_T1_AFTER_S1_PICKUP);
				} else if (activeSceneName.Equals (StaticGameInfo.TASK_2)) {
					SetHint (StaticGameInfo.HINT_T2_AFTER_S1_PICKUP);
				} else if (activeSceneName.Equals (StaticGameInfo.TASK_3)) {
					SetHint (StaticGameInfo.HINT_T3_AFTER_S1_PICKUP);
				}
				break;
			case "Stage1CorrectOption":
				stage2PickUp.SetActive (true);
				stage1IncorrectOption1.SetActive (false);
				stage1IncorrectOption2.SetActive (false);
				gameObject.SetActive (false);
				SetHint (StaticGameInfo.DEFAULT_HINT);
				break;
			case "Stage1IncorrectOption1":
			case "Stage2IncorrectOption1":
				StaticGameInfo.EndGame (false, exitObjects, arObjects);
//				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
//				stage2PickUp.SetActive (true);
//				stage1CorrectOption.SetActive (false);
//				stage1IncorrectOption2.SetActive (false);
//				gameObject.SetActive (false);
//				SetHint (StaticGameInfo.DEFAULT_HINT);
				break;
			case "Stage1IncorrectOption2":
			case "Stage2IncorrectOption2":
				StaticGameInfo.EndGame (false, exitObjects, arObjects);
//				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
//				stage2PickUp.SetActive (true);
//				stage1CorrectOption.SetActive (false);
//				stage1IncorrectOption1.SetActive (false);
//				gameObject.SetActive (false);
//				SetHint (StaticGameInfo.DEFAULT_HINT);
				break;
			case "Task3CorrectOption1":
				stage2PickUp.SetActive (true);
				stage1.SetActive (false);
				SetHint (StaticGameInfo.DEFAULT_HINT);
				break;
			case "Stage2PickUp":
				stage2PickUp.SetActive (false);
				stage2.SetActive (true);
				if (activeSceneName.Equals (StaticGameInfo.TASK_3)) {
					SetHint (StaticGameInfo.HINT_T3_AFTER_S2_PICKUP);
				}
				break;
			case "Task3CorrectOption2":
				StaticGameInfo.EndGame (true, exitObjects, arObjects);
				stage2.SetActive (false);
				SetHint (StaticGameInfo.LEVEL_COMPLETE);
				break;
		}
	}

	public void SetHint(string hintStr) {
		hintText.text = hintStr;
		StaticGameInfo.hint = hintStr;
	}

}