## ECS NavAgent
---
#### Ecs NavAgent 처리 과정
<img width="496" alt="image" src="https://github.com/HyunWoo-Jo/UnityEcs_practice2/assets/73084993/7f5c3c33-a6cd-4e3a-b0fb-7177b3c27336">

***
#### NavMesh를 다른 World에서 사용할 수 있도록 처리
```csharp
// NavMeshAdder.cs

[SerializeField] private List<NavMeshData> _navMeshDataList = new();
private List<NavMeshDataInstance> _navMeshDataInsList = new();

void Awake()
{
    foreach(var navData in _navMeshDataList) {
        var navIns = NavMesh.AddNavMeshData(navData);
        _navMeshDataInsList.Add(navIns);
    }
}
private void OnDisable() {
    foreach(var navIns in _navMeshDataInsList) {
        navIns.Remove();
    }
}
```

***
#### Components
```csharp
// NavAgentProperties.cs

public partial struct NavAgentProperties : IComponentData, IEnableableComponent
{
    public Entity targetEntity;
    public bool isPathFinded;
    public bool isStop;
    public int curretWaypoint;
    public float traceRange;
}

public partial struct WaypointBuffer : IBufferElementData {
    public float3 waypoint;
}
```
***
#### NavAgent System
```csharp
// NavAgentSystem.cs

[BurstCompile]
public partial struct NavAgentSystem : ISystem {

    private NavMeshQuery _navQuery;
    private float3 _extents;
    private int _maxPathSize;

    [BurstCompile]
    private void OnCreate(ref SystemState state) {
        _navQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent, 1000);
        _extents = new float3(1, 1, 1);
        _maxPathSize = 100;
    }
    [BurstCompile]
    private void OnUpdate(ref SystemState state) {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var navAspect in SystemAPI.Query<NavAgentAspect>()) {
            if (!state.EntityManager.Exists(navAspect.GetTargetEntity())) continue;
            if (navAspect.IsStop) continue;
            FindPath(ref state, navAspect);
            if (navAspect.IsFinded) {
                MoveDirction(navAspect);
            }
        }
    }
    [BurstCompile]
    private void FindPath(ref SystemState state, NavAgentAspect navAspect) {
        float3 startPosition = navAspect.Position;
        float3 endPosition = SystemAPI.GetComponentRO<LocalTransform>(navAspect.GetTargetEntity()).ValueRO.Position;
        NavMeshLocation startLocation = _navQuery.MapLocation(startPosition, _extents, 0);
        NavMeshLocation endLocation = _navQuery.MapLocation(endPosition, _extents, 0);
     
        PathQueryStatus status;
        PathQueryStatus returningStatus;
        if (_navQuery.IsValid(startLocation) && _navQuery.IsValid(endLocation)) {
            status = _navQuery.BeginFindPath(startLocation, endLocation);
            if (status == PathQueryStatus.InProgress) {
                status = _navQuery.UpdateFindPath(_maxPathSize, out int iterationsPerformed);
            }
            if (status == PathQueryStatus.Success) {
                status = _navQuery.EndFindPath(out int pathSize);

                NativeArray<NavMeshLocation> result = new NativeArray<NavMeshLocation>(pathSize + 1, Allocator.Temp);
                NativeArray<StraightPathFlags> straightPathFlags = new NativeArray<StraightPathFlags>(_maxPathSize, Allocator.Temp);
                NativeArray<float> vertexSize = new NativeArray<float>(_maxPathSize, Allocator.Temp);
                NativeArray<PolygonId> polygonIds = new NativeArray<PolygonId>(pathSize + 1, Allocator.Temp);
                int straightPathCount = 0;

                _navQuery.GetPathResult(polygonIds);

                returningStatus = PathUtils.FindStraightPath(
                    _navQuery,
                    startPosition,
                    endPosition,
                    polygonIds,
                    pathSize,
                    ref result,
                    ref straightPathFlags,
                    ref vertexSize,
                    ref straightPathCount,
                    _maxPathSize
                    );
                if (returningStatus == PathQueryStatus.Success) {
                    navAspect.ClearWaypointBuffer();

                    foreach (NavMeshLocation location in result) {
                        if (location.position != Vector3.zero) {
                            navAspect.AddWaypointBuffer(location.position);
                        }
                    }
                    navAspect.FindedNavAgent();
                }
                straightPathFlags.Dispose();
                polygonIds.Dispose();
                vertexSize.Dispose();
            }
        }
    }
    private void MoveDirction(NavAgentAspect navAspect) {
        float3 position = navAspect.Position;
        float3 currentWayPosition = navAspect.GetCurrentWaypointPosition();
        position.y = 0f;
        while(math.distance(position, currentWayPosition) < navAspect.TraceRange) {
            if (navAspect.NextWaypoint()) {
                navAspect.IsMoveStop = true;
                break;
            }
            currentWayPosition = navAspect.GetCurrentWaypointPosition();
        }
        navAspect.SetTragetPosition(currentWayPosition);
        float3 direction = currentWayPosition - position;
        if (direction.x == 0f && direction.z == 0f) return;
        navAspect.IsTurnStop = false;
        // 이동

        navAspect.Dirction = math.normalize(direction);
    }
}
```
