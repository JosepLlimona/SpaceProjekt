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

    public bool checkPlacment(Module module, string pos)
    {
        if(pos == "up")
            return up && module.down;
        else if(pos == "right")
            return right && module.left;
        else if(pos == "left")
            return left && module.right;
        else if (pos == "down")
            return down && module.up;
        return false;
    }
}
