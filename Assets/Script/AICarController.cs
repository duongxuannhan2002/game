using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform[] waypointsPath1;  // Tuyến đường 1
    public Transform[] waypointsPath2;  // Tuyến đường 2
    public Transform[] waypointsPath3;  // Tuyến đường 3
    public Transform[] waypointsPath4;  // Tuyến đường 4


    public float speed = 10f;
    public float turnSpeed = 5f;
    public float waypointRadius = 2f;

    public float detectionDistance = 20f;  // Khoảng cách để phát hiện vật cản
    public float avoidanceForce = 20f;     // Độ mạnh của lực tránh vật cản

    private Transform[] currentWaypoints;
    private int currentWaypointIndex = 0;

    private CavasInGame cavasInGame;  // Biến tham chiếu tới CanvasInGame

    void Start()
    {
        // Tìm và gán đối tượng CavasInGame
        cavasInGame = FindObjectOfType<CavasInGame>();

        // Kiểm tra xem có tìm được không
        if (cavasInGame == null)
        {
            Debug.LogError("Không tìm thấy CavasInGame trong scene.");
            return;
        }

        // Khi bắt đầu, ngẫu nhiên chọn một tuyến đường
        if (PlayerPrefs.GetInt("Map") == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                currentWaypoints = waypointsPath1;
            }
            else
            {
                currentWaypoints = waypointsPath2;
            }
        }
        else
        if (PlayerPrefs.GetInt("Map") == 1)
        {
            currentWaypoints = waypointsPath3;
        }
        else
        if (PlayerPrefs.GetInt("Map") == 2)
        {
            currentWaypoints = waypointsPath4;
        }

    }

    void Update()
    {
        // Kiểm tra nếu nút bắt đầu được bấm
        if (cavasInGame != null && cavasInGame.isStart)
        {
            // Di chuyển theo tuyến đường đã chọn
            Transform targetWaypoint = currentWaypoints[currentWaypointIndex];
            Vector3 direction = targetWaypoint.position - transform.position;
            direction.y = 0;

            // Kiểm tra vật cản
            if (DetectObstacle())
            {
                AvoidObstacle();
            }
            else
            {
                // Xoay xe và di chuyển theo waypoint
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointRadius)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % currentWaypoints.Length;
            }
        }
    }

    bool DetectObstacle()
    {
        // Raycast để phát hiện vật cản
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    void AvoidObstacle()
    {
        Vector3 avoidanceDirection = Vector3.Cross(transform.forward, Vector3.up).normalized;

        RaycastHit leftHit, rightHit;
        bool leftBlocked = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out leftHit, detectionDistance);
        bool rightBlocked = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out rightHit, detectionDistance);

        if (leftBlocked && !rightBlocked)
        {
            avoidanceDirection = transform.TransformDirection(Vector3.right);
        }
        else if (rightBlocked && !leftBlocked)
        {
            avoidanceDirection = transform.TransformDirection(Vector3.left);
        }
        else
        {
            avoidanceDirection = -transform.forward;
        }

        transform.Translate(avoidanceDirection * avoidanceForce * Time.deltaTime);
    }
}
