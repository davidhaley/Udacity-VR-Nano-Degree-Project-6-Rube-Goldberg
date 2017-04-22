using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fan : MonoBehaviour {

    public GameObject fanBlades;
    public ParticleSystem fanParticleSystem;
    public GameObject fanSwitch;
    public Image panelImage;
    public bool awakeOn;

    private bool on;

    private float speed = 4f;

    private Renderer switchRenderer;
    private Material onMaterial;
    private Material offMaterial;
    private Color onColor;
    private Color offColor;

    private void Awake()
    {
        switchRenderer = fanSwitch.GetComponent<Renderer>();
        onMaterial = Resources.Load<Material>("Materials/SwitchOn");
        offMaterial = Resources.Load<Material>("Materials/SwitchOff");

        on = false;
        switchRenderer.material = offMaterial;

        onColor = new Color32(122, 212, 111, 150);
        offColor = new Color32(255, 69, 0, 150);

        if (awakeOn)
        {
            Switch();
        }
    }

    private void Update()
    {
        if (on)
        {
            // One 360 degree rotation per second, muliplied by speed (ie. 4 means 4 360 degree rotations per second)
            fanBlades.transform.Rotate(Vector3.forward, (Time.deltaTime * 360) * speed);

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
        Debug.Log("hand clicked switch");
        on = !on;

        if (on)
        {
            switchRenderer.material = onMaterial;
            panelImage.color = onColor;
            fanSwitch.transform.Rotate(Vector3.up, 90f);
        }
        else
        {
            switchRenderer.material = offMaterial;
            panelImage.color = offColor;
            fanSwitch.transform.Rotate(Vector3.up, -90f);
        }
    }
}
