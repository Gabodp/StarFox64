using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    private Transform player;

    //Basic Parameters
    public CinemachineDollyCart dolly_cart;
    public float speed;
    
    private readonly float lookSpeed = 8000;
    private readonly float moveForwardSpeed = 20.0f;

    public Transform aimTargetRotation;
    private PlayerShootingSystem s_system;
    private Rigidbody rb;
    private int right,left;

    public TrailRenderer[] trails;

    //Boost parameters
    private bool canBoost;
    private bool isRefilling;
    private float boostCapacity;
    private float timeToNextBoost;
    private bool boosting;

    private void Start()
    {
        player = transform.GetChild(0);
        s_system = GetComponent<PlayerShootingSystem>();
        right = 0; left = 0;
        dolly_cart.m_Speed = moveForwardSpeed;
        rb = GetComponent<Rigidbody>();

        canBoost = true;
        boosting = false;
        isRefilling = false;
        boostCapacity = 100.0f;

    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        canBoost = boostCapacity > 0 && !isRefilling ? true : false;

        XY_Move(horizontal, vertical, speed);
        RotationLook(horizontal, vertical, lookSpeed);
        HorizontalLean(player, horizontal, 60, .1f);
        KeyBoardInput();

        if (boosting)
        {
            boostCapacity = Mathf.Clamp(boostCapacity - (Time.deltaTime * 20), 0f, 100f);//Esto hara que el tiempo maximo de boost sea de 5 segundos
            
            //Falta agregar linea para ir decrementando la barra de boost visualmente
            print(boostCapacity);

            if (boostCapacity == 0f)
            {
                print("Empieza el refill");
                SpeedUp(false);
                isRefilling = true;
                DOVirtual.Float(0f, 100f, 5f, RefillBoostTank).OnComplete(RefillFinished);
            }
                
        }
            
    }

    void KeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
            s_system.shootRockets = true;
        if (Input.GetKeyUp(KeyCode.F))
            s_system.shootRockets = false;
        if (Input.GetKey(KeyCode.Space))
            s_system.shootLasers = true;
        if (Input.GetKeyUp(KeyCode.Space))
            s_system.shootLasers = false;
        if (Input.GetKeyDown(KeyCode.D))
        {
            right++; left = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            left++; right = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canBoost)
            SpeedUp(true);
        if (Input.GetKeyUp(KeyCode.LeftShift) && canBoost)
            SpeedUp(false);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Brake(true);
        if (Input.GetKeyUp(KeyCode.LeftControl))
            Brake(false);

    }



    void XY_Move(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPos();

        if (right == 2)
            BarrelRoll(1);
        else if (left == 2)
            BarrelRoll(-1);
    }

    void ClampPos()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x,0.08f,0.92f);
        position.y = Mathf.Clamp(position.y,0.08f,0.92f);
        transform.position = Camera.main.ViewportToWorldPoint(position);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTargetRotation.parent.position = Vector3.zero;
        aimTargetRotation.localPosition = new Vector3(h, v, 3);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTargetRotation.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    private void SpeedUp(bool activated)
    {
        if (activated)
            GameController.Instance.ShakeCamera(5.0f, 0.4f);

        boosting = activated;
        float newForwardSpeed = activated ? moveForwardSpeed * 2 : moveForwardSpeed;
        float zoom = activated ? -3 : 0;
        float fov_start = activated ? 60 : 75;
        float fov_end = activated ? 75 : 60;

        float trail_start = activated ? 0.08f : 0.2f;
        float trail_end = activated ? 0.2f : 0.08f;
        DOVirtual.Float(trail_start, trail_end, 3f, SetTrailLength);
        DOVirtual.Float(dolly_cart.m_Speed, newForwardSpeed, .15f, SetForwardSpeed);
        DOVirtual.Float(fov_start, fov_end,.8f, GameController.Instance.FieldOfView);

        GameController.Instance.SetCameraZoom(zoom, 0.5f);

    }

    private void Brake(bool activated)
    {
        float newForwardSpeed = activated ? moveForwardSpeed * 0.60f : moveForwardSpeed;
        float zoom = activated ? 1.5f : 0;
        float fov_start = activated ? 60 : 50;
        float fov_end = activated ? 50 : 60;
        DOVirtual.Float(dolly_cart.m_Speed, newForwardSpeed, .15f, SetForwardSpeed);
        DOVirtual.Float(fov_start, fov_end, .8f, GameController.Instance.FieldOfView);

        
        GameController.Instance.SetCameraZoom(zoom, 0.5f);
    }

    private void BarrelRoll(int direction)
    {
        if (!DOTween.IsTweening(player))
        {
            player.DOLocalRotate(new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 360 * -direction), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
            right = 0;
            left = 0;
        }
    }

    private void RefillBoostTank(float value)
    {
        boostCapacity = value;
    }

    private void RefillFinished()
    {
        isRefilling = false;
        print("Termino el refill");
    }

    private void SetForwardSpeed(float newSpeed)
    {
        dolly_cart.m_Speed = newSpeed;
    }

    private void SetTrailLength(float length)
    {
        for(int i = 0; i < trails.Length; i++)
        {
            trails[i].time = length;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity = Vector3.zero;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMove(Vector3.zero, 1.5f));
        mySequence.Insert(0,transform.DOLocalRotate(Vector3.zero, 1.5f));
        mySequence.OnComplete(StopRotating);
        mySequence.Play();

    }

    private void StopRotating()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            GameController.Instance.SetLifePoints(-other.gameObject.GetComponent<BaseProjectile>().damage);
        }
    }

}
