
using UnityEngine;

public class MorterController : EnemyController
{

    protected override void Start()
    {
        base.Start();
    }

    public void EnemyInRange(GameObject p_target)
    {
        s_system.SetTarget(p_target);
    }

    public void EnemyOutOfRange()
    {
        s_system.SetTarget(null);
    }

}
