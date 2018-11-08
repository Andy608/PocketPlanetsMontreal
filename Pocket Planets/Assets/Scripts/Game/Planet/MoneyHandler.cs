using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    private Planet currentPlanet;

    private Money profitPerFrame;

    private void OnEnable()
    {
        currentPlanet = GetComponent<Planet>();
        profitPerFrame = Money.Multiply(currentPlanet.PlanetProperties.DefaultProfitPerSecond, Time.deltaTime);
    }

    private void Update()
    {
        if (currentPlanet.PlanetState == EnumPlanetState.ALIVE)
        {
            Debug.Log("PROFIT: " + profitPerFrame.GetBalance());
            Managers.EconomyManager.Instance.Wallet.AddMoney(profitPerFrame);
        }
    }
}
