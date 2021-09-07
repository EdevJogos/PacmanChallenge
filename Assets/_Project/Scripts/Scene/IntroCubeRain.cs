using System.Collections.Generic;
using UnityEngine;

public class IntroCubeRain : MonoBehaviour
{
    public CubeDrop cubeDropPrefab;

    public float cubeDropDelay = 0.23f;

    private List<CubeDrop> _cubeDropsPool = new List<CubeDrop>(20);

    private void Awake()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    } 

    private void Start()
    {
        for (int __i = 0; __i < 20; __i++)
        {
            _cubeDropsPool.Add(Instantiate(cubeDropPrefab));
        }
    }

    private void Update()
    {
        if (GameCEO.State != GameState.INTRO)
            return;

        cubeDropDelay -= Time.deltaTime;

        if(cubeDropDelay <= 0)
        {
            GetAvaliableCubeDrop().Active(new Vector2(Random.Range(0, 18), 34f));

            cubeDropDelay = 0.23f;
        }
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private CubeDrop GetAvaliableCubeDrop()
    {
        for (int __i = 0; __i < 20; __i++)
        {
            if(!_cubeDropsPool[__i].isActiveAndEnabled)
            {
                return _cubeDropsPool[__i];
            }
        }

        return null;
    }

    private void GameCEO_onGameStateChanged()
    {
        if (_cubeDropsPool.Count == 0)
            return;

        for (int __i = 0; __i < 20; __i++)
        {
            if (_cubeDropsPool[__i].isActiveAndEnabled)
            {
                _cubeDropsPool[__i].Disable();
            }
        }
    }
}
