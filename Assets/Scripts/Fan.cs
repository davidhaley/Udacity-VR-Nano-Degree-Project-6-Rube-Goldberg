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
    public AudioSource audioSourceFan;
    public AudioSource audioSourceSwitch;

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

    private AudioClip fanSwitchOn;
    private AudioClip fanSwitchOff;

    private bool on = false;

    private void Awake()
    {
        InitializeFanPanelUI();
        SetupAudioSources();

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

            OnSwitchAudio();
        }
        else
        {
            switchText.text = "Off";
            switchRenderer.material = switchOffMaterial;
            switchImg.color = switchOffColor;
            fanSwitch.transform.Rotate(Vector3.up, -90f);

            OffSwitchAudio();
        }
    }

    public void ChangeSpeed()
    {
        Quaternion dialRotation = Quaternion.Euler(0f, 0f, 0f);

        if (fanSpeed is FanSpeedLow)
        {
            fanSpeed = new FanSpeedMed();
            speedMedImg.color = speedActiveColor;
            wind.Force = 125000f;

            speedLowImg.color = speedInactiveColor;
            speedHighImg.color = speedInactiveColor;
        }
        else if (fanSpeed is FanSpeedMed)
        {
            fanSpeed = new FanSpeedHigh();
            speedHighImg.color = speedActiveColor;
            wind.Force = 175000f;

            speedMedImg.color = speedInactiveColor;
            speedLowImg.color = speedInactiveColor;
        }
        else
        {
            fanSpeed = new FanSpeedLow();
            speedLowImg.color = speedActiveColor;
            wind.Force = 75000f;

            speedMedImg.color = speedInactiveColor;
            speedHighImg.color = speedInactiveColor;
        }

        ChangeSpeedFanAudio(fanSpeed);
        dialRotation = fanSpeed.DialRotation;
        SpeedDialPosition(dialRotation);
    }

    private void OnSwitchAudio()
    {
        audioSourceSwitch.clip = fanSwitchOn;
        audioSourceSwitch.Play();

        ChangeSpeedFanAudio(fanSpeed);
    }

    public void OffSwitchAudio()
    {
        audioSourceSwitch.clip = fanSwitchOff;
        audioSourceSwitch.Play();

        audioSourceFan.Stop();
    }

    public void ChangeSpeedFanAudio(FanSpeed fanSpeed)
    {
        audioSourceFan.clip = fanSpeed.audioClip;

        if (on)
        {
            audioSourceFan.Play();
        }
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

    private void SetupAudioSources()
    {
        fanSwitchOn = Resources.Load<AudioClip>("Sounds/Fan/FanSwitchOn");
        fanSwitchOff = Resources.Load<AudioClip>("Sounds/Fan/FanSwitchOff");

        audioSourceFan.loop = true;
        audioSourceFan.volume = 0.30f;
        audioSourceFan.playOnAwake = false;

        audioSourceSwitch.loop = false;
        audioSourceSwitch.volume = 0.20f;
        audioSourceSwitch.playOnAwake = false;
    }

    private void InitializeFanPanelUI()
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
