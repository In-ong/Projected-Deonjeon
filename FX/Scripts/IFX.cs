using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFX
{
    bool OnEffect();

    void SetPosition(GameObject parent);

    void Effect(GameObject target);
}
