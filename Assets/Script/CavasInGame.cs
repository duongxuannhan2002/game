using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class CavasInGame : MonoBehaviour
{
    [SerializeField] public Button startButton;
    [SerializeField] public GameObject pauseOption;
    [SerializeField] public GameObject inPause;
    [SerializeField] public TextMeshProUGUI countdownText; // Text UI để hiển thị đếm ngược

    [SerializeField] public TextMeshProUGUI timedownText; // Text UI để hiển thị đếm ngược

    [SerializeField] public TextMeshProUGUI coin; // Text UI để hiển thị tiền
    private DatabaseManager dbManager;

    public bool isStart = false;
    public void Start(){
        dbManager = FindObjectOfType<DatabaseManager>(); // Tìm đối tượng DatabaseManager
    }


    public void Update()
    {
        int tien = dbManager.GetMonney();
        coin.SetText(tien.ToString() + "$");
        if (isStart && PlayerPrefs.GetInt("GameMode")==1)
        {
            disPlayTimeDown();
        }
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.isEnd)
        {
            Debug.Log("chay khi thua");
            Transform parentTransform = GameObject.Find("Canvas").transform;
            Transform child = parentTransform.Find("InPause");
            child.gameObject.SetActive(true);
            child.transform.Find("ButtonContinue").gameObject.SetActive(false);
            GameObject.Find("Control").SetActive(false);
        }
        if (gameManager.isLose)
        {
            timedownText.SetText("THUA");
        }
        if (gameManager.isWin)
        {
            timedownText.SetText("THẮNG");
        }
    }

    public void clickStart()
    {
        startButton.gameObject.SetActive(false); // Ẩn nút start
        StartCoroutine(StartCountdown()); // Bắt đầu Coroutine đếm ngược
    }

    private IEnumerator StartCountdown()
    {
        GameObject.Find("SelectCar").SetActive(false);
        Transform parentTransform = GameObject.Find("Canvas").transform;
        Transform child = parentTransform.Find("Control");
        child.gameObject.SetActive(true);
        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString(); // Hiển thị số đếm ngược
            yield return new WaitForSeconds(1f); // Chờ 1 giây
            countdown--;
        }

        countdownText.text = "Go!"; // Hiển thị "Go!"
        yield return new WaitForSeconds(1f); // Chờ thêm 1 giây

        countdownText.gameObject.SetActive(false); // Ẩn text đếm ngược
        isStart = true;
        pauseOption.SetActive(true);
    }

    private void disPlayTimeDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        timedownText.SetText(Mathf.FloorToInt(gameManager.Timeend).ToString());
    }

    public void clickPause()
    {
        pauseOption.SetActive(false);
        inPause.SetActive(true);
        isStart = false;
    }

    public void clickCon()
    {
        pauseOption.SetActive(true);
        inPause.SetActive(false);
        isStart = true;
    }

    public void clickRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void clickExit()
    {
        SceneManager.LoadScene(0);
    }
}
