using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimurYalovoy.UserWallet
{
    public abstract class Deposit : MonoBehaviour
    {
        #region Save & Load methods
        public virtual void UW_Save() { }
        public virtual void UW_Load() { }
        #endregion
    }
}
