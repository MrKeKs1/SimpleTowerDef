using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : Bar
{
    private float maxHp;
    private float currentHp;

    public void SetHp(float currentHp, float maxHp)
    {
        this.currentHp = currentHp;
        this.maxHp = maxHp;

        UpdateUI();
    }

    private void UpdateUI() => SetValue(currentHp / maxHp);
}