using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimurYalovoy.UserWallet
{
    public interface IOperable
    {
        void Increment();
        void Decrement();
        bool NonNegative();

        void ZeroingValue();
    }
}
