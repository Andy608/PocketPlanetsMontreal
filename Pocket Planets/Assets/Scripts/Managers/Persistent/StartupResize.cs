using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupResize : MonoBehaviour
{
	void Start ()
    {
        Screen.SetResolution(443, 960, FullScreenMode.Windowed, 60);
    }
}
