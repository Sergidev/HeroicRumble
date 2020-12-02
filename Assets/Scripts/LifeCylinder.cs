using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCylinder : MonoBehaviour
{
    public float TotalLife;
    public float CurrentLife;

    private MeshRenderer content;

    [SerializeField]
    private Color high = Color.green;

    [SerializeField]
    private Color low = Color.red;

    [SerializeField]
    private bool lerpColors = true;

    public float MaxValue { get; set; }

    private float scaleMax { get; set; }

    public bool DecreaseLife;
    public bool MantainScale;
    public float dmg;

    void Update()
    {
        HandleBar(Map(CurrentLife, 0, TotalLife, 0, 1));
        if (DecreaseLife) CurrentLife -= dmg;
    }

    void Start()
    {
        scaleMax = this.transform.localScale.y;
        content = this.GetComponent<MeshRenderer>();   
    }

    private void HandleBar(float c)
    {
        if (!MantainScale) this.transform.eulerAngles = new Vector3(0, 0, 90);

        if(CurrentLife > 0)
        {
            if (!MantainScale) this.transform.localScale = new Vector3(this.transform.localScale.x, c * scaleMax, this.transform.localScale.z);

            if (lerpColors)
            {
                content.materials[0].color = Color.Lerp(low, high, c);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
