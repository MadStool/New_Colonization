using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWallet : MonoBehaviour
{
    private int _countPoints;
    public int CurrentPoints => _countPoints;

    public event Action<int> PointsChanged;

    public void AddPoint()
    {
        _countPoints++;
        PointsChanged?.Invoke(_countPoints);
    }

    public bool TrySpend(int amount)
    {
        if (_countPoints >= amount)
        {
            _countPoints -= amount;
            PointsChanged?.Invoke(_countPoints);
            return true;
        }

        return false;
    }
}