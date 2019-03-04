using UnityEngine;
using System.Collections;
using SocketIO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.UI;
public class connection : MonoBehaviour {
	public SocketIOComponent socket;
    string check="";
    // Use this for initialization

    void Start () {
		StartCoroutine (connect ());
		socket.On ("Connect", reciveCon);
		socket.On ("PLAY", OnPlay);
		socket.On ("OTHERPLAY", otherOnPlay);
		socket.On ("LIST", OnlineIns);
		socket.On ("LEFTTHESERVER", DestroyDisconnectPlayers);
		socket.On ("GETLIST", GetList);
        socket.On("SHOOTING", OtherShooting);
    }
	public
		int i;
	public void OnClickOnLineList(){
		socket.Emit ("SENDLIST");
	
		StartCoroutine (wait3 ());
	}
	void GetList(SocketIOEvent evt){

	}
	IEnumerator wait3(){
		yield return new WaitForSeconds (2f);
	}
	IEnumerator connect(){
		yield return new WaitForSeconds(0.5f);
		Dictionary<string,string> data = new Dictionary<string,string> ();
		data ["name"] = "check";
		socket.Emit("Connect",new JSONObject(data));
		yield return new WaitForSeconds(0.5f);
		if (check == "check") {
			Debug.Log ("Connected To The Server!");

		} else {
			Debug.Log ("Connecting...");
			refresh();
		}

	}
	void reciveCon(SocketIOEvent evt){
		check = jsonToString (evt.data.GetField ("name").ToString (), "\"");
	}
	string jsonToString(string target,string s){
		string[] neS = Regex.Split (target, s);
		return neS [1];
	}
	void refresh(){
		if (check == "") {
			StartCoroutine (connect ());
		} 
	}
	public void OnclickPlay(){
        Dictionary<string, string> data = new Dictionary<string, string>();
        Vector3 position = new Vector3(0, 0, 0);
        data["position"] = position.x + "," + position.y + "," + position.z;
        socket.Emit("PLAY", new JSONObject(data));
    }
	void OnPlay(SocketIOEvent evt){

		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Welcome to Game!");
	}
	void otherOnPlay(SocketIOEvent evt){
		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Appaned to Game!");
	}
	void OnlineIns(SocketIOEvent evt){
		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Appaned to Game!");
	}
    public void Shooting(Transform firepos, Quaternion firepos_rotation, string guntype,float bulletSpeed)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
    }

    void DestroyDisconnectPlayers(SocketIOEvent evt){
		Destroy (GameObject.Find(evt.data.GetField ("id").ToString ()));
	}
	
    void OtherShooting(SocketIOEvent evt)
    {
        Vector3 firepos=JsonToVec(jsonToString(evt.data.GetField("firepos").ToString(), "\""));
        Vector3 firerot_ = JsonToVec(jsonToString(evt.data.GetField("firerot").ToString(), "\""));
        Quaternion firerot = new Quaternion(firerot_.x, firerot_.y, firerot_.z,0);
        float bulletSpeed= float.Parse(jsonToString(evt.data.GetField("bulletSpeed").ToString(), "\""));


    }

    public Vector3 JsonToVec(string target){
		Vector3 newvector;
		string[] newS = Regex.Split (target, ",");
		newvector = new Vector3 (float.Parse (newS [0]), float.Parse (newS [1]), float.Parse (newS [2]));
		return newvector;
	}

}
 