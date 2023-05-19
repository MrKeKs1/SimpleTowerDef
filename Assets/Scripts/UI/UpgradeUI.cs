using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility.Pointer;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private PointerButton buyButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI costText;

    private UpgradeType _upgradeType;

    public void Init(UpgradeType upgradeType, string name, Action<UpgradeType> clickAction)
    {
        _upgradeType = upgradeType;
        title.text = name;

        buyButton.Init(() => clickAction.Invoke(_upgradeType));
    }

    public void SetCost(int cost) => costText.text = $"$ {cost}";

    public void SetAvailable(bool available) => canvasGroup.alpha = available ? 1 : 0.5f;
}