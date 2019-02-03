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

public class LoginScript : MonoBehaviour {

	public GameObject loginObjectUI;

	public GameObject registerObjectUI;

	public GameObject userInfoUI;

	public GameObject loginErrorText;

	private HttpClient client = new HttpClient();

	public async void Login () {
		Debug.Log ("Login method!");
		string userName = loginObjectUI.transform.GetChild (0).GetComponent<InputField> ().text;
		string password = loginObjectUI.transform.GetChild (1).GetComponent<InputField> ().text;
		if (await validateLogin (userName, password)) {
			Debug.Log ("Login Successful!");
			loginObjectUI.SetActive (false);
			DisplayUserInfoUI (userName);
			populateStaticInfo (userName);
			await setGameSettings (userName);
			Debug.Log ("Current Scene: " + StaticGameInfo.currentScene + ", currentTaskNumber: " + StaticGameInfo.currentTask);
			SceneManager.LoadScene (StaticGameInfo.currentScene);
		} else {
			loginErrorText.SetActive (true);	
		}
	}

	public async void Register () {
		string userName = registerObjectUI.transform.GetChild (0).GetComponent<InputField> ().text;
		string password = registerObjectUI.transform.GetChild (1).GetComponent<InputField> ().text;
		Dropdown levelOfExpertise = registerObjectUI.transform.GetChild (2).GetComponent<Dropdown> ();
		int level = levelOfExpertise.value + 1;
		Debug.Log ("Level" + level);
		UserDomain userDomain = await addUser(userName, password, level);
		Debug.Log ("UserDomain values: "+userDomain.userId+userDomain.levelOfExpertise);
		Debug.Log ("Register Successful!");
		registerObjectUI.SetActive (false);
		DisplayUserInfoUI (userName);
		populateStaticInfo (userName);
		StaticGameInfo.currentScene = "SpeedAdjustmentScene";
		StaticGameInfo.currentTask = 0;
		//await setGameSettings (userName);
		Debug.Log ("Current Scene: "+StaticGameInfo.currentScene + ", currentTaskNumber: "+StaticGameInfo.currentTask);
		SceneManager.LoadScene (StaticGameInfo.currentScene);
	}

	public void DisplayUserInfoUI (string userName) {
		//Make a rest call here to fetch the details or send details from login()
		userInfoUI.SetActive (true);
		Text userNameText = userInfoUI.transform.GetChild (0).GetComponent<Text> ();
		userNameText.text = userName;
	}

	public void populateStaticInfo (string userName) {
		StaticGameInfo.userName = userName;
	}

	public async Task<bool> validateLogin (string username, string password) {
		bool isValid = true;
		HttpResponseMessage response = await client.GetAsync(StaticGameInfo.url+"user/checkIfValidCredentials?userId="+username+"&password="+password);
		if (response.IsSuccessStatusCode)
		{
			isValid = await response.Content.ReadAsAsync<bool>();
		}
		return isValid;
	}

	public async Task<UserDomain> addUser (string username, string password, int level) {
		UserDomain userDomain = null;
		HttpResponseMessage response = await client.PostAsJsonAsync(StaticGameInfo.url+"user/addUser?userId="+username+"&password="+password+"&levelOfExpertise="+level, new MultipartContent());
		response.EnsureSuccessStatusCode();
		userDomain = await response.Content.ReadAsAsync<UserDomain>();
		return userDomain;
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

}
