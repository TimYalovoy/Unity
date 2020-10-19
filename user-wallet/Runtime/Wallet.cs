using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TimurYalovoy.UserWallet
{
    public class Wallet : Deposit
    {
        public SaveLoadMethods saveLoadMethods;

        #region Wallet Singlton
        private Wallet() { }
        private static Wallet _instance;
        private static readonly object _lock = new object();

        private void Start()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = this;
                    }
                }
            }

            WalletInitialize();
        }

        private void WalletInitialize()
        {
            switch (saveLoadMethods)
            {
                case SaveLoadMethods.PlayerPrefs:
                    {
                        if (PlayerPrefs.HasKey($"CurrencyName_{currencies[0].CName}"))
                        {
                            UW_Load();
                        }
                    }
                    break;
                case SaveLoadMethods.JSON:
                    {
                        if (!File.Exists(Currency.path))
                        {
                            Debug.Log($"Load file is not found.");
                            break;
                        }
                        if (new FileInfo(Currency.path).Length > 0)
                        {
                            UW_Load();
                        }
                        else
                        {
                            Debug.Log($"Load file is empty.");
                            break;
                        }
                    }
                    break;
            }
        }
        #endregion

        [SerializeField] public List<Currency> currencies;

        #region UserWallet Save & Load Methods
        public override void UW_Load()
        {
            switch (saveLoadMethods)
            {
                case SaveLoadMethods.PlayerPrefs:
                    {
                        for (int i = 0; i < this.currencies.Count; i++)
                        {
                            Load.PlayerPrefsLoad(currencies[i].CName);
                        }
                    }
                    break;
                case SaveLoadMethods.JSON:
                    {
                        currencies = Load.JsonLoad<Currency>();
                    }
                    break;
            }
        }

        public override void UW_Save()
        {
            switch (saveLoadMethods)
            {
                case SaveLoadMethods.PlayerPrefs:
                    {
                        for (int i = 0; i < currencies.Count; i++)
                        {
                            Currency currency = currencies[i];
                            Save.PlayerPrefsSave(currency);
                        }
                    }
                    break;
                case SaveLoadMethods.JSON:
                    {
                        Save.JsonSave(currencies);
                    }
                    break;
            }
        }
        #endregion
    }
}
