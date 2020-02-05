using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsTiltControlActive : MonoBehaviour
{
    public bool isTiltControlActive = false;
    public void OnValueChanged(bool value)
    {
        GetComponent<Toggle>().isOn = value;
        TouchHandler.tiltControl = value;
        isTiltControlActive = value;
    }
}
