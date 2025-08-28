using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWallet : MonoBehaviour
{
    private int _countPoints;
    public int CurrentPoints => _countPoints;

    public void AddPoint()
    {
        _countPoints++;
    }

    public bool TrySpend(int amount)
    {
        if (_countPoints >= amount)
        {
            _countPoints -= amount;

            return true;
        }

        return false;
    }
}