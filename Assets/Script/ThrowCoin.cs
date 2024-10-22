using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCoin : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource engineSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // Hủy đối tượng đồng coin khi xe đi qua
            Destroy(gameObject);
            FindObjectOfType<DatabaseManager>().UpdateAddMoney();
            engineSound.Play();
        }
    }
}
