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
    private float moveForwardSpeed = 12;

    public Transform aimTargetRotation;
    public Transform aimTarget;
    private PlayerShootingSystem s_system;
    private int right;
    private int left;

    private void Start()
    {
        player = transform.GetChild(0);
        s_system = GetComponent<PlayerShootingSystem>();
        right = 0;
        left = 0;
        dolly_cart.m_Speed = moveForwardSpeed;

    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        LocalMove(horizontal, vertical, speed);
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
            
    }



    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();

        if (right == 2)
            BarrelRoll(1);
        else if (left == 2)
            BarrelRoll(-1);
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
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
        float speed = activated ? moveForwardSpeed * 3 : moveForwardSpeed;

        DOVirtual.Float(dolly_cart.m_Speed, speed, .15f, SetForwardSpeed);
    }

    private void Brake(bool activated)
    {
        float speed = activated ? moveForwardSpeed / 3 : moveForwardSpeed;

        DOVirtual.Float(dolly_cart.m_Speed, speed, .15f, SetForwardSpeed);
    }

    void BarrelRoll(int direction)
    {
        if (!DOTween.IsTweening(player))
        {
            player.DOLocalRotate(new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 360 * -direction), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
            right= 0;
            left = 0;
        }
    }

    void SetForwardSpeed(float x)
    {
        dolly_cart.m_Speed = x;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTargetRotation.position, .5f);
        Gizmos.DrawSphere(aimTargetRotation.position, .15f);

    }*/
}
