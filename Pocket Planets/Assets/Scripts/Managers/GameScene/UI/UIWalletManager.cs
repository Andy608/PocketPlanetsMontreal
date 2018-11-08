using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class UIWalletManager : ManagerBase<UIWalletManager>
    {
        [SerializeField] private List<TextMeshProUGUI> walletLabels = new List<TextMeshProUGUI>();

        private void OnEnable()
        {
            EventManager.OnMoneyChanged += UpdateLabels;
        }

        private void OnDisable()
        {
            EventManager.OnMoneyChanged -= UpdateLabels;
        }

        public void UpdateLabels(Money money)
        {
            if (money == EconomyManager.Instance.Wallet)
            {
                for (int i = 0; i < walletLabels.Count; ++i)
                {
                    walletLabels[i].text = money.GetBalance();
                }
            }
        }
    }
}
