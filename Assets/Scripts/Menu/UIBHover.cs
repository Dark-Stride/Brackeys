using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //change color
    public Image BGImage;
    public Color normalColor = Color.white;
    public Color HoverColor = Color.green;

    //text color
    public Text btnText;
    public Color normalTextColor = Color.black;
    public Color HoverTextColor = Color.black;

    // glow
    public Outline outline;
    public Color glowColor = Color.blue;
    public float pulseSpeed = 2f;

    //scale
    public float ScaleRate = 1.5f;
    private Color originalGlowColor;
    private bool isHovered = false;
    private Vector3 originalScale;


    void Start()
    {
        if (outline != null)
            originalGlowColor = outline.effectColor;

        originalScale = transform.localScale;

        if (BGImage != null)
            BGImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        //change color
        if (BGImage != null) BGImage.color = HoverColor;
        btnText.color = HoverTextColor;

        // scale up
        transform.localScale = originalScale * ScaleRate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        //change color
        if (BGImage != null) BGImage.color = normalColor;
        btnText.color = normalTextColor;

        // scale up
        transform.localScale = originalScale;
    }

    void Update()
    {
        if (outline != null)
        {
            if (isHovered)
            {
                float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
                outline.effectColor = Color.Lerp(originalGlowColor, glowColor, pulse);
            }
            else
            {
                // return to original glow color
                outline.effectColor = Color.Lerp(outline.effectColor, originalGlowColor, Time.deltaTime * 5f);
            }
        }
    }
}
