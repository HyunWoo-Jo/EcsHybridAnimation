using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data {
    public enum LayerName : uint
    {
        Not = 0,
        All = 2147483647,
        Ground = 1 ,
        Player = 1 << 1,
        Enemy = 1 << 2,
        Raycast = 1 << 3,

    }
}
