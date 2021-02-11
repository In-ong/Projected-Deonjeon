using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFx
{
    FXManager.eFxCategory GetCategory();

    bool OnEffect();

    void Effect(GameObject target);

    void SetParentGameObject(GameObject parent);

    void SetPosition(GameObject target);
}
