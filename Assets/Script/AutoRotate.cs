using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    // Tốc độ xoay
    public float rotationSpeed = 50f;

    void Update()
    {
        // Xoay đối tượng quanh trục Y (có thể thay đổi trục X hoặc Z tùy ý)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
