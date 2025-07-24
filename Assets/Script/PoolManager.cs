using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    public GameObjectPool gremlinPool;
    public GameObjectPool dummyPool;
    [SerializeField] GameObject _gremlinsContainer;
    [SerializeField] GameObject _dummiesContainer;
    public static PoolManager instance;

    private void Awake()
    {
        instance = this;
        //gremlinPool = new(ChildrenUtil.GetAsGameObjects(_gremlinsContainer.transform));
        dummyPool = new(ChildrenUtil.GetAsGameObjects(_dummiesContainer.transform));
    }
    public static GameObject CreatePooledItem(GameObject prefab, Vector3 pos, Quaternion rot, Transform par)
    {
        instance.gremlinPool.Prefab = prefab;
        GameObject o = instance.gremlinPool.Get();
        o.transform.position = pos;
        o.transform.rotation = rot;
        if (par != null)
        {
            o.transform.parent = par;
        }
        return o;
    }
}


