using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
            this.transform.Rotate(0, 70 * Time.deltaTime, 0);
            this.transform.position = GameManager.Instance.player.transform.position + new Vector3(0, 1, 0);
    }
}
