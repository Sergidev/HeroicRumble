using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LifeBar : MonoBehaviour
{
    public float TotalLife;
    public float CurrentLife;

    public TextMeshProUGUI LifeText;

    [SerializeField]
    private Image content = null;

    [SerializeField]
    private Color high = Color.green;

    [SerializeField]
    private Color low = Color.red;

    [SerializeField]
    private bool lerpColors = true;

    void Update()
    {
        HandleBar(Map(CurrentLife, 0, TotalLife, 0, 1));
        if (LifeText != null)
        {
            if (CurrentLife < 0) CurrentLife = 0;
            LifeText.text = (int)CurrentLife + "/" + (int)TotalLife;
        }       
    }

    void Start()
    {
        if (lerpColors)
        {
            content.color = high;
        }
    }

    private void HandleBar(float c)
    {
        if (CurrentLife > TotalLife) CurrentLife = TotalLife;

        content.fillAmount = c;

        if (lerpColors)
        {
            content.color = Color.Lerp(low, high, c);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
