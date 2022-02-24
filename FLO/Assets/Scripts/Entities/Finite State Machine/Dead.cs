using UnityEngine;

public class Dead : IState
{
    const float DESPAWN_DELAY = 5f;
    private readonly Entity _entity;
    private float _despawnTime;

    public Dead(Entity entity)
    {
        _entity = entity;
    }

    public void Tick()
    {
        if (Time.time >= _despawnTime)
            GameObject.Destroy(_entity.gameObject);
    }

    public void OnEnter()
    {
        //Drop loot
        Debug.Log("Dead");
        _despawnTime = Time.time + DESPAWN_DELAY;
    }

    public void OnExit()
    {
    }
}