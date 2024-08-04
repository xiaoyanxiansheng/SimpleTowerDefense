using UnityEngine;

public class LevelEnemyBehaviour : MonoBehaviour
{
    public int entityId;
    public GameObject start;
    public GameObject end;
    public float enterTime;
}

public class LevelEnemyData
{
    public int entityId;
    public Vector2 start;
    public Vector2 end;
    public float enterTime;

    public static LevelEnemyData GetLevelEnemyData(LevelEnemyBehaviour behaviour)
    {
        LevelEnemyData data = new LevelEnemyData();
        data.entityId = behaviour.entityId;
        data.enterTime = behaviour.enterTime;
        data.start = BigWorldManager.Instance.WorldToLocal(behaviour.start.transform.position);
        data.end = BigWorldManager.Instance.WorldToLocal(behaviour.end.transform.position);
        return data;
    }
}