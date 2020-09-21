using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    IEnumerator CheckMoveCo();
    void CalculateAngle();
    void MoveElements();
}
