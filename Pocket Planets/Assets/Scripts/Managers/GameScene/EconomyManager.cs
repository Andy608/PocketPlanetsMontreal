using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumEconomyLevel
{
    A, B, C, D, E, F, G, H
}

namespace Managers
{
    public class EconomyManager : ManagerBase<EconomyManager>
    {
        private Money currentBalance;

        private void Start()
        {
            currentBalance = new Money();
        }

        private void Update()
        {
            currentBalance.AddMoney(EnumEconomyLevel.A, 1);
        }
    }

    //Gonna have to redo this.
    public class Money
    {
        private int[] moneyLevels = new int[(int)EnumEconomyLevel.H];

        public Money()
        {
            InitMoney();
        }

        private void InitMoney()
        {
            for (int i = 0; i < moneyLevels.Length; ++i)
            {
                moneyLevels[i] = 0;
            }
        }

        public void AddMoney(EnumEconomyLevel level, int amount)
        {
            amount = Mathf.Clamp(amount, 0, 99);

            int currentBalance = moneyLevels[(int)level];

            if (currentBalance + amount >= 100)
            {
                int overflow = currentBalance + amount - 100;

                moneyLevels[(int)level] = overflow;
                AddMoney(level + 1, 1);
            }
            else
            {
                moneyLevels[(int)level] += amount;
            }

            //Debug.Log("A: " + moneyLevels[0] + " B: " + moneyLevels[1]);
        }

        public void RemoveMoney(EnumEconomyLevel level, int cost)
        {

        }

        public string GetBalance()
        {
            EnumEconomyLevel currentLevel = EnumEconomyLevel.A;
            int levelAmount = 0;
            int prevLevelAmount = 0;

            for (int i = 0; i < moneyLevels.Length; ++i)
            {
                int currentLevelAmount = moneyLevels[i];
                if (currentLevelAmount > 0)
                {
                    currentLevel = (EnumEconomyLevel)i;
                    levelAmount = currentLevelAmount;

                    if (i > 0)
                    {
                        prevLevelAmount = moneyLevels[i - 1];
                    }
                    else
                    {
                        prevLevelAmount = 0;
                    }
                }
            }

            return (levelAmount.ToString() + "." + getPrevLevel(prevLevelAmount) + " " + currentLevel.ToString());
        }

        private string getPrevLevel(int amount)
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
}
