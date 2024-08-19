using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScale : MonoBehaviour
{
    [SerializeField] float[] needleAngles = new float[5]; // What angle should the needle be at for each player size
    [SerializeField] float fadeAlpha = 0.5f; // How transparent does the UI become when the player is overlapping it
    private RectTransform rectTransform; // The RectTransform component of the Needle
    private RectTransform scaleTransform; // The RectTransform component of the Scale
    private Transform player; // The worldspace transform component of the player
    [SerializeField] Vector3 playerScreenPoint; // The screenpoint position of the player, taken from worlspace
    [SerializeField] Vector3[] scaleAnchorPoints = new Vector3[2]; // The position of the scale's anchor points in Screenpoint space
   
    private Camera cam; // The main rendering camera

    private UnityEngine.UI.Image needleImage; // A reference to the needle's UI sprite
    private UnityEngine.UI.Image scaleImage; // A reference to the scale's UI sprite
    private Color transparentColor; // The color for the UI when it is overlapped by the player
    private Color opaqueColor; // The color for the UI when it is not overlapped by the player

    private void Start()
    {
        // Assign the appropriate variables
        player = FindAnyObjectByType<PlayerController>().transform;
        cam = FindAnyObjectByType<Camera>();
        rectTransform = GetComponent<RectTransform>();
        scaleTransform = transform.parent.GetComponent<RectTransform>();
        needleImage = GetComponent<UnityEngine.UI.Image>();
        scaleImage = transform.parent.GetComponent<UnityEngine.UI.Image>();
        
        
        opaqueColor = scaleImage.color; // Set the opaque color to be the UI element's starting color
        transparentColor = opaqueColor; // Set the transparent color to match the opaque color as a baseline
        transparentColor.a = fadeAlpha; // Modify the transparent color's Alpha channel to match fadeAlpha

        SetAngle(0); // Set the needle to the default angle, The player will always start at size 0


    }
    private void Update()
    {
        // Identify the screenpoint position of the player, as well as the scale's maximum and minimum anchors
        playerScreenPoint = cam.WorldToScreenPoint(player.position);
        scaleAnchorPoints[0] = cam.ViewportToScreenPoint(scaleTransform.anchorMin);
        scaleAnchorPoints[1] = cam.ViewportToScreenPoint(scaleTransform.anchorMax);

        // If the player is within the scale's anchor points, make the UI elements transparent
        if (playerScreenPoint.x > scaleAnchorPoints[0].x && playerScreenPoint.x < scaleAnchorPoints[1].x && playerScreenPoint.y > scaleAnchorPoints[0].y && playerScreenPoint.y < scaleAnchorPoints[1].y)
        {
            needleImage.color = transparentColor;
            scaleImage.color = transparentColor;
        }
        
        // Otherwise, make them opaque
        else
        {
            needleImage.color = opaqueColor;
            scaleImage.color = opaqueColor;
        }
    }
    public void SetAngle(int playerSize)
    {
        rectTransform.eulerAngles = new Vector3(0, 0, needleAngles[playerSize + 2]);
    }
}
