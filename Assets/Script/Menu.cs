using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject Htmenu;
    [SerializeField] private GameObject Htmap;
    [SerializeField] private GameObject Htchallenge;
    [SerializeField] private GameObject Htsetting;
    [SerializeField] private GameObject Htexit;

    public Button[] mapButtons;
    public Button[] challengeButtons;
    public DatabaseManager dbManager;
    List<int> mapUnlock;

    void Start()
    {
        dbManager = FindObjectOfType<DatabaseManager>();

        // Gán sự kiện cho mỗi nút trong mapButtons
        for (int i = 0; i < mapButtons.Length; i++)
        {
            int index = i; // Lưu chỉ số của button
            mapButtons[i].onClick.AddListener(() => OnMapButtonClicked(index));
        }
    }

    public void clickPVE()
    {
        Htmenu.SetActive(false);
        Htmap.SetActive(true);
        mapUnlock = dbManager.GetMapUnlock(1);
        for (int i = 0; i < mapButtons.Length; i++)
        {
            if (mapUnlock.Contains(i))
            {
                mapButtons[i].GetComponent<Image>().color = Color.green; // Nút màu xanh nếu mở khóa
            }
            else
            {
                mapButtons[i].GetComponent<Image>().color = Color.red;   // Nút màu đỏ nếu chưa mở
            }
        }
        Htexit.SetActive(true);
    }

    // Hàm này sẽ được gọi khi mỗi button được nhấn, và bạn sẽ biết button nào được nhấn qua index
    public void OnMapButtonClicked(int index)
    {
        if (mapUnlock.Contains(index))
        {
            PlayerPrefs.SetInt("Map", index);
            Htmap.SetActive(false);
            Htchallenge.SetActive(true);
            List<int> listCl = dbManager.GetChallenge(index);
            if(listCl[0]==1){
                challengeButtons[0].GetComponent<Image>().color = Color.green;
            }
            if(listCl[1]==1){
                challengeButtons[1].GetComponent<Image>().color = Color.green;
            }
        }
        else
        {

        }
    }

    public void OnSinglePlayerButton()
    {
        PlayerPrefs.SetInt("GameMode", 1); // 1 là chế độ chơi đơn
        SceneManager.LoadScene(1); // Chuyển tới màn hình chơi game
    }

    public void OnAIModeButton()
    {
        PlayerPrefs.SetInt("GameMode", 2); // 2 là chế độ chơi với AI
        SceneManager.LoadScene(1); // Chuyển tới màn hình chơi game
    }

    public void clickPVP()
    {
        SceneManager.LoadScene(2);
    }

    public void clickSetting()
    {
        Htsetting.SetActive(true);
        Htmenu.SetActive(false);
        Htexit.SetActive(true);
    }

    public void clickStore()
    {
        SceneManager.LoadScene(3);
    }

    public void clickExit()
    {
        SceneManager.LoadScene(0);
    }
}
