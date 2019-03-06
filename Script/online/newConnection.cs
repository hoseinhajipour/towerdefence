using UnityEngine;
using System.Collections;
using SocketIO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class newConnection : MonoBehaviour
{
    public SocketIOComponent socket;
    string check = "";
    string room_name = "room0";
    match_result_data match_Result_Data=new match_result_data();
    public float timeout_find_battle = 5.0f;
    private float next_try_find;


    PlayerInfo own_info;
    PlayerInfo enemy_info;

    public PlayerInfo current_user =new PlayerInfo();
    public GameController gameController;
    public ShowerGenarator ShowerGenarator_;
    public SoldierGenerator SoldierGenerator_;
    void Start()
    {
        StartCoroutine(connect());
        match_Result_Data.type = "";
        socket.On("Connect", reciveCon);
        socket.On("PLAY", OnPlay);
        socket.On("ATTACKO", onOtherAttack);
        socket.On("find_match_player", onfind_match_player);
    }

    IEnumerator wait3()
    {
        yield return new WaitForSeconds(2f);
    }
    IEnumerator connect()
    {
        yield return new WaitForSeconds(0.5f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = "check";
        socket.Emit("Connect", new JSONObject(data));
        yield return new WaitForSeconds(0.5f);
        if (check == "check")
        {
            Debug.Log("Connected To The Server!");
            onPlay();
        }
        else
        {
            Debug.Log("Connecting...");
            refresh();
        }

    }

    public void onPlay()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = PlayerPrefs.GetString("username");
        socket.Emit("PLAY", new JSONObject(data));
    }
    void OnPlay(SocketIOEvent evt)
    {
        Debug.Log("ready for play");
        Debug.Log(evt.data);
        current_user.id= int.Parse(evt.data.GetField("id").ToString());
        current_user.name = evt.data.GetField("name").ToString();
    
        find_match_player();
    }

    void reciveCon(SocketIOEvent evt)
    {
        check = jsonToString(evt.data.GetField("name").ToString(), "\"");
    }

    public void attackReq(attack_info attack_Info_)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        string attack_Info_str = JsonUtility.ToJson(attack_Info_, true);
        Debug.Log(attack_Info_str);
        socket.Emit("attack", new JSONObject(attack_Info_str));
    }
    void onOtherAttack(SocketIOEvent evt)
    {
        Debug.Log("new other attack");
        Debug.Log(evt.data);
        attack_info attack_Info_ = new attack_info();
        attack_Info_.name =  jsonToString(evt.data.GetField("name").ToString(), "\"");
        attack_Info_.tag =  jsonToString(evt.data.GetField("tag").ToString(), "\"");
        attack_Info_.position =jsonToString(evt.data.GetField("position").ToString(), "\"");

        if (attack_Info_.name == "Shower")
        {
            ShowerGenarator_.directCreate(attack_Info_);
        }else if (attack_Info_.name == "Soldier")
        {
            SoldierGenerator_.directCreate(attack_Info_);
        }
    }


    public void find_match_player()
    {
        next_try_find = Time.time + timeout_find_battle;
        StartCoroutine(do_find_match_player());
    }
    IEnumerator do_find_match_player()
    {
        yield return new WaitForSeconds(0.5f);
        string reulttoJason = JsonUtility.ToJson(match_Result_Data, true);
      //  Debug.Log(reulttoJason);
        socket.Emit("find_match_player", new JSONObject(reulttoJason));
        yield return new WaitForSeconds(0.5f);
        if (match_Result_Data.type == "battle_start")
        {
            Debug.Log("battle start !!");
            prepare_game_secne();
        }
        else
        {

            if (Time.time > next_try_find)
            {
                Debug.Log("try find battle time out");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["room_name"] = match_Result_Data.room_name;
                socket.Emit("endbattle", new JSONObject(data));
                SceneManager.LoadScene("menu");
            }
            else
            {
                Debug.Log("try find battle");
                try_match();
            }
        }

    }
    void try_match()
    {
        if (match_Result_Data.userlist ==null)
        {
            if (match_Result_Data.type == "wait_for_player")
            {
                StartCoroutine(do_find_match_player());
            }
        }
        else
        {
            match_Result_Data.type = "battle_start";
            Debug.Log("battle start !!");
            prepare_game_secne();
        }
    }
    void onfind_match_player(SocketIOEvent evt)
    {
        
        match_Result_Data.type= jsonToString(evt.data.GetField("type").ToString(), "\"");
        match_Result_Data.room_name = jsonToString(evt.data.GetField("room_name").ToString(), "\"");

        var data = JSON.Parse(evt.data.ToString());

        if (data["userlist"].Count > 0)
        {
            match_Result_Data.userlist = new List<PlayerInfo>();
            for (int i=0;i< data["userlist"].Count; i++)
            {
                PlayerInfo temp = new PlayerInfo();
                temp.id = data["userlist"][i]["id"];
                temp.socketID = data["userlist"][i]["socketID"];
                temp.name = data["userlist"][i]["name"];
                temp.room_name = data["userlist"][i]["room_name"];
                match_Result_Data.userlist.Add(temp);
            }
            Debug.Log(match_Result_Data.userlist);

        }
        Debug.Log(evt.data);

    }
    


    void refresh()
    {
        if (check == "")
        {
            StartCoroutine(connect());
        }
    }


    public void do_end_ballte()
    {
        StartCoroutine(endBattle());
    }
    IEnumerator endBattle()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("do end battle");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["room_name"] = match_Result_Data.room_name;

        socket.Emit("endbattle", new JSONObject(data));
        SceneManager.LoadScene("menu");

    }
    public Vector3 JsonToVec(string target)
    {
        Vector3 newvector;
        string[] newS = Regex.Split(target, ",");
        newvector = new Vector3(float.Parse(newS[0]), float.Parse(newS[1]), float.Parse(newS[2]));
        return newvector;
    }
    void onupdaterooms(SocketIOEvent evt)
    {

        Debug.Log(evt.data);
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

    void prepare_game_secne()
    {
       if(match_Result_Data.userlist[0].name == PlayerPrefs.GetString("username")){

            gameController.own_info = match_Result_Data.userlist[0];
            gameController.enemy_info = match_Result_Data.userlist[1];
            gameController.i_am_a= "own";
        }
        else
        {
            gameController.own_info = match_Result_Data.userlist[1];
            gameController.enemy_info = match_Result_Data.userlist[0];
            gameController.i_am_a = "enemy";
        }
        gameController.player_name.text = gameController.own_info.name;
        gameController.enemy_name.text = gameController.enemy_info.name;
        gameController.room_name = match_Result_Data.room_name;
        gameController.hide_find_player_match_panel();
    }
}
