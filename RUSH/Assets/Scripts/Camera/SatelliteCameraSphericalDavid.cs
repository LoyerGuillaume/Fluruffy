using UnityEngine;
using System.Collections;
using MathTools;

public class SatelliteCameraSphericalDavid : BaseManager<SatelliteCameraSphericalDavid>
{
    public CoordSystem.Spherical minCoord, maxCoord, currCoord, speedCoord;
    private CoordSystem.Spherical targetCoord;
    public Transform target;
    public float kLerpPos;

    private bool isActive = false;

    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }


    protected override void Play(params object[] prms)
    {
        print("LEVELMANAGER : PLAY");
    }

    void Start()
    {
        minCoord.ConvertThetaPhiToRad();
        maxCoord.ConvertThetaPhiToRad();
        currCoord.ConvertThetaPhiToRad();
        speedCoord.ConvertThetaPhiToRad();
        targetCoord = new CoordSystem.Spherical(currCoord);
    }

    void Update()
    {
        //targetCoord = new CoordSystem.Spherical (Mathf.Clamp (targetCoord.rho - speedCoord.rho * Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime, minCoord.rho, maxCoord.rho), targetCoord.theta + Time.deltaTime * (Input.GetKey (KeyCode.Q) ? -speedCoord.theta : (Input.GetKey (KeyCode.D) ? speedCoord.theta : 0)), Mathf.Clamp (targetCoord.phi + Time.deltaTime * (Input.GetKey (KeyCode.Z) ? -speedCoord.phi : (Input.GetKey (KeyCode.S) ? speedCoord.phi : 0)), minCoord.phi, maxCoord.phi));
        //                targetCoord.theta += Time.deltaTime * (Input.GetKey (KeyCode.Q) ? -speedCoord.theta : (Input.GetKey (KeyCode.D) ? speedCoord.theta : 0));
        //                targetCoord.phi = Mathf.Clamp (targetCoord.phi + Time.deltaTime * (Input.GetKey (KeyCode.Z) ? -speedCoord.phi : (Input.GetKey (KeyCode.S) ? speedCoord.phi : 0)), minCoord.phi, maxCoord.phi);
        //                targetCoord.rho = Mathf.Clamp (targetCoord.rho - speedCoord.rho * Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime, minCoord.rho, maxCoord.rho);
        //currCoord = CoordSystem.Spherical.Lerp (currCoord, targetCoord, Time.deltaTime * kLerpPos);
        //transform.position = CoordSystem.SphericalToCartesian (currCoord);

        transform.position =
            CoordSystem.SphericalToCartesian(
                currCoord = CoordSystem.Spherical.Lerp(
                    currCoord, targetCoord = new CoordSystem.Spherical(
                        Mathf.Clamp(targetCoord.rho - speedCoord.rho * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime, minCoord.rho, maxCoord.rho),
                        targetCoord.theta + Time.deltaTime * (Input.GetKey(KeyCode.Q) ? -speedCoord.theta : (Input.GetKey(KeyCode.D) ? speedCoord.theta : 0)),
                        Mathf.Clamp(targetCoord.phi + Time.deltaTime * (Input.GetKey(KeyCode.Z) ? -speedCoord.phi : (Input.GetKey(KeyCode.S) ? speedCoord.phi : 0)),
                         minCoord.phi, maxCoord.phi)), Time.deltaTime * kLerpPos));
        transform.LookAt(target);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height - 45, Screen.width, 20), "ZQSD and mouse wheel to navigate");
    }
}

