using UnityEngine;

public class BallisticMover : MonoBehaviour
{
    public float speed = 10f;
    public float length = 20f;
    public float height = 5f;

    int _previousLayer;
    bool _colliderInCredit = false;
    void Start()
    {
        _previousLayer = gameObject.layer;
        int ballisticLayer = LayerMask.NameToLayer("Ballistic");
        if (ballisticLayer == -1)
        {
            Debug.LogError("Слой \"Ballistic\" не найден.");
            enabled = false;
            return;
        }
        gameObject.layer = ballisticLayer;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>().size /= 2;
            _colliderInCredit = true;
        }
        rb.AddForce((PlayerManager.CharacterStatic.rootTransform.forward * 2 + Vector3.up) * 5, ForceMode.Impulse);

        Invoke(nameof(ResetLayerAndStop), 1);
    }
    Vector3 _prevPos;
    private void FixedUpdate()
    {
        float m = (transform.position - _prevPos).magnitude;
        if (m < 0.001f)
        {
            ResetLayerAndStop();
            Destroy(this);
        }
        _prevPos = transform.position;
    }

    void ResetLayerAndStop()
    {
        if (_colliderInCredit)
        {
            Destroy(GetComponent<BoxCollider>());
        }
        gameObject.layer = _previousLayer;
        Destroy(GetComponent<Rigidbody>());
    }
}
