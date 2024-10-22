using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thêm thư viện để làm việc với UI
using Cinemachine;

public class CarSwitcher : MonoBehaviour
{
    public GameObject[] allCars;               // Mảng chứa tất cả các xe (Prefab)
    private List<GameObject> unlockedCars;     // Danh sách các xe đã mở khóa
    private int currentCarIndex = 0;           // Chỉ số xe hiện tại
    private GameObject currentCarInstance;     // Xe hiện tại
    public Transform spawnPosition;            // Vị trí spawn cố định

    // Các tham chiếu cần thiết cho điều khiển
    public FixedJoystick joystick;             // Joystick dùng để điều khiển xe
    public Button brakeButton;                 // Nút phanh
    public GameObject hieuUngPhanh;            // Hiệu ứng phanh

    public DatabaseManager dbManager;   // Quản lý kết nối CSDL
    public int playerId = 1;                   // ID của người chơi (ví dụ: PlayerId=1)

    void Start()
{
    dbManager = FindObjectOfType<DatabaseManager>(); // Tìm đối tượng DatabaseManager

    if (dbManager == null)
    {
        Debug.LogError("DatabaseManager not found!");
        return;
    }

    // Lấy danh sách xe đã mở khóa
    LoadUnlockedCars();

    if (unlockedCars.Count > 0)
    {
        SpawnCar(currentCarIndex);         // Spawn xe đầu tiên trong danh sách xe đã mở khóa
    }
    else
    {
        Debug.LogError("No unlocked cars found for the player.");
    }
}

    // Hàm để tải danh sách các xe đã mở khóa từ cơ sở dữ liệu
    void LoadUnlockedCars()
    {
        unlockedCars = new List<GameObject>();

        // Lấy danh sách ID xe đã mở khóa từ CSDL
        List<int> unlockedCarIds = dbManager.GetUnlockedCars(playerId);

        // Tạo danh sách Prefab các xe đã mở khóa dựa trên ID
        foreach (int carId in unlockedCarIds)
        {
            // Giả sử ID của các xe trùng với chỉ số của prefab trong mảng allCars
            if (carId >= 0 && carId < allCars.Length)
            {
                unlockedCars.Add(allCars[carId]);
            }
        }
    }

    // Hàm để chuyển sang xe tiếp theo
    public void OnNextCarButtonPressed()
    {
        SwitchCar(1);                          // Chuyển sang xe tiếp theo trong danh sách đã mở khóa
    }

    // Hàm để quay lại xe trước đó
    public void OnPreviousCarButtonPressed()
    {
        SwitchCar(-1);                         // Quay lại xe trước đó trong danh sách đã mở khóa
    }

    // Hàm để chuyển đổi xe
    void SwitchCar(int direction)
    {
        if (currentCarInstance != null)
        {
            Destroy(currentCarInstance);       // Xóa xe hiện tại
        }

        // Cập nhật chỉ số xe hiện tại (thêm hoặc bớt xe dựa trên direction)
        currentCarIndex = (currentCarIndex + direction + unlockedCars.Count) % unlockedCars.Count;

        SpawnCar(currentCarIndex);             // Spawn xe mới
    }

    // Hàm spawn xe
    void SpawnCar(int index)
    {
        // Tạo xe mới từ danh sách xe đã mở khóa
        currentCarInstance = Instantiate(unlockedCars[index], spawnPosition.position, spawnPosition.rotation);

        // Tương tự các phần xử lý khác như tìm PointFollow, gán Cinemachine, và gán script Car như code của bạn
        // (code này vẫn giữ nguyên như bạn đã viết)
        Transform pointFollow = currentCarInstance.transform.Find("PointFollow");
        if (pointFollow != null)
        {
            // Lấy Cinemachine Virtual Camera
            CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                // Gán pointfollow vào Follow của virtual camera
                vcam.Follow = pointFollow;
                Debug.Log("Pointfollow successfully assigned to the virtual camera!");
            }
            else
            {
                Debug.LogError("No Cinemachine Virtual Camera found in the scene!");
            }
        }
        else
        {
            Debug.LogError("Pointfollow not found in the car prefab!");
        }

        // Lấy script Car từ xe mới tạo
        Car carController = currentCarInstance.GetComponent<Car>();
        if (carController != null)
        {
            // Tìm đối tượng con có tên "Effect" bên trong xe clone mới
            GameObject effectObject = currentCarInstance.transform.Find("Effect")?.gameObject;
            if (effectObject != null)
            {
                // Gán đối tượng Effect vào hieuUngPhanh của carController
                carController.hieuUngPhanh = effectObject;
            }
            else
            {
                Debug.LogError("Effect GameObject not found in the car prefab!");
            }

            // Gán Rigidbody cho carController
            Rigidbody newRb = currentCarInstance.GetComponent<Rigidbody>();
            if (newRb != null)
            {
                carController.rb = newRb;
                Debug.Log("Rigidbody successfully assigned!");
            }
            else
            {
                Debug.LogError("Rigidbody not found on the car prefab!");
            }
        }
        else
        {
            Debug.LogError("Car script not found on the car prefab!");
        }
    }
}

// public class CarSwitcher : MonoBehaviour
// {
//     public GameObject[] cars;                 // Mảng chứa các xe
//     private int currentCarIndex = 0;          // Chỉ số xe hiện tại
//     private GameObject currentCarInstance;     // Xe hiện tại
//     public Transform spawnPosition;             // Vị trí spawn cố định

//     // Các tham chiếu cần thiết cho điều khiển
//     public FixedJoystick joystick;              // Joystick dùng để điều khiển xe
//     public Button brakeButton;                  // Nút phanh
//     public GameObject hieuUngPhanh;            // Hiệu ứng phanh

//     void Start()
//     {
//         SpawnCar(currentCarIndex);              // Spawn xe đầu tiên
//     }

//     // Hàm để chuyển sang xe tiếp theo
//     public void OnNextCarButtonPressed()
//     {
//         SwitchCar(1);                            // Chuyển sang xe tiếp theo
//     }

//     // Hàm để quay lại xe trước đó
//     public void OnPreviousCarButtonPressed()
//     {
//         SwitchCar(-1);                           // Quay lại xe trước đó
//     }

//     // Hàm để chuyển đổi xe
//     void SwitchCar(int direction)
//     {
//         if (currentCarInstance != null)
//         {
//             Destroy(currentCarInstance);         // Xóa xe hiện tại
//         }

//         // Cập nhật chỉ số xe hiện tại (thêm hoặc bớt xe dựa trên direction)
//         currentCarIndex = (currentCarIndex + direction + cars.Length) % cars.Length;

//         SpawnCar(currentCarIndex);               // Spawn xe mới
//     }

//     void SpawnCar(int index)
//     {
//         // Tạo xe mới
//         currentCarInstance = Instantiate(cars[index], spawnPosition.position, spawnPosition.rotation);
//         // Tìm đối tượng pointfollow trong xe mới tạo
//         Transform pointFollow = currentCarInstance.transform.Find("PointFollow");
//         if (pointFollow != null)
//         {
//             // Lấy Cinemachine Virtual Camera
//             CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
//             if (vcam != null)
//             {
//                 // Gán pointfollow vào Follow của virtual camera
//                 vcam.Follow = pointFollow;
//                 Debug.Log("Pointfollow successfully assigned to the virtual camera!");
//             }
//             else
//             {
//                 Debug.LogError("No Cinemachine Virtual Camera found in the scene!");
//             }
//         }
//         else
//         {
//             Debug.LogError("Pointfollow not found in the car prefab!");
//         }

//         // Lấy script Car từ xe mới tạo
//         Car carController = currentCarInstance.GetComponent<Car>();
//         if (carController != null)
//         {
//             // Tìm đối tượng con có tên "Effect" bên trong xe clone mới
//             GameObject effectObject = currentCarInstance.transform.Find("Effect")?.gameObject;
//             if (effectObject != null)
//             {
//                 // Gán đối tượng Effect vào hieuUngPhanh của carController
//                 carController.hieuUngPhanh = effectObject;
//             }
//             else
//             {
//                 Debug.LogError("Effect GameObject not found in the car prefab!");
//             }

//             // Gán Rigidbody cho carController
//             Rigidbody newRb = currentCarInstance.GetComponent<Rigidbody>();
//             if (newRb != null)
//             {
//                 carController.rb = newRb;
//                 Debug.Log("Rigidbody successfully assigned!");
//             }
//             else
//             {
//                 Debug.LogError("Rigidbody not found on the car prefab!");
//             }
//         }
//         else
//         {
//             Debug.LogError("Car script not found on the car prefab!");
//         }
//     }

// }
