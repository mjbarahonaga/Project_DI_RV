using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable_M
{
    public void Hit(Arrow_M arrow = null);
    public void RemoveHits();
}
