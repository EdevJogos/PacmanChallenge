using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Vector2 MouseWorldPosition
    {
        get
        {
            return MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private static bool _ScreenShake;
    private static float _Strength;
    private static float _ScreenShakeTime;
    private static Vector3 _OriginalPosition;

    private Vector3 _randomVector;

    public static Camera MainCamera;

    public void Initiate()
    {
        MainCamera = Camera.main;
        _OriginalPosition = MainCamera.transform.localPosition;
    }

    private void Update()
    {
        if (_ScreenShake && MainCamera.enabled)
        {
            _randomVector = Random.insideUnitSphere;

            MainCamera.transform.localPosition = new Vector3(_randomVector.x * _Strength, _randomVector.y * _Strength, -10f) + _OriginalPosition;

            _ScreenShakeTime -= Time.deltaTime;

            if (_ScreenShakeTime <= 0)
            {
                StopShake();
            }
        }
    }

    public static void ShakeScreen(float p_duration, float p_strength)
    {
        _Strength = p_strength;
        _ScreenShakeTime += p_duration;
        _ScreenShakeTime = Mathf.Clamp(_ScreenShakeTime, 0f, p_duration);

        _ScreenShake = true;
    }

    public static void StopShake()
    {
        _ScreenShake = false;
        _ScreenShakeTime = 0f;

        MainCamera.transform.localPosition = _OriginalPosition;
    }
}