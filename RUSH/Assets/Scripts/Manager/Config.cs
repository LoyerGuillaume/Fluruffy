using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {


    public static Config manager;
    private Config() { }

    void Awake()
    {
        manager = this;
    }
    

    [SerializeField]
    private AnimationCurve cubeCollisionWithWallAnimationCurve;

    public AnimationCurve CubeCollisionWithWallAnimationCurve
    {
        get
        {
            return cubeCollisionWithWallAnimationCurve;
        }
    }


    [SerializeField]
    private AnimationCurve curvePopPositionCube;

    public AnimationCurve CurvePopPositionCube
    {
        get
        {
            return curvePopPositionCube;
        }
    }

    [SerializeField]
    private AnimationCurve curvePopScaleCube;

    public AnimationCurve CurvePopScaleCube
    {
        get
        {
            return curvePopScaleCube;
        }
    }
}
