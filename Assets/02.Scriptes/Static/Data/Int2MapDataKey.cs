using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
namespace Game.Data
{
    
    public struct Int2MapDataKey : IEquatable<Int2MapDataKey> {
        public const int INTERVAL_DISTANCE = 10; // 10 ¥‹¿ß∑Œ ∏ √ª≈©∏¶ π≠¿Ω
        private int _x;
        private int _y;

        public Int2MapDataKey(int x = int.MaxValue, int y = int.MaxValue) {
            _x = x;
            _y = y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeValueFromPosition(float3 value) {
            _x = (int)(value.x / INTERVAL_DISTANCE);
            _y = (int)(value.y / INTERVAL_DISTANCE);
        }

        public readonly int2 GetKey() {
            return new int2(_x, _y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeValue(int2 value) {
            _x = value.x;
            _y = value.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 31 + _x.GetHashCode();
                hash = hash * 31 + _y.GetHashCode();
                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Int2MapDataKey other) {
            return _x== other._x && _y== other._y;
        }
        public override string ToString() {
            return _x.ToString() + " / " + _y.ToString();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Int2MapDataKey k1, Int2MapDataKey k2) {
            return k1.Equals(k2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Int2MapDataKey k1, Int2MapDataKey k2) {
            return !k1.Equals(k2);
        }
    }

    public struct Int2KeyEntity {
        public Int2MapDataKey key;
        public Entity entity;

        public Int2KeyEntity(Int2MapDataKey key, Entity entity) {
            this.key = key;
            this.entity = entity;
        }

    }
}
