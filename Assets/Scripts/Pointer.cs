/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public delegate void PointerEvents();
    public static PointerEvents OnChildDestoyed;

    private void OnTransformChildrenChanged()
    {
        OnChildDestoyed();
    }
}
