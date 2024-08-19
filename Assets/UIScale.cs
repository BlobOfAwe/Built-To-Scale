using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScale : MonoBehaviour
{
    [SerializeField] float[] needleAngles = new float[5];
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetAngle(0); // The player will always start at size 0
    }
    public void SetAngle(int playerSize)
    {
        rectTransform.eulerAngles = new Vector3(0, 0, needleAngles[playerSize + 2]);
    }
}
