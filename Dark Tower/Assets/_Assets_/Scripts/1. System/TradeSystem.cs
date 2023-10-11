using UnityEngine;
using TMPro;

public class TradeSystem : MonoBehaviour
{
    private static TradeSystem instance;
    public static TradeSystem Instance 
    {
        get { return instance; }
    }

    public TextMeshProUGUI cashText;

    private int _cash = 0;
    public int cash
    {
        get
        {
            return _cash;
        }
        set
        {
            _cash = value;
            cashText.text = string.Format("{0:#,###}", _cash);
        }
    }

    public static void EarnCash(int value)
    {
        instance.cash += value;
    }

    public static void SpendCash(int value)
    {
        instance.cash -= value;
    }

    void Awake() 
    {
        instance = this;
        cash = 10000;
    }
}
