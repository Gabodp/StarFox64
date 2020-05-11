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
    private bool idle;
    private Sequence mySequence;

    protected override void Start()
    {
        base.Start();
        IdleAnimation();
        idle = true;
    }

    protected override void Update()
    {
        base.Update();
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
            //mySequence.SetDelay<Sequence>(0.2f);
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y + 0.8f,0.8f));
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y, 0.8f));

            mySequence.Play();
        }

    }

    public void ActivateEnemy(GameObject player)
    {
        mySequence.Kill();
        DoAnimation();
        base.EnemyInRange(player.transform.gameObject);
        transform.SetParent(player.transform.parent);
    }

}
