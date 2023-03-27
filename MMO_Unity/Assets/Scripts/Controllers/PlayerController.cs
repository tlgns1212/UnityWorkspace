using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    
    PlayerStat _stat;
    protected bool _stopSkill = false;
    [SerializeField]
    protected Camera _mainCamera;
    [SerializeField]
    protected float turnSmoothing = 0.06f;

    private Rigidbody RBody { get; set; }
    private Vector3 _lastDirection;
    private float horizontalInput;
    private float verticalInput;

    public void SetCamera(Camera camera) { _mainCamera = camera; }

    public override void Init()
    {
        _stat = GetComponent<PlayerStat>();
        RBody = GetComponent<Rigidbody>();
        WorldObjectType = Define.WorldObject.Player;

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    
    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if( _lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            //NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            
            //nma.CalculatePath
            //nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up *0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }        
    }
    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }


        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }

        
    }

    void OnKeyboard(Define.ArrowKeyEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnArrowKeyEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnArrowKeyEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.ArrowKeyEvent.PressUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    // Rotate the player to match correct orientation, according to camera and key pressed.
    Vector3 Rotating()
    {
        // Get camera forward direction, without vertical component.
        Vector3 forward = _mainCamera.transform.TransformDirection(Vector3.forward);

        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculate target direction based on camera forward and direction key.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * verticalInput + right * horizontalInput;

        // Lerp current direction to calculated target direction.
        if ((IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(RBody.rotation, targetRotation, turnSmoothing);
            RBody.MoveRotation(newRotation);
            _lastDirection = targetDirection;
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(horizontalInput) > 0.9 || Mathf.Abs(verticalInput) > 0.9))
        {
            Repositioning();
        }

        return targetDirection;
    }


    // Check if the player is moving.
    public bool IsMoving()
    {
        return (horizontalInput != 0) || (verticalInput != 0);
    }

    public void Repositioning()
    {
        if (_lastDirection != Vector3.zero)
        {
            _lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(_lastDirection);
            Quaternion newRotation = Quaternion.Slerp(RBody.rotation, targetRotation, turnSmoothing);
            RBody.MoveRotation(newRotation);
        }
    }


    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if(evt == Define.MouseEvent.PointerUp) 
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnArrowKeyEvent_IdleRun(Define.ArrowKeyEvent evt)
    {
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        
        //Rotating();
        ////transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward + new Vector3(0.1f, 0, 0)), 0.2f);
        //transform.Translate(_lastDirection * _stat.MoveSpeed * Time.deltaTime, Space.Self);


        switch (evt)
        {
            case Define.ArrowKeyEvent.PressDown:
                {
                    State = Define.State.Moving;
                    _destPos = Rotating(); 
                    //transform.Translate(transform.TransformDirection(transform.forward) * _stat.MoveSpeed * Time.deltaTime, Space.Self);
                    _destPos += transform.position + transform.forward * 0.1f;
                    //transform.Translate(_lastDirection * _stat.MoveSpeed * Time.deltaTime, Space.Self);
                    _stopSkill = false;
                }
                break;
            case Define.ArrowKeyEvent.Press:
                {
                    _destPos = Rotating();
                    //transform.Translate(transform.TransformDirection(transform.forward) * _stat.MoveSpeed * Time.deltaTime, Space.Self);
                    _destPos += transform.position + transform.forward * 0.1f;
                    //transform.Translate(_lastDirection * _stat.MoveSpeed * Time.deltaTime, Space.Self);
                }
                break;
            case Define.ArrowKeyEvent.PressUp:
                {
                    _stopSkill = true;
                    _destPos = transform.position;
                    State = Define.State.Idle;
                }
                break;
        }
    }


    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        _destPos.y = 0;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                    {
                        _destPos = hit.point;
                        _destPos.y = 0;
                    }
                        
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }

}
