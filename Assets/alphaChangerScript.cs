using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class alphaChangerScript : MonoBehaviour
{
    public GameObject[] uiObjects;
    public SpriteRenderer[] backgroundSprites;
    SpriteRenderer sr;

    public float[] noiseSpeed;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;
    float[] noiseOffsetX;
    float[] noiseOffsetY;
    public int index;
    private int i;
    public UnityEngine.UI.Slider slider;
    public float fractionOfImagesToSelect;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        noiseOffsetX = new float[backgroundSprites.Length];
        noiseOffsetY = new float[backgroundSprites.Length];
        noiseSpeed = new float[backgroundSprites.Length];
        //first take a random point on the perlin noise

        SelectImages();
    }

    void Update()
    {
        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            float noiseStep = Mathf.PerlinNoise(Time.time * noiseSpeed[i] + noiseOffsetX[i], Time.time * noiseSpeed[i] + noiseOffsetY[i]);

            // float t = (float)i / (backgroundSprites.Length - 1);
            // float depthWeight = Mathf.Lerp(1f, 0.3f, t);

            //now we will use the noise as the step value for the lerp fn
            float noiseTarget = Mathf.Lerp(slider.value, maxAlpha, noiseStep);//*depthWeight;
            Color color = backgroundSprites[i].color;
            color.a = Mathf.Lerp(color.a, noiseTarget, Time.deltaTime);
            backgroundSprites[i].color = color;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShowUI();
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Screen touched");
                ShowUI();
            }
        }

    }
    public void SelectImages()
    {
        for (i = 0; i < backgroundSprites.Length; i++)
        {
            noiseOffsetX[i] = UnityEngine.Random.Range(0f, 100f);
            noiseOffsetY[i] = UnityEngine.Random.Range(0f, 100f);
            Color color = backgroundSprites[i].color;
            color.a = 0f; ;
            backgroundSprites[i].color = color;
            noiseSpeed[i] = UnityEngine.Random.Range(0.05f, 0.15f);
            if (UnityEngine.Random.value < (1 - fractionOfImagesToSelect))
            {
                backgroundSprites[i].enabled = false;
            }
            else
            {
                backgroundSprites[i].enabled = true;
            }


        }
    }
    public void HideUI()
    {
        for (int i = 0; i < uiObjects.Length; i++)
        {
            if (uiObjects[i] != null)
                uiObjects[i].SetActive(false);
        }
    }

    public void ShowUI()
    {

        for (int i = 0; i < uiObjects.Length; i++)
        {
            if (uiObjects[i] != null)
                uiObjects[i].SetActive(true);
        }

    }
}
