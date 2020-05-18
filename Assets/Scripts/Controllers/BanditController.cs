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

    private const float timeToLeave = 10.0f;
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
            mySequence.SetLoops<Sequence>(-1, LoopType.Restart);
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y + 0.8f, 0.8f));
            mySequence.Append(transform.DOLocalMoveY(transform.localPosition.y, 0.8f));

            mySequence.Play();
        }

    }

    public void ActivateBandit(GameObject player)
    {
        mySequence.Kill();
        DoAnimation();
        t_system.SetTarget(player.transform.GetChild(2).gameObject);//Le asignas el nuevo centro del jugador
        s_system.SetTarget(player.transform.GetChild(2).gameObject);
        transform.SetParent(player.transform.parent);

        StartCoroutine(LeavingRoutine());
    }

    IEnumerator LeavingRoutine()
    {
        yield return new WaitForSeconds(timeToLeave);

        int[] directions = new int[] { -1, 1 };
        float xDirection = directions[Random.Range(0, 2)];
        transform.DOLocalMove(new Vector3(xDirection * 60, transform.localPosition.y, transform.localPosition.z), 2f);
        
        yield return new WaitForSeconds(1.5f);

        s_system.SetTarget(null);
        t_system.SetTarget(null);
        transform.SetParent(null);
    }
   
}
