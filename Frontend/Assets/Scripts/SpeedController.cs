using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeedController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	public Text titleText;
	public Text hintText;
	public Text userName;
	public GameObject exitObjects;
	public GameObject arObjects;
	public GameObject stage1PickUp;
	public GameObject stage2PickUp;
	public GameObject stage3PickUp;
	public GameObject stage4PickUp;
	private HttpClient client = new HttpClient();

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate() {
		mobileMovement();
		//keyboardMovement();
		//joystickMovement();
	}

	void keyboardMovement() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
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
		switch (other.gameObject.name) {
		case "Stage1PickUp":
			other.gameObject.SetActive (false);
			stage2PickUp.SetActive (true);
			break;
		case "Stage2PickUp":
			other.gameObject.SetActive (false);
			stage3PickUp.SetActive (true);
			break;
		case "Stage3PickUp":
			other.gameObject.SetActive (false);
			stage4PickUp.SetActive (true);
			break;
		case "Stage4PickUp":
			other.gameObject.SetActive (false);
			gameEndThings ();
			break;
		}
	}

	async void gameEndThings () {
		hintText.text = StaticGameInfo.LEVEL_COMPLETE;
		StaticGameInfo.EndGame (true, exitObjects, arObjects);
		bool statusDummy = await setGameSpeed (StaticGameInfo.userName, (int)StaticGameInfo.timeTaken);
		bool status = await setGameSettings (userName.text);
		if (!status) {
			Debug.Log ("Failed");
		}
	}

	public async Task<bool> setGameSettings (string username) {
		JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
		string settingsJson = null;
		bool status = false;
		HttpResponseMessage response = await client.GetAsync(StaticGameInfo.url+"user/getGameSettingsForUser?userId="+username);
		if (response.IsSuccessStatusCode)
		{
			settingsJson = await response.Content.ReadAsStringAsync();
			dynamic dobj = jsonSerializer.Deserialize<dynamic>(settingsJson);
			int speed = dobj ["speed"];
			Debug.Log ("getGameSettingsApi: "+speed);
			int taskNumber = dobj["taskNumber"];
			StaticGameInfo.currentScene = "Task " + taskNumber;
			StaticGameInfo.currentTask = taskNumber;
			StaticGameInfo.speed = speed;
			status = true;
		}
		return status;
	}

	public async Task<bool> setGameSpeed (string username, int timetaken) {
		HttpResponseMessage response = await client.PostAsJsonAsync(StaticGameInfo.url+"user/settings/setSpeedForUser?userId="+username+"&timeTaken="+timetaken, new MultipartContent());
		response.EnsureSuccessStatusCode();
		return true;
	}
}
