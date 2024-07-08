## Logic
---
#### 인풋 처리
```csharp
// InputEventSystem.cs

[BurstCompile]
private void MoveClick() {
    _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
    var ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // 레이를 통한 인풋 처리
    var rayStart = ray.origin;
    var rayEnd = ray.GetPoint(300f);
    var raycastInput = new RaycastInput {
        Start = rayStart,
        End = rayEnd,
        Filter = new CollisionFilter {
            GroupIndex = 0,
            BelongsTo = (uint)LayerName.Ground,
            CollidesWith = (uint)LayerName.All
        }
    };
#if UNITY_EDITOR
    Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
    if (_physicsWorld.CastRay(raycastInput, out var raycastHit)) {
        foreach (var clickProperties in SystemAPI.Query<InputEventProperties>()) {
            foreach (var (navAspect, attackAspect) in SystemAPI.Query<NavAgentAspect, AttackAspect>().WithAll<PlayerTag>()) {
                SystemAPI.GetComponentRW<LocalTransform>(clickProperties.clickMovePointEntity).ValueRW.Position = raycastHit.Position;
                int attackCount = attackAspect.GetAttackAnimationCount();
                if (attackCount == -1) {
                    navAspect.IsStop = false;
                    navAspect.IsMoveStop = false;
                }
            }
        }
    }
}
------------------------------------------------------------------------------------------------------------------------
// 

```
