using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace TimurYalovoy.UserWallet
{
    public class DBManager : MonoBehaviour
    {
        [SerializeField] protected Wallet wallet;

        void Start()
        {
            wallet = GameObject.Find("Player").GetComponent<Wallet>();
            foreach (Currency currency in wallet.currencies)
            {
                StartCoroutine(LoadCurrencyFromServer(currency));
            }
        }

        private void OnApplicationQuit()
        {
            StartCoroutine(SaveCurrencyOnServer());
        }
        private IEnumerator SaveCurrencyOnServer()
        {
            foreach (Currency currency in wallet.currencies)
            {
                WWWForm form = new WWWForm();
                form.AddField("name", currency.CName);
                form.AddField("img", currency.CName);
                form.AddField("value", currency.Value);
                form.AddField("maxValue", currency.MaxCurrencyValue);

                UnityWebRequest www = UnityWebRequest.Post("https://userwallettest.000webhostapp.com/save_currency.php", form);

                yield return www.SendWebRequest();

                if (!www.isNetworkError || !www.isHttpError)
                {
                    Debug.Log($"Server response: {www.responseCode}\nResponse body:{www.downloadHandler.text}");
                }
                else
                {
                    Debug.Log($"Error: {www.error}");
                    yield break;
                }
            }
        }

        private IEnumerator LoadCurrencyFromServer(Currency currency)
        {
            WWWForm form = new WWWForm();
            form.AddField("name", currency.CName);

            UnityWebRequest www = UnityWebRequest.Post("https://userwallettest.000webhostapp.com/load_currency.php", form);

            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                Debug.Log($"Server response: {www.responseCode}\nResponse body:{www.downloadHandler.text}");
                JsonUtility.FromJsonOverwrite(www.downloadHandler.text, currency);
                currency.name = currency.CName;
                wallet.currencies.Add(currency);
            }
            else
            {
                Debug.Log($"Error: {www.error}");
                yield break;
            }
        }
    }
}
