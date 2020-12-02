using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private static CameraRotate instance;
    public static CameraRotate Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CameraRotate>();
            }

            return instance;
        }
    }
    public bool CameraToggle = false;

    void Update()
    {
        if(CameraToggle)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(40, 0, 0), 0.05f);
        }
        else
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
        }
    }

    public void ToggleCamera()
    {
        CameraToggle = !CameraToggle;

        if(CameraToggle) GameManager.Instance.StartGame();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
