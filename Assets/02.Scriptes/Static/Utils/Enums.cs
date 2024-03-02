using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils {
    public enum LayerName : uint
    {
        All = uint.MaxValue,
        Ground = 1 << 1,
        Player = 1 << 2,
        Enemy = 1 << 3,
        Raycast = 1 << 4,

    }
}
