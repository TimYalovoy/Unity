using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Member : MonoBehaviour, IMoveable, IUpgradeable
{
    public abstract IEnumerator CheckMoveCo();
    public abstract void CalculateAngle();
    public abstract void MoveElements();

    public abstract void CheckPowerUp();
    public abstract void MakePowerUp_ChangeGravity();

}
