using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PVP : MonoBehaviour
{
    [SerializeField] private GameObject HtCreateRoom;

    [SerializeField] private GameObject HtSettingRoom;

    public void clickCreate(){
        HtCreateRoom.SetActive(false);
        HtSettingRoom.SetActive(true);
    }

    public void clickStart(){

    }

    public void clickExit(){
        SceneManager.LoadScene(0);
    }
}
