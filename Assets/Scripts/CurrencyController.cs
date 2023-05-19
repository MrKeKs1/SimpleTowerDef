using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;
using TheSTAR.Utility;
using Zenject;

public class CurrencyController : MonoBehaviour
{
    private List<ITransactionReactable> _transactionReactables;

    [Inject] private readonly DataController _data;

    public void Init(List<ITransactionReactable> trs)
    {
        _transactionReactables = trs;
        InitReaction();
    }

    public void AddCurrency(CurrencyType currencyType, int count = 1, bool autoSave = true)
    {
        _data.gameData.battleData.AddCurrency(currencyType, count, out int result);
        if (autoSave) _data.Save();

        Reaction(currencyType, result);
    }

    public void ReduceCurrency(CurrencyType currencyType, int count = 1, bool autoSave = true, Action completeAction = null, Action failAction = null)
    {
        if (_data.gameData.battleData.GetCurrencyCount(currencyType) >= count)
        {
            _data.gameData.battleData.AddCurrency(currencyType, -count, out int result);
            if (autoSave) _data.Save();

            completeAction?.Invoke();

            Reaction(currencyType, result);
        }
        else failAction?.Invoke();
    }

    public void ClearCurrency(CurrencyType currencyType)
    {
        var count = GetCurrencyValue(currencyType);
        _data.gameData.battleData.AddCurrency(currencyType, -count, out var result);

        Reaction(currencyType, result);
    }

    public int GetCurrencyValue(CurrencyType currencyType)
    {
        return _data.gameData.battleData.GetCurrencyCount(currencyType);
    }

    private void InitReaction()
    {
        var currencyTypes = EnumUtility.GetValues<CurrencyType>();

        foreach (var tr in _transactionReactables)
            foreach (var currencyType in currencyTypes)
                tr.OnTransactionReact(currencyType, _data.gameData.battleData.GetCurrencyCount(currencyType));
    }

    private void Reaction(CurrencyType currencyType, int finalValue)
    {
        foreach (var tr in _transactionReactables) tr.OnTransactionReact(currencyType, finalValue);
    }
}

public interface ITransactionReactable
{
    void OnTransactionReact(CurrencyType currencyType, int finalValue);
}

public enum CurrencyType
{
    Coin
}