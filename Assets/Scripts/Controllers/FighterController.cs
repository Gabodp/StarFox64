using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FighterController : EnemyController
{
    private Rigidbody rb;
    private Vector3 direction;

    public float speed;
    public GameObject model;
    public GameObject smokeParticles;
    public GameObject smokeParticlesPosition;

    private float timestamp;
    private readonly float lifeTime = 10.0f;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        s_system.SetTarget(gameObject);//Should be player but it doesnt matter here, just avoid being null

        timestamp = 0.0f;
    }

    private void Update()
    {
        //We implement our own Update for this type of enemy
        timestamp += Time.deltaTime;

        if (timestamp >= lifeTime)
            Destroy(gameObject);
        else
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
        
    }

    protected override void ExplodeSequence()
    {
        print("Llamada al hijo explode sequence");
        rb.useGravity = true;
        speed *= 0.9f;

        /*MayDay Animation*/
        GameObject obj = Instantiate(smokeParticles, smokeParticlesPosition.transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        Destroy(obj, 2.0f);
        if (!DOTween.IsTweening(model.transform))
        {
            model.transform.DORotate(new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 540), 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine).OnComplete(base.ExplodeSequence);
        }

    }


    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }

}
