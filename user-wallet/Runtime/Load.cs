using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TimurYalovoy.UserWallet
{
    public class Load : MonoBehaviour
{
    public static Currency PlayerPrefsLoad(string key)
    {
        if (PlayerPrefs.HasKey($"CurrencyName_{key}")
            && PlayerPrefs.HasKey(key)
            && PlayerPrefs.HasKey($"CurrencyValue_{key}")
            && PlayerPrefs.HasKey($"MaxCurrencyValue_{key}"))
        {
            Currency currency = ScriptableObject.CreateInstance<Currency>();
            currency.CName = PlayerPrefs.GetString($"CurrencyName_" + key);
            currency.Sprite = Resources.Load(PlayerPrefs.GetString(key)) as Sprite;
            currency.Value = PlayerPrefs.GetInt($"CurrencyValue_{key}");
            currency.MaxCurrencyValue = PlayerPrefs.GetInt($"MaxCurrencyValue_{key}");
            currency.name = currency.CName;
            return currency;
        }
        else
        {
            return null;
        }
    }

    public static List<Currency> JsonLoad<T>()
    {
        if (File.Exists(Currency.path))
        {
            List<Currency> currencies = new List<Currency>();
            for (int index = 0; index < File.ReadAllLines(Currency.path).Length; index++)
            {
                Currency currency = ScriptableObject.CreateInstance<Currency>();
                JsonUtility.FromJsonOverwrite(File.ReadAllLines(Currency.path)[index], currency);
                string cname = currency.CName;
                currency.name = cname;
                currencies.Add(currency);
            }
            return currencies;
        }
        return default;
    }
}
}
