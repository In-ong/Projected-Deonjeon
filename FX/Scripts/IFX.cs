using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFx
{
    bool OnEffect();

    void Effect(GameObject target);

    void SetPosition(GameObject target);
}
