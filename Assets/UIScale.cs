using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScale : MonoBehaviour
{
    [SerializeField] float[] needleAngles = new float[5];
    [SerializeField] float fadeAlpha = 0.5f;
    private RectTransform rectTransform;
    private Transform player;
    private Camera cam;
    private Canvas canvas;

    private UnityEngine.UI.Image needleImage;
    private UnityEngine.UI.Image scaleImage;
    private Color transparentColor;
    private Color opaqueColor;
    private bool isOpaque = true;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        cam = FindAnyObjectByType<Camera>();
        canvas = FindAnyObjectByType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        needleImage = GetComponent<UnityEngine.UI.Image>();
        scaleImage = transform.parent.GetComponent<UnityEngine.UI.Image>();
        opaqueColor = scaleImage.color;
        transparentColor = opaqueColor;
        transparentColor.a = fadeAlpha;
        SetAngle(0); // The player will always start at size 0
        isOpaque = true;


    }
    private void Update()
    {

        
    }
    public void SetAngle(int playerSize)
    {
        rectTransform.eulerAngles = new Vector3(0, 0, needleAngles[playerSize + 2]);
    }
}
