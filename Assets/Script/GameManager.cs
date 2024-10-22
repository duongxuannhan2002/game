using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Timeend;
    public bool isEnd = false;

    public bool isWin = false;

    public bool isLose = false;

    DatabaseManager databaseManager;

    void Start()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        if(PlayerPrefs.GetInt("Map")==0){
            Timeend = 71f;
        }else
        if(PlayerPrefs.GetInt("Map")==1){
            Timeend = 96f;
        }else
        if(PlayerPrefs.GetInt("Map")==2){
            Timeend = 81f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CavasInGame cavasInGame = FindObjectOfType<CavasInGame>();
        if (cavasInGame.isStart && PlayerPrefs.GetInt("GameMode") == 1)
        {
            Timeend -= Time.deltaTime;
            if (Timeend <= 0)
            {
                EndGame();
                isLose = true;
            }
        }
    }

    public void EndGame()
    {
        isEnd = true;
        FindObjectOfType<CavasInGame>().isStart = false;
        List<int> mapUnlock = databaseManager.GetMapUnlock(1);
        if (!mapUnlock.Contains(PlayerPrefs.GetInt("Map") + 1))
        {
            if (PlayerPrefs.GetInt("GameMode") == 1)
            {
                databaseManager.UpdateChallenge(PlayerPrefs.GetInt("Map"), 1);
            }
            else
            {
                databaseManager.UpdateChallenge(PlayerPrefs.GetInt("Map"), 2);
            }
            List<int> listTT = databaseManager.GetChallenge(PlayerPrefs.GetInt("Map"));
            // Debug.Log(listTT[0] + "" + listTT[1]);
            if (listTT[0] == 1 && listTT[1] == 1)
            {
                Debug.Log("chay trong day");
                databaseManager.unlockMap(PlayerPrefs.GetInt("Map") + 1);
            }
        }
    }
}
