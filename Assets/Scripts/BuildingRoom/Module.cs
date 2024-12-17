using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [Header ("Valid Positions")]
    public bool up = false;
    public bool right = false;
    public bool down = false;
    public bool left = false;

    public bool checkPlacment(Module module)
    {
        if (up && module.down)
            return true;
        else if (right && module.left)
            return true;
        else if (left && module.right)
            return true;
        else if (down && module.up)
            return true;
        return false;
    }
}
