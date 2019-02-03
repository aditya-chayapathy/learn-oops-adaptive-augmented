//using System;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptToTrigger : MonoBehaviour {

	public GameObject userInfoUI;

	void Start() {
		UnityEngine.Debug.Log ("Script to Trigger called!");
		DisplayUserInfoUI ();
	}

	public void ShowObject(GameObject gameObjectArg) {
		//Debug.Log ("Showing GO: " + gameObjectArg.name);
		gameObjectArg.SetActive (true);
		UpdateSomething (gameObjectArg);
	}

	public void HideObject(GameObject gameObjectArg) {
		//Debug.Log ("Hiding GO: " + gameObjectArg.name);
		gameObjectArg.SetActive (false);
	}

	public void DisplayUserInfoUI () {
		//Make a rest call here to fetch the details or send details from login()
		userInfoUI.SetActive (true);
		Text userName = userInfoUI.transform.GetChild (0).GetComponent<Text> ();
		userName.text = StaticGameInfo.userName;
	}

	public void UpdateSomething(GameObject gameObjectArg) {
		switch (gameObjectArg.name) {
		case "Hint Text":
			Text hintText = gameObjectArg.GetComponent<Text> ();
			hintText.text = StaticGameInfo.hint;
			break;
		}
	}

	public void StartTimer() {
		StaticGameInfo.stopWatch = new Stopwatch ();
		StaticGameInfo.stopWatch.Restart ();
		UnityEngine.Debug.Log ("Stopwatch started!");
	}

	public void RestartScene() {
		StaticGameInfo.complete = false;
		StaticGameInfo.hint = StaticGameInfo.DEFAULT_HINT;
		SceneManager.LoadScene (StaticGameInfo.currentScene);
	}

	public void StartNextScene() {
		int nextTask = StaticGameInfo.currentTask + 1;
		string nextScene = "Task " + nextTask;
		StaticGameInfo.currentTask = nextTask;
		StaticGameInfo.currentScene = nextScene;
		StaticGameInfo.complete = false;
		StaticGameInfo.hint = StaticGameInfo.DEFAULT_HINT;
		SceneManager.LoadScene (nextScene);
	}

	public void StartSceneAfterSpeedSet() {
		StaticGameInfo.complete = false;
		StaticGameInfo.hint = StaticGameInfo.DEFAULT_HINT;
		SceneManager.LoadScene (StaticGameInfo.currentScene);
	}

}