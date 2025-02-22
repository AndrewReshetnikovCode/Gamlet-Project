using UnityEngine;

public class FirstPersonDragController : MonoBehaviour
{
    [SerializeField] DragRigidbody _dragRigidbody;
    [SerializeField] KeyCode _activateKey;
    [SerializeField] float _raycastLength;
    [SerializeField] Transform _raycastDir;
    [SerializeField] LayerMask _raycastLayerMask;
    [SerializeField] float _jointBreakForce;
    [SerializeField] float _jointBreakTorque;
    bool _isDragging = false;
    [SerializeField]
    [Range(0f, 100f)] float _minDistance;
    [SerializeField]
    [Range(0f, 100f)] float _maxDistance;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_activateKey))
        {
            if (_isDragging)
            {
                EndDrag();
            }
            else
            {
                RaycastHit hit;
                Physics.Raycast(new Ray(_raycastDir.position, _raycastDir.forward), out hit, _raycastLength, _raycastLayerMask);
                if (hit.transform != null)
                {
                    StartDrag(hit.transform.gameObject, hit.point);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_isDragging && _dragRigidbody.Joint.gameObject.tag == "Nail")
            {
                TryConnectTwoPlanks(_dragRigidbody.DragPoint.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_isDragging)
            {
                _dragRigidbody.Joint.angularXMotion = ConfigurableJointMotion.Locked;
                _dragRigidbody.Joint.angularYMotion = ConfigurableJointMotion.Locked;
                _dragRigidbody.Joint.angularZMotion = ConfigurableJointMotion.Locked;
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (_isDragging)
            {
                _dragRigidbody.Joint.angularXMotion = ConfigurableJointMotion.Free;
                _dragRigidbody.Joint.angularYMotion = ConfigurableJointMotion.Free;
                _dragRigidbody.Joint.angularZMotion = ConfigurableJointMotion.Free;
            }
        }
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            if (_isDragging)
            {
                float distanceToCamera = Vector3.Distance(_dragRigidbody.DragPoint.transform.position, _raycastDir.position);
                if (Input.mouseScrollDelta.y == 1)
                {
                    if (distanceToCamera < _maxDistance)
                    {
                        _dragRigidbody.DragPoint.transform.position += _raycastDir.forward * Input.mouseScrollDelta.y;
                    }
                }
                else
                {
                    if (distanceToCamera > _minDistance)
                    {
                        _dragRigidbody.DragPoint.transform.position += _raycastDir.forward * Input.mouseScrollDelta.y;
                    }
                }
            }
        }
    }

    void StartDrag(GameObject obj, Vector3 dragPosWorld)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        if (rb.isKinematic)
        {
            return;
        }
        _dragRigidbody.StartDrag(rb, dragPosWorld);

        _isDragging = true;

    }
    void EndDrag()
    {
        _dragRigidbody.EndDrag();

        _isDragging = false;
    }
    void TryConnectTwoPlanks(Vector3 nailPos)
    {
        Collider[] hits = Physics.OverlapSphere(_dragRigidbody.DragPoint.position, 1);
        Collider plank1 = null;
        Collider plank2 = null;
        foreach (var item in hits)
        {
            if (item.tag == "Plank")
            {
                plank1 = item;
            }
        }
        foreach (var item in hits)
        {
            if (item.tag == "Plank" && item != plank1)
            {
                plank2 = item;
            }
        }
        if (plank1 && plank2)
        {
            ConfigurableJoint cj = plank1.gameObject.AddComponent<ConfigurableJoint>();
            cj.connectedBody = plank2.GetComponent<Rigidbody>();
            cj.autoConfigureConnectedAnchor = false;
            cj.anchor = plank1.transform.InverseTransformPoint(nailPos);
            cj.connectedAnchor = plank2.transform.InverseTransformPoint(nailPos);

            cj.xMotion = ConfigurableJointMotion.Limited;
            cj.yMotion = ConfigurableJointMotion.Limited;
            cj.zMotion = ConfigurableJointMotion.Limited;

            cj.angularXMotion = ConfigurableJointMotion.Locked;
            cj.angularZMotion = ConfigurableJointMotion.Locked;

            cj.axis = _dragRigidbody.Joint.transform.up;
            cj.secondaryAxis = _dragRigidbody.Joint.transform.up;

            //plank2.GetComponent<Rigidbody>().angularDrag = 9999;

            cj.breakForce = _jointBreakForce;
            cj.breakTorque = _jointBreakTorque;
            cj.enableCollision = true;
        }
        GameObject nailClone = Instantiate(_dragRigidbody.Joint.gameObject);
        Destroy(nailClone.GetComponent<Joint>());
        Destroy(nailClone.GetComponent<Rigidbody>());
        nailClone.transform.SetParent(plank1.transform, true);
    }
}
