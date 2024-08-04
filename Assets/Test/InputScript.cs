using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("键值A");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("按下键值A");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("按上键值A");
        }
    }
}