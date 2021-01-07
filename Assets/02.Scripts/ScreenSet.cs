using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSet : MonoBehaviour
{
    public int size;
    public RectTransform[] rect;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Screen.SetResolution(1280, 800, true);

    }

    // Update is called once per frame
    void Update()
    {
        rect[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / size);
        rect[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / size);
    }
}
