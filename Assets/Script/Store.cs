using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CarStore : MonoBehaviour
{
    public GameObject[] allCars;  // Mảng chứa tất cả các xe (Prefab)
    public TextMeshProUGUI carNameText;      // Text hiển thị tên xe
    public TextMeshProUGUI carPriceText;     // Text hiển thị giá xe
    public TextMeshProUGUI Monney;     // Text hiển thị tiền

    public TextMeshProUGUI noti;     // Text hiển thị thông báo


    public Transform spawnPosition; // Vị trí mà xe sẽ xuất hiện
    private GameObject currentCarInstance; // Xe hiện tại đang được hiển thị
    public Button buyButton;      // Nút mua xe
    public Button leftButton;     // Nút sang trái
    public Button rightButton;    // Nút sang phải

    private int currentCarIndex = 0;  // Chỉ số xe hiện tại
    private List<int> unlockedCarIds; // Danh sách ID các xe đã mở khóa

    public DatabaseManager dbManager;   // Quản lý kết nối CSDL
    public int playerId = 1;  // ID của người chơi

    void Start()
    {
        dbManager = FindObjectOfType<DatabaseManager>(); // Tìm đối tượng DatabaseManager

        if (dbManager == null)
        {
            Debug.LogError("DatabaseManager không được tìm thấy!");
            return;
        }

        // Gán sự kiện cho nút trái và phải
        leftButton.onClick.AddListener(OnLeftButtonPressed);
        rightButton.onClick.AddListener(OnRightButtonPressed);

        // Bắt đầu coroutine để lấy danh sách xe đã mở khóa với độ trễ
        StartCoroutine(LoadUnlockedCars());
    }

    private IEnumerator LoadUnlockedCars()
    {
        // Hiển thị thông báo đang tải (nếu cần)
        Debug.Log("Đang tải danh sách xe đã mở khóa...");

        // Tạo độ trễ trước khi lấy danh sách xe đã mở khóa
        yield return new WaitForSeconds(0.1f); // Thay đổi thời gian theo ý muốn

        unlockedCarIds = dbManager.GetUnlockedCars(playerId);

        // Hiển thị xe đầu tiên
        ShowCar(currentCarIndex);
    }

    void Update(){
        int tien = dbManager.GetMonney();
        Monney.SetText(tien.ToString() + "$");
        unlockedCarIds = dbManager.GetUnlockedCars(playerId);
    }

    // Hàm hiển thị xe dựa trên chỉ số hiện tại
    void ShowCar(int index)
    {
        // Hủy xe cũ nếu đã có xe trên scene
        if (currentCarInstance != null)
        {
            Destroy(currentCarInstance);
        }

        // Cập nhật tên xe
        carNameText.text = allCars[index].name;

        // Kiểm tra nếu xe đã mở khóa
        if (unlockedCarIds.Contains(index))
        {
            carPriceText.gameObject.SetActive(false);  // Ẩn giá
            buyButton.gameObject.SetActive(false);     // Ẩn nút mua
        }
        else
        {
            // Hiển thị giá và nút mua cho xe chưa mở khóa
            carPriceText.gameObject.SetActive(true);
            carPriceText.text = "Giá: " + GetCarPrice(index).ToString() + "$";  // Lấy giá xe
            buyButton.gameObject.SetActive(true);
            buyButton.onClick.RemoveAllListeners();  // Xóa các sự kiện cũ
            buyButton.onClick.AddListener(() => BuyCar(index));  // Gán sự kiện mua xe
        }

        // Khởi tạo xe ở vị trí spawn
        currentCarInstance = Instantiate(allCars[index], spawnPosition.position, spawnPosition.rotation);
    }

    // Hàm khi nhấn nút trái
    void OnLeftButtonPressed()
    {
        currentCarIndex--; // Giảm chỉ số xe
        if (currentCarIndex < 0)
        {
            currentCarIndex = allCars.Length - 1; // Quay lại xe cuối cùng
        }
        ShowCar(currentCarIndex); // Hiển thị xe mới
    }

    // Hàm khi nhấn nút phải
    void OnRightButtonPressed()
    {
        currentCarIndex++; // Tăng chỉ số xe
        if (currentCarIndex >= allCars.Length)
        {
            currentCarIndex = 0; // Quay lại xe đầu tiên
        }
        ShowCar(currentCarIndex); // Hiển thị xe mới
    }

    // Hàm để lấy giá của xe (giả định)
    int GetCarPrice(int carIndex)
    {
        int price = dbManager.GetPrice(carIndex);
        return price;
    }

    // Hàm để mua xe
    void BuyCar(int carIndex)
    {
        int tien = dbManager.GetMonney();
        int price = GetCarPrice(carIndex);
        if(tien>price){
            ShowNotification("Thành Công");
            carPriceText.gameObject.SetActive(false);  // Ẩn giá
            buyButton.gameObject.SetActive(false);
            dbManager.AddCarForPlayer(carIndex);
            dbManager.UpdateMoney(tien-price); 
        }else{
            ShowNotification("Không đủ tiền");
        }
    }

    public void clickExit(){
        SceneManager.LoadScene(0);
    }

    public void ShowNotification(string message)
    {
        noti.text = message;
        noti.gameObject.SetActive(true); // Hiển thị Text
        StartCoroutine(HideNotificationAfterDelay());
    }

    // Coroutine để ẩn thông báo sau một khoảng thời gian
    private IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(2); // Đợi trong khoảng thời gian
        noti.gameObject.SetActive(false); // Ẩn Text
    }
}
