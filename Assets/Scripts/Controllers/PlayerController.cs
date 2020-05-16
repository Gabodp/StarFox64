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
    private readonly float moveForwardSpeed = 22.0f;

    public Transform aimTargetRotation;
    public Transform aimTarget;
    private PlayerShootingSystem s_system;
    private Rigidbody rb;
    private int right;
    private int left;


    private void Start()
    {
        player = transform.GetChild(0);
        s_system = GetComponent<PlayerShootingSystem>();
        right = 0;
        left = 0;
        dolly_cart.m_Speed = moveForwardSpeed;
        rb = GetComponent<Rigidbody>();

    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        XY_Move(horizontal, vertical, speed);
        RotationLook(horizontal, vertical, lookSpeed);
        HorizontalLean(player, horizontal, 60, .1f);
        KeyBoardInput();
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
            right++;
            left = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            left++;
            right = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SpeedUp(true);
        if (Input.GetKeyUp(KeyCode.LeftShift))
            SpeedUp(false);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Brake(true);
        if (Input.GetKeyUp(KeyCode.LeftControl))
            Brake(false);
        /*if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.localPosition = Vector3.zero;
        }*/
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
        float newForwardSpeed = activated ? moveForwardSpeed * 2 : moveForwardSpeed;
        float zoom = activated ? -4 : 0;
        float fov = activated ? 75 : 65;
        DOVirtual.Float(dolly_cart.m_Speed, newForwardSpeed, .15f, SetForwardSpeed);
        GameController.Instance.SetCameraZoom(zoom, 0.5f);
        GameController.Instance.FieldOfView(fov);
    }

    private void Brake(bool activated)
    {
        float newForwardSpeed = activated ? moveForwardSpeed * 0.60f : moveForwardSpeed;
        float zoom = activated ? 2 : 0;
        float fov = activated ? 50 : 65;
        DOVirtual.Float(dolly_cart.m_Speed, newForwardSpeed, .15f, SetForwardSpeed);
        GameController.Instance.SetCameraZoom(zoom, 0.3f);
        GameController.Instance.FieldOfView(fov);
    }

    void BarrelRoll(int direction)
    {
        if (!DOTween.IsTweening(player))
        {
            player.DOLocalRotate(new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 360 * -direction), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
            right = 0;
            left = 0;
        }
    }

    void SetForwardSpeed(float newSpeed)
    {
        dolly_cart.m_Speed = newSpeed;
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
        print("Termino el tween");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameController.Instance.SetLifePoints(-10);
        }
    }

}
