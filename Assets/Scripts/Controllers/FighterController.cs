using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FighterController : EnemyController
{
    private Rigidbody rb;
    private Vector3 direction;
    private bool alive;

    public GameObject model;
    public GameObject smokeParticles;
    public GameObject smokeParticlesPosition;

    public float speed;
    protected override void Start()
    {
        base.Start();
        alive = true;
        rb = GetComponent<Rigidbody>();
        GetComponentInChildren<EnemyShootingSystem>().SetTarget(gameObject);//Deberia ser el player pero para este caso no importa, con tal que no sea null
    }

    protected override void Update()
    {
        //We implement our own Update for this type of enemy
        //base.Update();
        if(lifePoints <= 0 && alive)
        {
            rb.useGravity = true;
            speed *= 0.85f;
            MayDayAnimation();
        }
        transform.position += direction * (speed * Time.deltaTime);
    }

    private void MayDayAnimation()
    {
        alive = false;
        GameObject obj = Instantiate(smokeParticles, smokeParticlesPosition.transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        Destroy(obj, 2.0f);
        if (!DOTween.IsTweening(model.transform))
        {
            model.transform.DORotate(new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 540), 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine).OnComplete(Explode);
            //transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 540), 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine).OnComplete(Explode);

        }

    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
