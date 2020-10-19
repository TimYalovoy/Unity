using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using UnityEngine;

namespace TimurYalovoy.UserWallet
{
    public class Save : MonoBehaviour
    {
        public static void PlayerPrefsSave(Currency currency)
        {
            PlayerPrefs.SetString($"CurrencyName_{currency.CName}", currency.CName);
            PlayerPrefs.SetString($"{currency.CName}", currency.CName);
            PlayerPrefs.SetInt($"CurrencyValue_{currency.CName}", currency.Value);
            PlayerPrefs.SetInt($"MaxCurrencyValue_{currency.CName}", currency.MaxCurrencyValue);
            PlayerPrefs.Save();
            Debug.Log($"Game data saved!");
        }

        public static void JsonSave(List<Currency> currencies)
        {
            string[] jsonArray = new string[currencies.Count];
            for (int i = 0; i < currencies.Count; i++)
            {
                jsonArray[i] = JsonUtility.ToJson(currencies[i], false);
            }
            File.WriteAllLines(Currency.path, jsonArray);
        }
    }
}
