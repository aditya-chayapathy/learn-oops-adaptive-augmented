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
using System.Diagnostics;

public static class StaticGameInfo {

	public const string TASK_1 = "Task 1";
	public const string TASK_2 = "Task 2";
	public const string TASK_3 = "Task 3";
	public const string TASK_4 = "Task 4";

	public static string userName;
	public static int currentTask = 1;
	public static string currentScene = TASK_1;
	public static int speed = 12;

	public const string DEFAULT_HINT = "Pickup the object visible!";
	public const string HINT_T1_AFTER_S1_PICKUP = "Instantiate into an object of type Cube!";
	public const string HINT_T2_AFTER_S1_PICKUP = "Choose the base class for human evolution!";
	public const string HINT_T3_AFTER_S1_PICKUP = "A house has a _____";
	public const string HINT_T4_AFTER_S1_PICKUP = "Choose different objects to see how the same area method is called!";
	public const string HINT_T1_AFTER_S2_PICKUP = "Set the object's color attribute to Green!";
	public const string HINT_T2_AFTER_S2_PICKUP = "Choose the derived class for given object!";
	public const string HINT_T3_AFTER_S2_PICKUP = "A house has a _____";
	public const string LEVEL_COMPLETE = "Congratulations! Task completed.";
	public const string FIRST_PART_OF_COLLISION_COUNT_TEXT = "Wall Collisions = ";
	public const string FIRST_PART_OF_TIME_TAKEN_TEXT = "Time taken = ";

	public static string hint = "Pickup the object visible!";

	public static long timeTaken = 0;
	public static bool complete = false;
	public static int noOfWallCollisions = 0;

	public static string url = "http://54.201.122.204:8080/";

	public static Stopwatch stopWatch = null;

	public static HttpClient httpClient = null;

	public static void EndGame(bool status, GameObject exitObjects, GameObject arObjects) {
		UnityEngine.Debug.Log ("In EndGame: Current Scene: "+StaticGameInfo.currentScene + ", currentTaskNumber: "+StaticGameInfo.currentTask);
		StaticGameInfo.stopWatch.Stop ();
		long timeElapsed = (long) StaticGameInfo.stopWatch.ElapsedMilliseconds;
		StaticGameInfo.timeTaken = timeElapsed;
		StaticGameInfo.complete = status;
		setEndOfSceneStats (exitObjects);
		writeToLog ();
		arObjects.SetActive (false);
		exitObjects.SetActive (true);

	}

	public static async void writeToLog () {
		if (StaticGameInfo.httpClient == null) {
			httpClient = new HttpClient();
		}
		UnityEngine.Debug.Log ("In writeLog: Current Scene: "+StaticGameInfo.currentScene + ", currentTaskNumber: "+StaticGameInfo.currentTask);
		HttpResponseMessage response = await httpClient.PostAsJsonAsync(StaticGameInfo.url+"user/logs/addUserLog?" +
			"speed="+StaticGameInfo.speed+"&numberOfWallCollisions="+StaticGameInfo.noOfWallCollisions+"&taskNumber="+StaticGameInfo.currentTask
			+"&userId="+StaticGameInfo.userName+"&isSuccess="+StaticGameInfo.complete+"&timeTaken="+StaticGameInfo.timeTaken,new MultipartContent());
		response.EnsureSuccessStatusCode();
		await response.Content.ReadAsAsync<UnityEngine.Object>();
	}

	public static async void setEndOfSceneStats(GameObject exitObjects) {
		if (StaticGameInfo.httpClient == null) {
			httpClient = new HttpClient();
		}
		JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
		string settingsJson = null;
		//bool status = false;
		HttpResponseMessage response = await httpClient.GetAsync(StaticGameInfo.url+"user/stats/getEndOfSceneStats?userId="
			+StaticGameInfo.userName+"&taskNumber="+StaticGameInfo.currentTask);
		if (response.IsSuccessStatusCode) {
			settingsJson = await response.Content.ReadAsStringAsync ();
			dynamic dobj = jsonSerializer.Deserialize<dynamic> (settingsJson);
			populateStatsBox (exitObjects, dobj ["averageGlobalSuccessfulCompletionTime"], dobj ["averageUserSuccessfulCompletionTime"], 
				dobj ["previousCompletionTime"], dobj ["previousSuccessfulCompletionTime"]);
		} else {
			UnityEngine.Debug.Log ("Failed");
		}

	}

	public static void populateStatsBox(GameObject exitObjects, long averageGlobalSuccessfulCompletionTime, long averageUserSuccessfulCompletionTime, 
		long previousCompletionTime, long previousSuccessfulCompletionTime) {
		Text taskEndingStatsText = exitObjects.transform.GetChild (0).GetComponent<Text> ();
		string taskEndingStatsString = "End of task statistics\n\n";
		taskEndingStatsString += "Average timetaken (all users) : " + averageGlobalSuccessfulCompletionTime/1000 + " s\n" 
			+ "Average timetaken ("+StaticGameInfo.userName+"): " + averageUserSuccessfulCompletionTime/1000 + " s\n"
			+ "Previous attempt: " + previousCompletionTime/1000 + " s\n" 
			+ "Previous successful attempt: " + previousSuccessfulCompletionTime/1000 + " s\n"
			+ "Timetaken for current attempt: " + StaticGameInfo.timeTaken/1000 + " s";
		taskEndingStatsText.text = taskEndingStatsString;
	}
//
//	public const Dictionary<int, string> levelToSceneDict = new Dictionary<int, string>() {
//		{1, "Task 1"},
//		{2, "Task 2"},
//		{3, "Task 3"},
//		{4, "Task 4"}
//	};
//
//	public const Dictionary<string, int> sceneToLevelDict = new Dictionary<int, string>() {
//		{"Task 1", 1},
//		{"Task 2", 2},
//		{"Task 3", 3},
//		{"Task 4", 4}
//	};
//
}