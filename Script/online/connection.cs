using UnityEngine;
using System.Collections;
using SocketIO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class connection : MonoBehaviour {
	public SocketIOComponent socket;
    public GameController gameController;
    string check="";

    public PlayerInfo own_info;
    public PlayerInfo enemy_info;

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
        socket.On("find_match_player", otherfind_match_player);
        socket.On("find_match_player_not", otherfind_match_player_not);
    }
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
            onPlay();
        } else {
			Debug.Log ("Connecting...");
			refresh();
		}

	}
	void reciveCon(SocketIOEvent evt){
		check = jsonToString (evt.data.GetField ("name").ToString (), "\"");
	}
	
    void refresh(){
		if (check == "") {
			StartCoroutine (connect ());
		} 
	}
    public void onPlay()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = PlayerPrefs.GetString("username");
        socket.Emit("PLAY", new JSONObject(data));
    }
    public void findmatchplayer(){
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = own_info.id.ToString();
        data["socketID"] = own_info.socketID;
        socket.Emit("find_match_player", new JSONObject(data));
    }
	void OnPlay(SocketIOEvent evt){
        Debug.Log(evt.data);
        Debug.Log(jsonToString(evt.data.GetField("name").ToString(), "\"") + " Welcome to Game!");
        own_info.name = jsonToString(evt.data.GetField("name").ToString(), "\"");
        own_info.socketID = jsonToString(evt.data.GetField("socketID").ToString(), "\"");
        own_info.id = int.Parse(evt.data.GetField("id").ToString());
        findmatchplayer();
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

    void otherfind_match_player(SocketIOEvent evt)
    {
       // gameController.
        Debug.Log("battle ready");
      //  Debug.Log(jsonToString(" match_player_id : " + evt.data.GetField("match_player_id").ToString(), "\""));

        enemy_info.id = int.Parse(evt.data.GetField("id").ToString());
        enemy_info.name = jsonToString(evt.data.GetField("name").ToString(), "\"");
        enemy_info.socketID = jsonToString(evt.data.GetField("socketID").ToString(), "\"");

        gameController.setEnemyInfo(enemy_info);
        gameController.i_am_a = jsonToString(evt.data.GetField("owner").ToString(), "\"");
        gameController.hide_find_player_match_panel();


    }

    void otherfind_match_player_not(SocketIOEvent evt)
    {
        Debug.Log("find match player not");
        Debug.Log(evt.data);
        SceneManager.LoadScene("menu");
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


    string jsonToStr(string target)
    {
        string s = "\"";
        string[] neS = Regex.Split(target, s);
        return neS[1];
    }

    string jsonToString(string target, string s)
    {
        string[] neS = Regex.Split(target, s);
        return neS[1];
    }
    int jsonToInt(string target)
    {
        string s = "\"";
        string[] neS = Regex.Split(target, s);
        return int.Parse(neS[1]);
    }
    bool jsonToBool(string target)
    {
        string s = "\"";
        string[] neS = Regex.Split(target, s);
        return bool.Parse(neS[1]);
    }
}
 