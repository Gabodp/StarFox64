using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class BanditController : EnemyController
{
    //Should be clamped to X(-8,8) and Y(4,-4)
    [Header("Local Pos to Player on Screen")]
    public float posX;
    public float posY;
    public float zDistance;

    private const float timeToLeave = 6.0f;
    private Sequence mySequence;
    private TrackingSystem t_system;

    protected override void Start()
    {
        base.Start();
        IdleAnimation();
        t_system = GetComponentInChildren<TrackingSystem>();
    }

    private void DoAnimation()
    {
        Vector3 direction = new Vector3(posX, posY, zDistance);
        transform.DOLocalMove(direction, 2.0f);
    }

    private void IdleAnimation()
    {
        DOTween.useSmoothDeltaTime = true;
        if (!DOTween.IsTweening(transform))
        {
            mySequence = DOTween.Sequence();
            mySequence.SetLoops<Sequence>(-1,LoopType.Restart);
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y + 0.8f,0.8f));
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y, 0.8f));

            mySequence.Play();
        }

    }

    public void ActivateEnemy(GameObject player)
    {
        mySequence.Kill();
        DoAnimation();
        t_system.SetTarget(player);
        transform.SetParent(player.transform.parent);
    }

}
