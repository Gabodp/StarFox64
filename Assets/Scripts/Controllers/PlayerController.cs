
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    private Transform player;

    //Basic Parameters
    public CinemachineDollyCart dolly_cart;
    public float speed;
    public GameObject explosion;

    private readonly float lookSpeed = 8000;
    private float moveForwardSpeed = 20.0f;

    public Transform aimTargetRotation;
    private PlayerShootingSystem s_system;
    private Rigidbody rb;
    private int right,left;
    private bool canChangeScene;
    private bool alive;

    public TrailRenderer[] trails;

    //Boost parameters
    private bool canBoost;
    private bool isRefilling;
    private float boostCapacity;
    private bool boosting;

    private void Start()
    {
        player = transform.GetChild(0);
        canChangeScene = true;
        alive = true;
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
        if (!alive) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        canBoost = boostCapacity > 0 && !isRefilling ? true : false;

        XY_Move(horizontal, vertical, speed);
        RotationLook(horizontal, vertical, lookSpeed);
        LeanLateral(player, horizontal, 60, .1f);
        KeyBoardInput();

        if (boosting)
        {
            boostCapacity = Mathf.Clamp(boostCapacity - (Time.deltaTime * 20), 0f, 100f);//Esto hara que el tiempo maximo de boost sea de 5 segundos

            GameController.Instance.SetBoostPoints(boostCapacity);
            if (boostCapacity == 0f && !GameController.Instance.godMode)
            {

                SpeedUp(false);
                isRefilling = true;
                //float refillTime = (100f - boostCapacity) / 5.f;
                DOVirtual.Float(0f, 100f, 5f, RefillBoostTank).OnComplete(RefillFinished);

            }
                
        }

        if (canChangeScene && dolly_cart.m_Position > GameController.Instance.GetPosToChangeLevel())
        {
            canChangeScene = false;
            GameController.Instance.loader.LoadNextLevel();
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
        ClamPosToViewPort();

        if (right == 2)
            BarrelRoll(1);
        else if (left == 2)
            BarrelRoll(-1);
    }

    void ClamPosToViewPort()
    {
        Vector3 newPos = Camera.main.WorldToViewportPoint(transform.position);
        newPos.x = Mathf.Clamp(newPos.x,0.08f,0.92f);

        newPos.y = Mathf.Clamp(newPos.y,0.08f,0.92f);

        transform.position = Camera.main.ViewportToWorldPoint(newPos);
    }

    void RotationLook(float horizontal, float vertical, float rotationSpeed)
    {
        aimTargetRotation.parent.position = Vector3.zero;

        aimTargetRotation.localPosition = new Vector3(horizontal, vertical, 3.0f);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTargetRotation.position), Mathf.Deg2Rad * rotationSpeed * Time.deltaTime);
    }

    void LeanLateral(Transform playerTransform, float axis, float rotationLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = playerTransform.localEulerAngles;
        playerTransform.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * rotationLimit, lerpTime));
    }

    private void SpeedUp(bool activated)
    {
        if (activated)
        {
            GameController.Instance.ShakeCamera(5.0f, 0.4f);
            AudioManager.PlaySound(AudioManager.Sound.SpeedUp);
        }
            

        boosting = activated;
        float newForwardSpeed = activated ? moveForwardSpeed * 2 : moveForwardSpeed;
        float zoom = activated ? -3 : 0;
        float fov_start = activated ? 60 : 75;
        float fov_end = activated ? 75 : 60;

        float trail_start = activated ? 0.08f : 0.2f;
        float trail_end = activated ? 0.2f : 0.08f;

        while (DOTween.IsTweening(dolly_cart.m_Speed))
            print("Its tweening m_speed");

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

    public void StopPlaying()
    {
        alive = false;
        GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioManager.PlaySound(AudioManager.Sound.DestroyExplosion);
        //Destroy(this.gameObject, 0.2f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        
        Destroy(obj, 1f);
        
    }

    private void RefillBoostTank(float value)
    {
        boostCapacity = value;
        GameController.Instance.SetBoostPoints(value);

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

        print(collision.gameObject.name);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMove(Vector3.zero, 1.5f));
        mySequence.Insert(0,transform.DOLocalRotate(Vector3.zero, 1.5f));
        mySequence.OnComplete(StopRotating);
        mySequence.Play();

        if (collision.gameObject.CompareTag("Terrain"))
            GameController.Instance.SetLifePoints(-25);
        else if(collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("Enemy"))
            GameController.Instance.SetLifePoints(-20);

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
            print("hit");
            GameController.Instance.SetLifePoints(-other.gameObject.GetComponent<BaseProjectile>().damage);
        }
        else if (other.CompareTag("Booster")) {
            moveForwardSpeed = 100.0f;
            DOVirtual.Float(dolly_cart.m_Speed, moveForwardSpeed, .5f, SetForwardSpeed);
        }
    }

}
