using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EconomyManager : ManagerBase<EconomyManager>
    {
        [SerializeField] private EnumEconomyLevel startingLevel;
        [SerializeField] private int primaryAmount;
        [SerializeField] private int secondaryAmount;

        private Money currentBalance;

        public Money Wallet { get { return currentBalance; } }

        private void Start()
        {
            currentBalance = new Money(startingLevel, primaryAmount, secondaryAmount);
            UIWalletManager.Instance.UpdateLabels(currentBalance);
        }

        private void Update()
        {
            //currentBalance.RemoveMoney(EnumEconomyLevel.A, 1);
            //Debug.Log(currentBalance.GetBalance());
        }

        public bool CanAfford(Money cost)
        {
            return Wallet.IsGreaterOrEqual(cost);
        }

        public void Buy(Planet planet)
        {
            Wallet.RemoveMoney(planet.PlanetProperties.DefaultCost);
        }
    }
}
