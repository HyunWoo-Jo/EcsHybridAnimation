## Hybrid Animation
---
#### Hybrid Animation 처리 과정
<img width="476" alt="image" src="https://github.com/HyunWoo-Jo/UnityEcs_practice2/assets/73084993/b0591bcd-62b2-4ad1-8a6a-fe5fe214d6ed">

***

#### 초기화 시스템
```csharp 
//AnimationMaterialInitializeSystem.cs

void OnUpdate(ref SystemState state) {
    var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp, PlaybackPolicy.SinglePlayback);

    foreach (var (animRef, materialRef,  entity) in SystemAPI.Query<AnimGameObjectReference, MaterialReference>().WithNone<AnimationReference>().WithEntityAccess()) {
        // 생성
        GameObject obj = HybridObjectManager.Instance.InstantiateObject(animRef.prefab);
        ecb.AddComponent(entity, new AnimationReference {
            animator = obj.GetComponent<Animator>(),
            transform = obj.GetComponent<Transform>()
        });
        var skinds = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        if(skinds.Length > 0)
        {
            materialRef.material = skinds[0].material;
        }
        // 삭제
        ecb.RemoveComponent<AnimGameObjectReference>(entity);
        DestroyChild(ref state, ref ecb, animRef.animatorEntity);
    }

    ecb.Playback(state.EntityManager);
    ecb.Dispose();
}
```
***
#### Hybrid Animation 구동부
```csharp
//AnimationMaterialSystem.cs

foreach (var (animRef, animPro, transform) in SystemAPI.Query<AnimationReference, RefRW<AnimationProperties>, RefRO<LocalTransform>>()) {
    // Hybrid Object와 Entity Position Rotation 맵핑
    animRef.transform.position = transform.ValueRO.Position;
    animRef.transform.rotation = transform.ValueRO.Rotation;

    // Animation Properties에 animtionState 저장
    animPro.ValueRW.currentAnimationTagHash = currentAnimationHash;
    animPro.ValueRW.currentAnimationNormalizedTime = animRef.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    // Move 전달
    if (animPro.ValueRO.preMove != animPro.ValueRO.isMove) {
        animPro.ValueRW.preMove = animPro.ValueRO.isMove;
        animRef.animator.SetBool(AnimationHash.Walk, animPro.ValueRO.isMove);
    }
    // Attack 전달
    if (animPro.ValueRO.preAttack != animPro.ValueRO.attack) {
        animPro.ValueRW.preAttack = animPro.ValueRO.attack;
        animRef.animator.SetInteger(AnimationHash.Attack, animPro.ValueRO.attack);
    }
    // Trigger 전달
    if (animPro.ValueRO.isTrigger) {
        animPro.ValueRW.isTrigger = false;
        animRef.animator.SetTrigger(animPro.ValueRO.triggerHash);
    }
}

// Material 정보 전송
foreach(var (materialRef, floatBuffer) in SystemAPI.Query<MaterialReference, DynamicBuffer<FloatMaterialElement>>())
{
    if (materialRef.material is null) continue;
    foreach(var element in floatBuffer)
    {
        materialRef.material.SetFloat(element.nameId, element.value);
    }
}
```
