using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image img;

    public Image Img => img;

    public void SetValue(float value) => img.fillAmount = value;
}