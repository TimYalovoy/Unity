using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace TimurYalovoy.UserWallet
{
    [CreateAssetMenu(fileName = "Asset", menuName = "Currency")]
    [System.Serializable]
    public class Currency : ScriptableObject, IOperable
    {
        #region Propeties
        [SerializeField] protected string cName;
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected int value;
        [SerializeField] protected int maxCurrencyValue;
        // path of save data
        #endregion
        public static string path;

        public void OnEnable()
        {
            path = Path.Combine(Application.dataPath, "Save.json");
        }

        #region Get/Set methods
        public string CName
        {
            get
            {
                return cName;
            }
            set
            {
                cName = value;
            }
        }

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                this.sprite = value;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        public int MaxCurrencyValue
        {
            get
            {
                return maxCurrencyValue;
            }
            set
            {
                maxCurrencyValue = value;
            }
        }
        #endregion

        #region Implementation interface IOperable
        private void ChangeValue(bool increment)
        {
            int currentValue = Value;
            if (increment)
            {
                if (currentValue == MaxCurrencyValue)
                {
                    currentValue = MaxCurrencyValue;
                }
                else
                {
                    currentValue++;
                }
            }
            else
            {
                if (NonNegative())
                {
                    currentValue--;
                }
                else
                {
                    currentValue = 0;
                }
            }
            Value = currentValue;
        }
        public void Increment()
        {
            ChangeValue(true);
        }

        public void Decrement()
        {
            ChangeValue(false);
        }

        public bool NonNegative()
        {
            if (Value > 0)
            {
                return true;
            }
            return false;
        }

        public void ZeroingValue()
        {
            Value = 0;
        }
        #endregion
    }
}
