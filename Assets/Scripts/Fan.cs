using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fan : MonoBehaviour
{
    public bool awakeOn;

    public GameObject fanBlades;
    public ParticleSystem fanParticleSystem;
    public GameObject fanSwitch;
    public RawImage fanSpeedDialImg;
    public Canvas switchCanvas;
    public Wind wind;
    public Image speedLowImg;
    public Image speedMedImg;
    public Image speedHighImg;

    private Renderer switchRenderer;
    private Material switchOnMaterial;
    private Material switchOffMaterial;
    private Color switchOnColor;
    private Color switchOffColor;
    private Image switchImg;
    private Text switchText;

    private FanSpeed fanSpeed;
    private Color speedActiveColor;
    private Color speedInactiveColor;

    private bool on = false;

    private void Awake()
    {
        InitializeFanSwitches();

        fanSpeed = new FanSpeedLow();
        speedLowImg.color = speedActiveColor;

        if (awakeOn)
        {
            Switch();
        }
    }

    private void Update()
    {
        if (on)
        {
            RotateFan(fanSpeed);

            if (fanParticleSystem.gameObject.activeSelf == false)
            {
                fanParticleSystem.gameObject.SetActive(true);
            }
        }
        else if (!on)
        {
            if (fanParticleSystem.gameObject.activeSelf == true)
            {
                fanParticleSystem.gameObject.SetActive(false);
            }
        }
    }

    public void Switch()
    {
        on = !on;

        if (on)
        {
            switchText.text = "On";
            switchRenderer.material = switchOnMaterial;
            switchImg.color = switchOnColor;
            fanSwitch.transform.Rotate(Vector3.up, 90f);
        }
        else
        {
            switchText.text = "Off";
            switchRenderer.material = switchOffMaterial;
            switchImg.color = switchOffColor;
            fanSwitch.transform.Rotate(Vector3.up, -90f);
        }
    }

    public void ChangeSpeed()
    {
        Quaternion dialRotation = Quaternion.Euler(0f, 0f, 0f);

        if (fanSpeed is FanSpeedLow)
        {
            fanSpeed = new FanSpeedMed();
            speedMedImg.color = speedActiveColor;
            wind.Force = 100000f;

            speedLowImg.color = speedInactiveColor;
            speedHighImg.color = speedInactiveColor;
        }
        else if (fanSpeed is FanSpeedMed)
        {
            fanSpeed = new FanSpeedHigh();
            speedHighImg.color = speedActiveColor;
            wind.Force = 150000f;

            speedMedImg.color = speedInactiveColor;
            speedLowImg.color = speedInactiveColor;
        }
        else
        {
            fanSpeed = new FanSpeedLow();
            speedLowImg.color = speedActiveColor;
            wind.Force = 50000f;

            speedMedImg.color = speedInactiveColor;
            speedHighImg.color = speedInactiveColor;
        }

        dialRotation = fanSpeed.DialRotation;
        SpeedDialPosition(dialRotation);
    }

    private void SpeedDialPosition(Quaternion dialRotation)
    {
        fanSpeedDialImg.transform.localRotation = dialRotation;
    }

    private void RotateFan(FanSpeed fanSpeed)
    {
        // One 360 degree rotation per second, muliplied by speed (ie. 4 means 4 360 degree rotations per second)
        fanBlades.transform.Rotate(Vector3.forward, (Time.deltaTime * 360) * fanSpeed.Speed);
    }

    private void InitializeFanSwitches()
    {
        switchOnMaterial = Resources.Load<Material>("Materials/SwitchOn");
        switchOffMaterial = Resources.Load<Material>("Materials/SwitchOff");

        switchRenderer = fanSwitch.GetComponent<Renderer>();
        switchRenderer.material = switchOffMaterial;

        switchImg = switchCanvas.GetComponentInChildren<Image>();
        switchText = switchCanvas.GetComponentInChildren<Text>();

        switchOnColor = new Color32(122, 212, 111, 150);
        switchOffColor = new Color32(255, 69, 0, 150);

        speedInactiveColor = new Color32(0, 12, 90, 200);
        speedActiveColor = new Color32(0, 3, 255, 200);
    }
}
