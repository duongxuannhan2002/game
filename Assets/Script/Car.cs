using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Car : MonoBehaviour
{
    [SerializeField] private float tocDoXe = 10f;
    [SerializeField] private float tocDoXoay = 100f;
    [SerializeField] private float tocDoPhanh = 2f;

    [SerializeField] public FixedJoystick jst;
    [SerializeField] public Button brakeButton;

    public AudioSource engineSound;

    public GameObject hieuUngPhanh;
    public Rigidbody rb;
    private bool isBraking = false;

    private CavasInGame cavasInGame;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on this car!");
        }
        cavasInGame = FindObjectOfType<CavasInGame>();
        int gameMode = PlayerPrefs.GetInt("GameMode");

        if (gameMode == 1)
        {
            // Cài đặt chế độ chơi đơn
            Debug.Log("Chế độ chơi đơn đã được chọn.");
            // SetupSinglePlayerMode();
            GameObject.Find("sedan 1").SetActive(false);
        }
        else if (gameMode == 2)
        {
            // Cài đặt chế độ chơi với AI
            Debug.Log("Chế độ chơi với AI đã được chọn.");
            // SetupAIMode();
        }

        if (PlayerPrefs.GetInt("Map") == 0)
        {
            GameObject.Find("Map").transform.Find("A").gameObject.SetActive(true);
            
        }
        else
        if (PlayerPrefs.GetInt("Map") == 1)
        {
            GameObject.Find("Map").transform.Find("B").gameObject.SetActive(true);
        }
        else
        if (PlayerPrefs.GetInt("Map") == 2)
        {
            GameObject.Find("Map").transform.Find("C").gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnBrakeButtonDown();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            OnBrakeButtonUp();
        }

        if (jst == null && GameObject.FindObjectOfType<FixedJoystick>().gameObject.activeSelf)
        {
            jst = FindObjectOfType<FixedJoystick>();
            Debug.Log("Joystick has been activated and assigned.");
        }

        if (brakeButton == null && GameObject.FindObjectOfType<Button>().gameObject.activeSelf)
        {
            brakeButton = FindObjectOfType<Button>();
            Debug.Log("brakebutton");
        }
        EventTrigger trigger = brakeButton.gameObject.GetComponent<EventTrigger>();

        if (brakeButton.gameObject.GetComponent<EventTrigger>() == null)
        {
            trigger = brakeButton.gameObject.AddComponent<EventTrigger>();
        }
        // Thêm EventTrigger cho nút phanh

        // Tạo sự kiện PointerDown
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnBrakeButtonDown(); });
        trigger.triggers.Add(pointerDownEntry);

        // Tạo sự kiện PointerUp
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnBrakeButtonUp(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned!");
            return;
        }
        if (cavasInGame.isStart)
        {
            moveCar();
        }

    }

    private void moveCar()
{
    float horizontal = jst.Horizontal;
    float vertical = jst.Vertical;

    float adjustedSpeed = isBraking ? tocDoPhanh : tocDoXe;

    // Di chuyển xe
    Vector3 moveDirection = transform.forward * adjustedSpeed * vertical;
    rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

    // Điều khiển góc quay xe
    if (horizontal != 0)
    {
        float angle = horizontal * tocDoXoay * Time.deltaTime;
        transform.Rotate(0, angle, 0);
    }

    // Điều khiển âm thanh động cơ dựa trên tốc độ di chuyển
    if (vertical != 0)
    {
        if (!engineSound.isPlaying)
        {
            engineSound.Play();
        }

        // Điều chỉnh cao độ của âm thanh dựa trên tốc độ xe
        engineSound.pitch = 1 + (Mathf.Abs(vertical) * 0.1f); // Tăng pitch theo tốc độ
    }
    else
    {
        // Nếu xe không di chuyển, giảm âm thanh động cơ
        engineSound.pitch = Mathf.Lerp(engineSound.pitch, 0.5f, Time.deltaTime * 2);

        // Dừng âm thanh khi xe dừng
        if (engineSound.pitch <= 0.5f && engineSound.isPlaying)
        {
            engineSound.Stop();
        }
    }
}

    public void OnBrakeButtonDown()
    {
        Debug.Log("Đã nhấn nút phanh");
        isBraking = true;
        hieuUngPhanh.SetActive(true);
    }

    public void OnBrakeButtonUp()
    {
        Debug.Log("Đã thả nút phanh");
        isBraking = false;
        hieuUngPhanh.SetActive(false);
    }
}
