using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCheck : MonoBehaviour
{
   
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Đã chạm vào: " + other.name); 
        // Kiểm tra nếu đối tượng va chạm có tag là "Finish"
        if (other.CompareTag("Car"))
        {
            // isWin = true;
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.isWin = true;
            gameManager.EndGame();
        }

        if (other.CompareTag("AI")){
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.isLose = true;
            gameManager.EndGame();
        }
    }
}
