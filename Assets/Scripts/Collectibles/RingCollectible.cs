﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RingCollectible : Collectible
{
    public int lifeRegenerated;
    public GameObject model;
    public bool isStatic;

    private bool taken;
    private float timestamp;
    
    private void Start()
    {
        taken = false;
        timestamp = 0;
    }

    private void Update()
    {
        timestamp += Time.deltaTime;
        if (isStatic) return;

        if ((timestamp > lifeTime) && !taken)
        {
            Disappear();
        }
    }

    public override void DoAction()
    {
        GameController.Instance.SetLifePoints(lifeRegenerated);
    }

    public override void DoAnimation()
    {
        if (!DOTween.IsTweening(transform))
        {
            Sequence mySequence = DOTween.Sequence();
            
            mySequence.Append(transform.DOScale(1.5f, 1.0f));
            mySequence.Append(transform.DOScale(0.1f, 0.8f));
            mySequence.Insert(0,transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, 1080, transform.localEulerAngles.z), 1.8f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine));
            mySequence.Insert(0,transform.DOLocalMove(Vector3.zero, 0.4f));
            mySequence.OnComplete(Disappear);
        }
    }

    private void Disappear()
    {
        if (!DOTween.IsTweening(transform))
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if( other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            transform.parent = other.gameObject.transform;
            DoAction();
            DoAnimation();
            taken = true;
            AudioManager.PlaySound(AudioManager.Sound.RingCollected);
        }
    }

   


}
