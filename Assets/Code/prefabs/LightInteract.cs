using UnityEngine;

public class LightInteract : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float lightIntensity = 1f;
    [SerializeField] private float lightRange = 1f;
    [SerializeField] private Color lightColor = Color.white;

    [SerializeField] private Light lightComponent;

    private void Start()
    {
        InitializeLight();
    }

    private void InitializeLight()
    {
        lightIntensity = lightComponent.intensity;
        lightRange = lightComponent.range;
        lightColor = lightComponent.color;
    }

    public void SetLightIntensity(float intensity)
    {
        lightComponent.intensity += intensity;
    }

    public void SetLightRange(float range)
    {
        lightComponent.range += range;
    }

    public void SetLightColor(Color color)
    {
        lightComponent.color = color;
    }
    public void TurnOnLight()
    {
        lightComponent.enabled = lightComponent.enabled ? false : true;
        Debug.Log("Light is on: " + lightComponent.enabled);
    }
}
