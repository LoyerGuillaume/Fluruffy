//using UnityEngine;
//using System.Collections;
//using MathTools;

//public class SatelliteCameraSpherical : MonoBehaviour
//{

//    public float minRho;
//    public float maxRho;

//    public float phi;
//    float targetPhi;

//    public float minPhi;
//    public float maxPhi;

//    public float thetaSpeed;
//    public float rhoSpeed;
//    public float phiSpeed;

//    public Transform target;

//    public float startTheta = 45f;
//    public float startRho = 45f;

//    float theta;
//    float targetTheta;

//    float rho;
//    float targetRho;

//    public float kLerpPos;

//    Vector3 targetPos;

//    // Use this for initialization
//    void Start()
//    {
//        rho = startRho;
//        targetRho = rho;

//        theta = startTheta;
//        targetTheta = theta;

//        minPhi *= Mathf.PI / 180f;
//        maxPhi *= Mathf.PI / 180f;
//        phi *= Mathf.PI / 180f;
//        targetPhi = phi;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKey(KeyCode.Q))
//            targetTheta -= thetaSpeed * Time.deltaTime;
//        else if (Input.GetKey(KeyCode.D))
//            targetTheta += thetaSpeed * Time.deltaTime;
//        theta = Mathf.Lerp(theta, targetTheta, Time.deltaTime * kLerpPos);

//        if (Input.GetKey(KeyCode.Z))
//            targetPhi -= phiSpeed * Time.deltaTime;
//        else if (Input.GetKey(KeyCode.S))
//            targetPhi += phiSpeed * Time.deltaTime;

//        targetPhi = Mathf.Clamp(targetPhi, minPhi, maxPhi);
//        phi = Mathf.Lerp(phi, targetPhi, Time.deltaTime * kLerpPos);

//        targetRho = Mathf.Clamp(targetRho - rhoSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime, minRho, maxRho);
//        rho = Mathf.Lerp(rho, targetRho, Time.deltaTime * kLerpPos);

//        transform.position = CoordSystem.SphericalToCartesian(new CoordSystem.Spherical(rho, theta, phi));

//        transform.LookAt(target);
//    }
//}

using UnityEngine;
using MathTools;
using System.Collections;

public class SatelliteCameraSpherical : MonoBehaviour
{

    public float minDist;
    public float maxDist;

    public float minElevation;
    public float maxElevation;

    public float elevation;
    float targetElevation;

    public float azimutSpeed;
    public float distanceSpeed;
    public float elevationSpeed;

    public Transform target;

    public float startAzimut = 45f;

    float azimut;
    float targetAzimut;

    float distance;
    float targetDistance;

    public float kLerpPos;

    Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        distance = (minDist + maxDist) / 2f;
        targetDistance = distance;

        azimut = startAzimut;
        targetAzimut = azimut;

        elevation *= Mathf.PI / 180f;
        targetElevation = elevation;

        minElevation *= Mathf.PI / 180f;
        maxElevation *= Mathf.PI / 180f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Q))
            targetAzimut -= azimutSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            targetAzimut += azimutSpeed * Time.deltaTime;
        azimut = Mathf.Lerp(azimut, targetAzimut, Time.deltaTime * kLerpPos);

        if (Input.GetKey(KeyCode.Z))
            targetElevation += elevationSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            targetElevation -= elevationSpeed * Time.deltaTime;
        targetElevation = Mathf.Clamp(targetElevation, minElevation, maxElevation);
        elevation = Mathf.Lerp(elevation, targetElevation, Time.deltaTime * kLerpPos);

        targetDistance = Mathf.Clamp(targetDistance - distanceSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime, minDist, maxDist);
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * kLerpPos);



        transform.position = CoordSystem.SphericalToCartesian(new CoordSystem.Spherical(distance, azimut, elevation));

        transform.LookAt(target);

        //Vector3 dirH = new Vector3(Mathf.Cos(azimut), 0, Mathf.Sin(azimut));

        //Vector3 newPos = target.position + dirH * distance * Mathf.Cos(elevation) + Vector3.up * distance * Mathf.Sin(elevation);
        //transform.position = newPos;//Vector3.Lerp(transform.position,newPos,Time.deltaTime*kLerpPos);
        //transform.LookAt(target);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(10,Screen.height-45,Screen.width,20),"ZQSD and mouse wheel to navigate");
    }
}
