using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    [SerializeField] GameObject _dragPoint;
    [SerializeField] ParticleSystem _dragPointGraphic;
    Rigidbody _dragPointRb;
    [SerializeField] ConfigurableJoint _joint;
    [SerializeField] float _springForce = 100;
    public Transform DragPoint { get => _dragPoint.transform; }
    public ConfigurableJoint Joint { get => _joint; }
    public float SpringForce { get => _springForce; set => _springForce = value; }

    bool _objectHasJointWhenStart = false;

    private void Start()
    {
        _dragPointRb = _dragPoint.GetComponent<Rigidbody>();
        //_dragPointGraphic.Stop();
        _dragPoint.SetActive(false);
    }
    public void StartDrag(Rigidbody rb, Vector3 dragPosWorld)
    {
        

        _dragPoint.SetActive(true);
        _dragPoint.transform.position = dragPosWorld;


        ConfigurableJoint cj;
        //if (rb.TryGetComponent<ConfigurableJoint>(out sj))
        //{
        //    //_objectHasJointWhenStart = true;
        //    //_joint = sj;
        //}
        //else
        //{
            _objectHasJointWhenStart = false;
            cj = rb.gameObject.AddComponent<ConfigurableJoint>();
            //rb.AddComponent<SetAngularDragToNormalOnJointBreak>();
            _joint = cj;
        //}
        _joint.xMotion = ConfigurableJointMotion.Limited;
        _joint.yMotion = ConfigurableJointMotion.Limited;
        _joint.zMotion = ConfigurableJointMotion.Limited;

        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedBody = _dragPointRb;
        _joint.connectedAnchor = Vector3.zero;
        _joint.anchor = _joint.transform.InverseTransformPoint(dragPosWorld);
        //_dragPoint.GetComponent<MeshRenderer>().enabled = true;
    }
    public void EndDrag()
    {
        //if (_objectHasJointWhenStart)
        //{
        //    if (_joint.IsDestroyed())
        //    {
        //        return;
        //    }
        //    _joint.connectedBody = null;
        //}
        //else
        //{
            Destroy(_joint);
        //}
        _joint = null;
        //_dragPoint.GetComponent<MeshRenderer>().enabled = false;
        _dragPoint.SetActive(false);
    }
}
