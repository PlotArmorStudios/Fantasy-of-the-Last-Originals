using UnityEngine;

public class EntityStopMotionCollision : StopMovementOnCollision
{
    protected override void FixedUpdate()
    {
        _touching = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask);
        _shouldStop = _touching.Length > 0;
        _rootMotion.StopMovement = _shouldStop;
    }
}