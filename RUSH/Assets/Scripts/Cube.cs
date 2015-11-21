using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {

    private float angle = 90f;
    private Vector3 direction;
    private Vector3 directionConveyor;


    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 startScale;
    private Vector3 targetScale;
    private AnimationCurve currentAnimationCurvePosition;
    private AnimationCurve currentAnimationCurveScale;
    private delegate void FunctionCallback();
    private FunctionCallback CallbackCoroutine;

    private string currentTeleportColor; //FIXME

    public string CurrentTeleportColor
    {
        get
        {
            return currentTeleportColor;
        }
    }

    private LevelAction currentAction;
    private bool collisionWithAction;

    private delegate IEnumerator StateCoroutine();
    private StateCoroutine ActionCoroutine;

    [SerializeField]
    private string color;

    private STATE currentState;
    public enum STATE
    {
        pop,
        fall,
        walk,
        collisionWithWall,
        depop,
        actionStop,
        actionConveyor,
        actionTeleport
    };

    // Use this for initialization
    void Start () { 
        SetStatePop();
        direction = transform.forward;

        if (Metronome.manager)
        {
            Metronome.manager.onTic += StartTic;
        }

        StartCoroutine(ActionCoroutine());
    }
    

    #region SetState
    public void SetStatePop()
    {
        //startPosition = transform.position;
        targetPosition = transform.position + Vector3.up;

        startScale = transform.localScale - Vector3.one * 0.5f;
        targetScale = transform.localScale + Vector3.one * 0.5f;

        currentAnimationCurvePosition = Config.manager.CurvePopPositionCube;
        currentAnimationCurveScale = Config.manager.CurvePopScaleCube;

        CallbackCoroutine = new FunctionCallback(SetStateWalk);

        ActionCoroutine = new StateCoroutine(ChangePositionAndScaleCoroutine);
        //ActionCoroutine = new StateCoroutine(PopCoroutine);
        currentState = STATE.pop;
    }

    void SetStateWalk()
    {
        ActionCoroutine = new StateCoroutine(MovementCoroutine);
        currentState = STATE.walk;
    }
    
    void SetStateFall()
    {
        //startPosition = transform.position;
        targetPosition = transform.position - Vector3.up;

        currentAnimationCurvePosition = Config.manager.LinearAnimationCurve;

        CallbackCoroutine = () => { };
        ActionCoroutine = new StateCoroutine(ChangePositionCoroutine);

        //ActionCoroutine = new StateCoroutine(FallCoroutine);
        currentState = STATE.fall;
    }

    //void SetStateCollisionWithWall()
    //{
    //    startPosition = transform.position;
    //    targetPosition = startPosition + Vector3.up;
    //    currentAnimationCurvePosition = Config.manager.CubeCollisionWithWallAnimationCurve;
        
    //    CallbackCoroutine = new FunctionCallback(SetStateWalk);

    //    ActionCoroutine = new StateCoroutine(ChangePositionCoroutine);

    //    //ActionCoroutine = new StateCoroutine(AnimationStopCoroutine);
    //    currentState = STATE.collisionWithWall;
    //}

    void SetStateActionStop()
    {
        //startPosition = transform.position;
        targetPosition = transform.position + Vector3.up;
        currentAnimationCurvePosition = Config.manager.CubeCollisionWithWallAnimationCurve;

        CallbackCoroutine = new FunctionCallback(SetStateWalk);

        ActionCoroutine = new StateCoroutine(ChangePositionCoroutine);

        //ActionCoroutine = new StateCoroutine(AnimationStopCoroutine);
        currentState = STATE.actionStop;
    }

    void SetStateActionConveyor()
    {
        //startPosition = transform.position;
        targetPosition = transform.position + directionConveyor;

        currentAnimationCurvePosition = Config.manager.LinearAnimationCurve;

        CallbackCoroutine = new FunctionCallback(SetStateWalk);

        ActionCoroutine = new StateCoroutine(ChangePositionCoroutine);

        print("SetStateActionConveyor");
        //ActionCoroutine = new StateCoroutine(ActionConveyorCoroutine);
        currentState = STATE.actionConveyor;
    }

    void SetStateDepop()
    {

        //startPosition = transform.position;
        targetPosition = transform.position + Vector3.up;

        startScale = transform.localScale - Vector3.one * 0.5f;
        targetScale = transform.localScale + Vector3.one * 0.5f;

        currentAnimationCurvePosition = Config.manager.CurvePopPositionCube;
        currentAnimationCurveScale = Config.manager.CurveDesapearScaleCube;

        CallbackCoroutine = new FunctionCallback(CubeArrived);

        ActionCoroutine = new StateCoroutine(ChangePositionAndScaleCoroutine);

        //ActionCoroutine = new StateCoroutine(DisapearCoroutine);
        currentState = STATE.depop;
    }

    void SetStateActionTeleport()
    {
        //startPosition = transform.position;
        targetPosition = transform.position + Vector3.up;

        startScale = transform.localScale - Vector3.one * 0.5f;
        targetScale = transform.localScale + Vector3.one * 0.5f;

        currentAnimationCurvePosition = Config.manager.CurvePopPositionCube;
        currentAnimationCurveScale = Config.manager.CurveDesapearScaleCube;

        CallbackCoroutine = new FunctionCallback(CubeTeleport);

        ActionCoroutine = new StateCoroutine(ChangePositionAndScaleCoroutine);
        //ActionCoroutine = new StateCoroutine(DisapearCoroutine);
        currentState = STATE.actionTeleport;
    }
    #endregion

    private void StartTic ()
    {
        //print("CUBE : StartTic");
        collisionWithAction = false;
        CheckCollision();
        //print("============");
        //Debug.Break();
        if (!collisionWithAction) //FIXME
        {
            currentAction = null;
        }

        StartCoroutine(ActionCoroutine());
    }

    #region StateCoroutine


    IEnumerator ChangePositionAndScaleCoroutine()
    {
        startPosition = transform.position;

        while (Metronome.manager.RatioTic < 1)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentAnimationCurvePosition.Evaluate(Metronome.manager.RatioTic));
            transform.localScale = Vector3.Lerp(startScale, targetScale, currentAnimationCurveScale.Evaluate(Metronome.manager.RatioTic));
            yield return null;
        }

        CallbackCoroutine();
    }

    IEnumerator ChangePositionCoroutine()
    {
        startPosition = transform.position;

        while (Metronome.manager.RatioTic < 1)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentAnimationCurvePosition.Evaluate(Metronome.manager.RatioTic));
            yield return null;
        }

        CallbackCoroutine();
    }

    //IEnumerator PopCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + Vector3.up;

    //    Vector3 startScale = transform.localScale - Vector3.one * 0.5f;
    //    Vector3 targetScale = transform.localScale + Vector3.one * 0.5f;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Config.manager.CurvePopPositionCube.Evaluate(Metronome.manager.RatioTic));
    //        transform.localScale = Vector3.Lerp(startScale, targetScale, Config.manager.CurvePopScaleCube.Evaluate(Metronome.manager.RatioTic));
    //        yield return null;
    //    }

    //    SetStateWalk();
    //}


    //IEnumerator DisapearCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + Vector3.up;

    //    Vector3 startScale = transform.localScale - Vector3.one * 0.5f;
    //    Vector3 targetScale = transform.localScale + Vector3.one * 0.5f;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Config.manager.CurvePopPositionCube.Evaluate(1 - Metronome.manager.RatioTic));
    //        transform.localScale = Vector3.Lerp(startScale, targetScale, Config.manager.CurvePopScaleCube.Evaluate(1 - Metronome.manager.RatioTic));
    //        yield return null;
    //    }

    //    if (currentState == STATE.depop) // FIXME
    //    {
    //        SendMessageUpwards("CubeArrived", this);
    //    }
    //    else if (currentState == STATE.actionTeleport)
    //    {
    //        SendMessageUpwards("CubeTeleport", this);
    //    }

    //}


    //IEnumerator AnimationStopCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + Vector3.up;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Config.manager.CubeCollisionWithWallAnimationCurve.Evaluate(Metronome.manager.RatioTic));
    //        yield return null;
    //    }

    //    SetStateWalk();
    //}


    //IEnumerator FallCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition - Vector3.up;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Metronome.manager.RatioTic);
    //        yield return null;
    //    }
    //}


    IEnumerator MovementCoroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 vecAxis = Vector3.Cross(Vector3.up, direction);

        Quaternion qStartRotation = transform.rotation;
        Quaternion qRotation = Quaternion.AngleAxis(angle, vecAxis);
        Quaternion qTargetRotation = qRotation * transform.rotation;

        Quaternion qPositionStart = Quaternion.AngleAxis(0, vecAxis);
        Quaternion qPositionTarget = Quaternion.AngleAxis(angle, vecAxis);

        Vector3 offset = direction / 2;
        offset.y = -0.5f;
        
        while (Metronome.manager.RatioTic < 1)
        {
            float step = angle * CustomTimer.manager.DeltaTime; // FIXME don't use CustomTimer
            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTargetRotation, step);
            Quaternion qPosition = Quaternion.Slerp(qPositionStart, qPositionTarget, Metronome.manager.RatioTic);
            transform.position = startPosition + offset + qPosition * -offset;
            yield return null;
        }
        
        RoundPositionAndRotation();
    }
    
    //IEnumerator ActionConveyorCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + directionConveyor;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Metronome.manager.RatioTic);
    //        yield return null;
    //    }

    //    SetStateWalk();
    //}

    //IEnumerator ActionStopCoroutine()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + Vector3.up;

    //    Vector3 startScale = transform.localScale - Vector3.one * 0.5f;
    //    Vector3 targetScale = transform.localScale + Vector3.one * 0.5f;

    //    while (Metronome.manager.RatioTic < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, targetPosition, Config.manager.CurvePopPositionCube.Evaluate(Metronome.manager.RatioTic));
    //        transform.localScale = Vector3.Lerp(startScale, targetScale, Config.manager.CurvePopScaleCube.Evaluate(Metronome.manager.RatioTic));
    //        yield return null;
    //    }

    //    SetStateWalk();
    //}

    #endregion

    private void RoundPositionAndRotation()
    {
        Vector3 eulerAngle = transform.rotation.eulerAngles;

        eulerAngle.x = Mathf.RoundToInt(eulerAngle.x);
        eulerAngle.y = Mathf.RoundToInt(eulerAngle.y);
        eulerAngle.z = Mathf.RoundToInt(eulerAngle.z);

        transform.rotation.eulerAngles.Set(eulerAngle.x, eulerAngle.y, eulerAngle.z);

        Vector3 position = transform.position;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = Mathf.RoundToInt(position.z);

        transform.position = position;
    }

    void Update()
    {
        //CheckInput();
    }

    //void CheckInput()
    //{
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        direction = transform.forward;
    //    }
    //    else if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        direction = -transform.forward;
    //    }
    //    else if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        direction = -transform.right;
    //    }
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        direction = transform.right;
    //    }

    //}

    #region Collision
    //void CheckCollision()
    //{
    //    if (CheckCollisionWithActions())
    //    {
    //        if (CheckCollisionWithWall())
    //        {
    //            OnCollisionWithWall();
    //        }
    //        return;
    //    }

    //    if (CheckCollisionWithTarget())
    //    {
    //        SetStateDepop();
    //        return;
    //    }

    //    if (CheckCollisionWithWall())
    //    {
    //        OnCollisionWithWall();
    //        return;
    //        //print("CUBE - COLLISION AVEC MUR");
    //    }

    //    if (CheckCollisionWithLevelElement())
    //    {
    //        SetStateWalk();
    //        return;
    //        //print("CUBE - PAS DE SOL");
    //    }
    //    else
    //    {
    //        SetStateFall();
    //        return;
    //    }
    //}

    void CubeArrived()
    {
        SendMessageUpwards("CubeArrived", this);
    }

    void CubeTeleport()
    {
        SendMessageUpwards("CubeTeleport", this);
    }

    void OnCollisionWithWall ()
    {
        SetStateActionStop();
        direction = Vector3.Cross(Vector3.up, direction);
    }

    void CheckCollision()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 1);
        for (int i = 0; i < hits.Length; i++)
        {
            //print("NAME : "+hits[i].collider.name);
            if (CheckCollisionWithAction(hits[i]))
            {
                collisionWithAction = true;
                if (CheckCollisionWithWall()) //FIXME
                {
                    OnCollisionWithWall();
                }
                return;
            }

            if (CheckCollisionWithTarget(hits[i]))
            {
                SetStateDepop();
                return;
            }

            if (CheckCollisionWithWall()) // FIXME
            {
                OnCollisionWithWall();
                return;
                //print("CUBE - COLLISION AVEC MUR");
            }

            //if (CheckCollisionWithLevelElement(hits[i]))
            //{
            //    SetStateWalk();
            //    return;
            //}
        }

        if (hits.Length == 0 || (hits.Length == 1 && hits[0].collider.CompareTag("DeathZone")))
        {
            SetStateFall();
        }

        return;

    }

    bool CheckCollisionWithAction(RaycastHit hit)
    {
        LevelAction levelAction = hit.collider.GetComponent<LevelAction>();
        if (levelAction != null)
        {
            print("COLLISION WITH ACTION");
            if (currentAction != levelAction)
            {
                currentAction = levelAction;
                if (levelAction is LevelActionArrow)
                {
                    direction = hit.collider.transform.forward;
                }
                else if (levelAction is LevelActionStop)
                {
                    SetStateActionStop();
                }
                else if (levelAction is LevelActionConveyors)
                {
                    directionConveyor = hit.collider.transform.forward;
                    SetStateActionConveyor();
                }
                else if (levelAction is LevelActionTeleport)
                {
                    LevelActionTeleport levelActionTeleport = (LevelActionTeleport)levelAction;
                    currentTeleportColor = levelActionTeleport.color;
                    levelActionTeleport.doTeleportCube = true;
                    SetStateActionTeleport();
                }
            }
            return true;
        }

        return false;
    }


    bool CheckCollisionWithTarget(RaycastHit hit)
    {
        Target target = hit.collider.GetComponent<Target>();
        if (target != null)
        {
            if (target.color == color)
            {
                return true;
            }
        }

        return false;
    }

    //bool CheckCollisionWithLevelElement(RaycastHit hit)
    //{
    //    return hit.collider.CompareTag("LevelElement");
    //}

    //bool CheckCollisionWithActions()
    //{
    //    RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, 1);
    //    for (int i = 0; i < hits.Length; i++)
    //    {
    //        LevelAction levelAction = hits[i].collider.GetComponent<LevelAction>();
    //        if (levelAction != null)
    //        {
    //            if (currentAction != levelAction)
    //            {
    //                currentAction = levelAction;
    //                if (levelAction is LevelActionArrow)
    //                {
    //                    direction = hits[i].collider.transform.forward;
    //                }
    //                else if (levelAction is LevelActionStop)
    //                {
    //                    SetStateActionStop();
    //                }
    //                else if (levelAction is LevelActionConveyors)
    //                {
    //                    directionConveyor = hits[i].collider.transform.forward;
    //                    SetStateActionConveyor();
    //                }
    //            }
    //            else
    //            {
    //                currentAction = null;
    //            }
    //            return true;
    //        }
    //    }

    //    currentAction = null;
    //    return false;
    //}

    //bool CheckCollisionWithTarget()
    //{
    //    RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, 1);
    //    for (int i = 0; i < hits.Length; i++)
    //    {
    //        Target target = hits[i].collider.GetComponent<Target>();
    //        if (target != null)
    //        {
    //            if (target.color == color)
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    //bool CheckCollisionWithLevelElement()
    //{
    //    RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, 1);

    //    for (int i = 0; i < hits.Length; i++)
    //    {
    //        if (hits[i].collider.CompareTag("LevelElement"))
    //        {
    //            return true;
    //        }

    //    }

    //    return false;
    //}


    bool CheckCollisionWithWall()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, direction, out hit, 1) && hit.collider.CompareTag("LevelElement");
    }

    #endregion


    void OnTriggerEnter(Collider colliderItem)
    {
        if (currentState != STATE.pop && (colliderItem.tag == "DeathZone" || colliderItem.tag == "Cube"))
        {
            print("CUBE - GAME OVER");
            SendMessageUpwards("CubeCollidedWithDeathZone");
        }
    }

    public void OnDestroy()
    {
        if (Metronome.manager)
        {
            Metronome.manager.onTic -= StartTic;
        }
    }
}
