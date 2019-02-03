using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Stage1ObjectController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	public GameObject stage2PickUp;
	public GameObject stage2;
	public GameObject player;
	public GameObject stage2CorrectObject;
	public GameObject stage2IncorrectObject1;
	public GameObject stage2IncorrectObject2;
	public Text titleText;
	public Text hintText;
	public GameObject exitObjects;
	public GameObject arObjects;

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
		if (!player.activeSelf) {
			float moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			float moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rb.AddForce (movement * speed);
		}
	}

	void mobileMovement() {
		if (!player.activeSelf) {
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
		if (!player.activeSelf) {
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

	void OnTriggerEnter(Collider other) {
		string activeSceneName = SceneManager.GetActiveScene ().name;
		switch (other.gameObject.name) {
		case "Stage2PickUp":
			other.gameObject.SetActive (false);
			stage2.gameObject.SetActive (true);
			if (activeSceneName.Equals (StaticGameInfo.TASK_1)) {
				SetHint (StaticGameInfo.HINT_T1_AFTER_S2_PICKUP);
			} else if (activeSceneName.Equals (StaticGameInfo.TASK_2)) {
				SetHint (StaticGameInfo.HINT_T2_AFTER_S2_PICKUP);
			} else if (activeSceneName.Equals (StaticGameInfo.TASK_3)) {
				SetHint (StaticGameInfo.HINT_T3_AFTER_S2_PICKUP);
			}
			break;
		case "Stage2CorrectOption":
			Debug.Log ("Stage2Correct: Current Scene: "+StaticGameInfo.currentScene + ", currentTaskNumber: "+StaticGameInfo.currentTask);
			StaticGameInfo.EndGame (true, exitObjects, arObjects);
			stage2IncorrectObject1.gameObject.SetActive (false);
			stage2IncorrectObject2.gameObject.SetActive (false);
			gameObject.SetActive (false);
			ChangeColor ("Green");
			SetHint (StaticGameInfo.LEVEL_COMPLETE);
			break;
		case "Stage2IncorrectOption1":
			StaticGameInfo.EndGame (false, exitObjects, arObjects);
//			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
//			stage2CorrectObject.gameObject.SetActive (false);
//			stage2IncorrectObject2.gameObject.SetActive (false);
//			gameObject.SetActive (false);
//			ChangeColor ("Red");
//			SetHint (StaticGameInfo.DEFAULT_HINT);
			break;
		case "Stage2IncorrectOption2":
			StaticGameInfo.EndGame (false, exitObjects, arObjects);
//			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
//			stage2IncorrectObject1.gameObject.SetActive (false);
//			stage2CorrectObject.gameObject.SetActive (false);
//			ChangeColor ("Yellow");
//			gameObject.SetActive (false);
//			SetHint (StaticGameInfo.DEFAULT_HINT);
			break;
		}
	}

	void ChangeColor(string color) {
		if (StaticGameInfo.currentTask == 1) {
			gameObject.SetActive (true);
			stage2.gameObject.SetActive (false);
			Color newColor;
			if (color == "Green") {
				newColor = Color.green;
			} else if (color == "Red") {
				newColor = Color.red;
			} else {
				newColor = Color.yellow;
			}

			Material newMaterial = new Material(Shader.Find("Standard"));
			newMaterial.color = newColor;

			GameObject player = rb.gameObject;
			MeshRenderer gameObjectRenderer = player.GetComponent<MeshRenderer>();
			gameObjectRenderer.material = newMaterial ;
		}
	}

	public void SetHint(string hintStr) {
		hintText.text = hintStr;
		StaticGameInfo.hint = hintStr;
	}

}
