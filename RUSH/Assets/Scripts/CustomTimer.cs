using UnityEngine;
using System.Collections;

public class CustomTimer : MonoBehaviour
{

    public static CustomTimer manager;

    public bool IsRunning { get; private set; }

    float _elapsedTime = 0;

    [SerializeField]
    private float speedTime = 2f;

    public float DeltaTime
    {
        get
        {
            return IsRunning ? Time.deltaTime * speedTime : 0;
        }
    }

    public float FixedDeltaTime
    {
        get
        {
            return IsRunning ? Time.fixedDeltaTime * speedTime : 0;
        }
    }


    public float ElapsedTime
    {
        get
        {
            return _elapsedTime;
        }
        private set
        {
            _elapsedTime = value;
        }
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void Reset()
    {
        ElapsedTime = 0;
    }

    public void Reset(bool startTimer)
    {
        ElapsedTime = 0;
        IsRunning = startTimer;
    }

    public void ResetAndStart()
    {
        Reset(true);
    }

    public void ResetAndStop()
    {
        Reset(false);
    }


    void Awake()
    {
        manager = this;
    }

    // Use this for initialization
    void Start()
    {
        Reset(false);
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += DeltaTime;
    }
}
