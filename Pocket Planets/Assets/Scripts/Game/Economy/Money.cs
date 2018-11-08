using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumEconomyLevel
{
    A, B, C, D, E, F, G, H
}

public class Money
{
    private float[] moneyLevels = new float[(int)EnumEconomyLevel.H];

    //Wallet Variables
    private EnumEconomyLevel currentLevel;
    private float primaryAmount;
    private float secondaryAmount;

    public Money(EnumEconomyLevel costLevel, float primaryCost, float secondaryCost)
    {
        SetMoney(costLevel, primaryCost, secondaryCost);
    }

    public Money()
    {
        ResetMoney();
    }

    private void SetMoney(EnumEconomyLevel costLevel, float primaryCost, float secondaryCost)
    {
        ResetMoney();

        moneyLevels[(int)costLevel] = primaryCost;

        if (costLevel > 0)
        {
            moneyLevels[(int)(costLevel - 1)] = secondaryCost;
        }

        UpdateWallet();
    }

    private void ResetMoney()
    {
        for (int i = 0; i < moneyLevels.Length; ++i)
        {
            moneyLevels[i] = 0;
        }

        currentLevel = EnumEconomyLevel.A;
        primaryAmount = 0;
        secondaryAmount = 0;
    }

    public void AddMoney(Money money)
    {
        for (int i = 0; i < money.moneyLevels.Length; ++i)
        {
            AddMoney((EnumEconomyLevel)i, money.moneyLevels[i]);
        }
    }

    public void AddMoney(EnumEconomyLevel level, float amount)
    {
        float currentBalance = moneyLevels[(int)level];
        float difference = currentBalance + amount;

        if (difference >= 100.0f)
        {
            float overflow = difference - 100.0f;

            moneyLevels[(int)level] = overflow;

            if ((level + 1) < EnumEconomyLevel.H)
            {
                AddMoney(level + 1, 1);
            }
        }
        else
        {
            //Debug.Log("Add money: " + amount + " Level: " + level);
            moneyLevels[(int)level] += amount;
        }

        UpdateWallet();
    }

    public bool RemoveMoney(Money money)
    {
        bool success = false;
        for (int i = 0; i < money.moneyLevels.Length; ++i)
        {
            success = RemoveMoney((EnumEconomyLevel)i, money.moneyLevels[i]);
        }

        return success;
    }

    public bool RemoveMoney(EnumEconomyLevel level, float cost)
    {
        bool success;
        cost = Mathf.Clamp(cost, 0, 99);

        float currentBalance = moneyLevels[(int)level];
        float difference = currentBalance - cost;

        if (difference < 0)
        {
            float remainder = difference + 100;

            moneyLevels[(int)level] = remainder;

            if ((level + 1) < EnumEconomyLevel.H)
            {
                return RemoveMoney(level + 1, 1);
            }
            else
            {
                ResetMoney();
                success = false;
            }
        }
        else
        {
            moneyLevels[(int)level] -= cost;
            success = true;
        }

        UpdateWallet();
        return success;
    }

    private void UpdateWallet()
    {
        EnumEconomyLevel prevLevel = currentLevel;
        float prevPrimaryLevelAmount = primaryAmount;
        float prevSecondaryAmount = secondaryAmount;

        currentLevel = EnumEconomyLevel.A;
        primaryAmount = 0;
        secondaryAmount = 0;

        for (int i = 0; i < moneyLevels.Length; ++i)
        {
            float currentLevelAmount = moneyLevels[i];
            if (currentLevelAmount > 0)
            {
                currentLevel = (EnumEconomyLevel)i;
                primaryAmount = currentLevelAmount;

                if (i > 0)
                {
                    secondaryAmount = moneyLevels[i - 1];
                }
                else
                {
                    secondaryAmount = 0;
                }
            }
        }

        if (prevLevel != currentLevel || prevPrimaryLevelAmount != primaryAmount || prevSecondaryAmount != secondaryAmount)
        {
            if (Managers.EventManager.OnMoneyChanged != null)
            {
                Managers.EventManager.OnMoneyChanged(this);
            }
        }
    }

    public bool IsGreaterOrEqual(Money other)
    {
        if (currentLevel > other.currentLevel)
        {
            return true;
        }
        
        if (currentLevel == other.currentLevel && primaryAmount > other.primaryAmount)
        {
            return true;
        }

        if (currentLevel == other.currentLevel && primaryAmount == other.primaryAmount && secondaryAmount >= other.secondaryAmount)
        {
            return true;
        }

        return false;
    }

    public static Money Multiply(Money money, float scaler)
    {
        Money scaled = new Money();

        for (int i = 0; i < money.moneyLevels.Length; ++i)
        {
            //Debug.Log("Level: " + (EnumEconomyLevel)i + " - " + (int)Mathf.Round(money.moneyLevels[i] * scaler));
            scaled.AddMoney((EnumEconomyLevel)i, money.moneyLevels[i] * scaler);
        }

        //float amount = 0;

        //for (int i = 0; i < money.moneyLevels.Length; ++i)
        //{
        //    amount += money.moneyLevels[i] * (Mathf.Pow(10, i));
        //}

        //amount *= scaler;

        return scaled;
    }

    public EnumEconomyLevel GetEconomyLevel()
    {
        return currentLevel;
    }

    public float GetPrimaryLevelAmount()
    {
        return primaryAmount;
    }

    public float GetSecondaryLevelAmount()
    {
        return secondaryAmount;
    }

    public string GetBalance()
    {
        return (((int)primaryAmount).ToString() + "." + getPrevLevel((int)secondaryAmount) + " " + currentLevel.ToString());
    }

    private string getPrevLevel(float amount)
    {
        if (amount < 10)
        {
            return "0" + amount.ToString();
        }
        else
        {
            return amount.ToString();
        }
    }
}
