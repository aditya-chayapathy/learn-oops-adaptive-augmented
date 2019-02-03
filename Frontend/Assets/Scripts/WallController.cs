using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallController : MonoBehaviour {

	public Text collisionCountText;
	public Text timeTakenText;
	public GameObject averageRepresentation;
	public GameObject goodRepresentation;
	public GameObject badRepresentation;
	private long baseTime;

	// Use this for initialization
	async void Start () {
		//UnityEngine.Debug.Log ("WallControllerScript executing!");
		StaticGameInfo.noOfWallCollisions = 0;
		baseTime = await getStatsForComparison();
		baseTime = baseTime / 1000;
	}

	void Update () {
		if(StaticGameInfo.stopWatch != null) {
			long timeElapsed = (long)StaticGameInfo.stopWatch.ElapsedMilliseconds / 1000;
			//UnityEngine.Debug.Log ("Time elapsed: " + timeElapsed.ToString());
			StaticGameInfo.timeTaken = timeElapsed;
			timeTakenText.text = StaticGameInfo.FIRST_PART_OF_TIME_TAKEN_TEXT + timeElapsed.ToString() + "s";
		}
		if (StaticGameInfo.timeTaken < baseTime) {
			averageRepresentation.SetActive (false);
			goodRepresentation.SetActive (true);
			badRepresentation.SetActive (false);
		} else if (StaticGameInfo.timeTaken > baseTime) {
			averageRepresentation.SetActive (false);
			goodRepresentation.SetActive (false);
			badRepresentation.SetActive (true);
		} else {
			averageRepresentation.SetActive (true);
			goodRepresentation.SetActive (false);
			badRepresentation.SetActive (true);
		}
	}

	void OnTriggerEnter(Collider other) {
		//other.isTrigger = false;
		StaticGameInfo.noOfWallCollisions++;
		UnityEngine.Debug.Log ("Collisions: " + StaticGameInfo.noOfWallCollisions);
		collisionCountText.text = StaticGameInfo.FIRST_PART_OF_COLLISION_COUNT_TEXT + StaticGameInfo.noOfWallCollisions;
	}

	public async Task<long> getStatsForComparison () {
		
		if (StaticGameInfo.httpClient == null) {
			StaticGameInfo.httpClient = new HttpClient();
		}
		JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
		string settingsJson = null;
		//bool status = false;
		HttpResponseMessage response = await StaticGameInfo.httpClient.GetAsync(StaticGameInfo.url+"user/stats/getEndOfSceneStats?userId="
			+StaticGameInfo.userName+"&taskNumber="+StaticGameInfo.currentTask);
		if (response.IsSuccessStatusCode) {
			settingsJson = await response.Content.ReadAsStringAsync ();
			dynamic dobj = jsonSerializer.Deserialize<dynamic> (settingsJson);
			long baseTime = dobj ["averageGlobalSuccessfulCompletionTime"];
			return baseTime;
		} else {
			UnityEngine.Debug.Log ("Failed");
			return 0;
		}
	}
}
