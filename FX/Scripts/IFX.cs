using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFX
{
    void SetPosition(Vector3 pos);

    void Effect(GameObject target);
}
