## Logic
---
#### 전투 처리
---
<img width="513" alt="image" src="https://github.com/HyunWoo-Jo/UnityEcs_practice2/assets/73084993/dc0b6204-db98-4eae-ae45-002529ebe821">

```csharp
// AttackSystem.cs

[BurstCompile]
void OnUpdate(ref SystemState state) {
    PhysicsWorld physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
    foreach (var attackAspect in SystemAPI.Query<AttackAspect>()) {
        int animationTagHash = attackAspect.GetCurrentAnimationTagHash();
        float normalizedTime = attackAspect.GetCurrentAnimationNormalizedTime();
        if (animationTagHash == _attackHash && normalizedTime > 0.9f) {
            attackAspect.IsAttackAble = true;
        } else if(animationTagHash == _attackHash && normalizedTime > 0.4f && attackAspect.IsAttackAble) {
            attackAspect.IsAttackAble = false;
            attackAspect.Attack(attackAspect.GetAttackAnimationCount());
        }
        // Ray 처리
        NativeList<Unity.Physics.RaycastHit> hitList = new NativeList<Unity.Physics.RaycastHit>(Allocator.Temp);
        NativeHashSet<Entity> hitEntitySet = new NativeHashSet<Entity>(10, Allocator.Temp);
        RefRO<LocalTransform> localTransform = SystemAPI.GetComponentRO<LocalTransform>(attackAspect.entity);
        foreach (var ray in attackAspect.GetRays()) {
            float3 start = localTransform.ValueRO.TransformPoint(ray.attackRayBuffer.rayStart); // local To global
            // rayDataElement 형식으로 변환
            RaycastInput rayInput = new RaycastInput {
                Filter = new CollisionFilter {
                    BelongsTo = ((uint)ray.attackRayBuffer.belongTo),
                    CollidesWith = ((uint)ray.attackRayBuffer.withIn),
                    GroupIndex = 0
                },
                Start = start, // local 좌표 월드로 수정
                End = start + localTransform.ValueRO.TransformDirection(ray.attackRayBuffer.rayEnd)             
            };
            float damage = ray.attackRayBuffer.attackMagnification * attackAspect.GetAggressiveStrength();
            if (physicsWorld.CastRay(rayInput, ref hitList)) {   
                if (ray.attackRayBuffer.isNewRay) {
                    hitEntitySet.Clear();
                }
                // 중복 hit 처리
                foreach (var hit in hitList) {
                    if (!SystemAPI.HasComponent<StatusProperties>(hit.Entity)) continue;
                    if (!hitEntitySet.Contains(hit.Entity)) {
                        // 데미지 처리                   
                        StatusAspect statusAspect = SystemAPI.GetAspect<StatusAspect>(hit.Entity);
                        statusAspect.AddBuffer(-damage);
                        hitEntitySet.Add(hit.Entity);
                    }
                }
                hitList.Clear();
            }
        }
        hitList.Dispose();
        hitEntitySet.Dispose();
        attackAspect.ClearBuffer();
    }
}
-----------------------------------------------------------------------------------------------------------------
// StatusAspect.cs
// Components
 public partial struct StatusProperties : IComponentData
 {
     public Entity entity;
     public float maxHp;
     public float currentHp;
     public float aggressiveStrength;
     public float defensivePower;
 }
// Buffer
public partial struct HpCalculatorElement : IBufferElementData {
    public float value;
}
-----------------------------------------------------------------------------------------------------------------
// Attack.cs
public partial struct AttackProperties : IComponentData {
    public bool isAttackAble;
}
public partial struct AttackRayElement : IBufferElementData {
    public RayData attackRayBuffer;
}

public partial struct AttackRayBlobReference : IComponentData{
    public BlobAssetReference<RayBlob> attackBlobRef;
    public BlobAssetReference<IntBlob> startIndexBlobRef;
    public BlobAssetReference<IntBlob> endIndexBlobRef;
}
public struct RayBlob {
    public BlobArray<RayData> attackBlob;
}
public struct IntBlob {
    public BlobArray<int> intBlob;
}
-----------------------------------------------------------------------------------------------------------------
// RayData.cs
[CreateAssetMenu(fileName = "RayData", menuName = "Scritable Object/RayDataList")]
public class RayDataList : ScriptableObject {
    public List<RayData> rayDataList;
}
[Serializable]
public struct RayData {
    public LayerName belongTo;
    public LayerName withIn;
    public float3 rayStart;
    public float3 rayEnd;
    public float attackMagnification;
    public bool isNewRay; // ture 새로운 레이, false 트루 레이가 나올때 까지 같은 Entite에 대한 컨택이 없음 
}

```

---
#### ECS 인풋 처리
---
```csharp
// InputEventSystem.cs

[BurstCompile]
protected override void OnUpdate() {
    // right Click
    if (Input.GetMouseButton(1)) { // callback 처리
        _rightClick_listener?.Invoke();
        _isRightClick = true;
    }
    // Left Click
    else if (Input.GetMouseButton(0)) {
        _leftClick_listener?.Invoke();
        _isLeftClick = true;
    }
    // right
    if (!Input.GetMouseButton(1)) {
        _isRightClick = false;
    }
    // left
    if (!Input.GetMouseButton(0)) {
        _isLeftClick = false;
    }
    ContinueousAttack();
}
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
