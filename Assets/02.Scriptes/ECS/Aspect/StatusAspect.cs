using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct StatusAspect : IAspect
    {
        private readonly RefRW<StatusProperties> _statusProperties;
        private readonly DynamicBuffer<HpCalculatorElement> _hpCalculatorBuffer;

        public float Hp {
            get { return _statusProperties.ValueRO.hp; }
            set { _statusProperties.ValueRW.hp = value; }
        }

        public DynamicBuffer<HpCalculatorElement> GetDynamicBuffer() {
            return _hpCalculatorBuffer;
        }
        public void ClearHpBuffer() {
            _hpCalculatorBuffer.Clear();
        }

        public void AddBuffer(float value) {
            _hpCalculatorBuffer.Add(new HpCalculatorElement { value = value});
        }

    }
}
