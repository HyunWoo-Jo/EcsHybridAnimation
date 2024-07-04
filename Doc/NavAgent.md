## ECS NavAgent
---
#### Hybrid Animation 처리 과정
***
#### Components
```csharp
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
