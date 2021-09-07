using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private const float EFFECT_DELAY = 6F;

    public static event System.Action<int> onLevelChanged;

    public event System.Action<int> onPointsUpdated;
    public event System.Action<int> onRestartPlayerRequested;
    public event System.Action onNextLevelRequested;
    public event System.Action onGameOver;

    public Color defaultBlockColor;
    public Color effectBlockColor;

    public int level { get { return _level; } }
    public int lives { get { return _lives; } }
    public int points { get { return _points; } }

    private int _level = 1;
    private int _lives = 3;
    private int _points;
    private int _totalCoins;

    private bool _effectPlaying, _effectSwitch;
    private float _effectTimer = EFFECT_DELAY;
    private List<BuildObject> _blockTiles = new List<BuildObject>(500);

    public void Initiate()
    {
        PlayerCharacter.onCoinCollected += Coin_onCoinCollected;

        Foe.onPlayerCaptured += Foe_onPlayerCaptured;
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if(!_effectPlaying)
        {
            _effectTimer -= Time.deltaTime;

            if (_effectTimer <= 0)
            {
                _effectPlaying = true;
                
                if(_effectSwitch)
                {
                    StartCoroutine(RoutineStageEffect02());
                }
                else
                {
                    StartCoroutine(RoutineStageEffect01());
                }

                _effectSwitch = !_effectSwitch;
                _effectTimer = EFFECT_DELAY;
            }
        }
        
    }

    private void OnDestroy()
    {
        PlayerCharacter.onCoinCollected -= Coin_onCoinCollected;

        Foe.onPlayerCaptured -= Foe_onPlayerCaptured;
    }

    public void InitializeStage(List<BuildObject> p_blockTiles, int p_totalCoins)
    {
        _totalCoins = p_totalCoins;
        _blockTiles = p_blockTiles;
    }

    public void Restart(bool p_fullRestart)
    {
        StopAllCoroutines();

        if(p_fullRestart)
        {
            _points = 0;
            ChangeLevel(1);
        }

        _lives = 3;
        _effectTimer = EFFECT_DELAY;
        _effectPlaying = false;
    }

    public void ChangeLevel(int p_level)
    {
        _level = p_level;

        onLevelChanged?.Invoke(_level);
    }

    private void Foe_onPlayerCaptured(Character p_agent)
    {
        _lives--;

        CameraManager.ShakeScreen(0.3f, 0.3f);
        Instantiate(ParticleDatabase.Instance.swordHitGreenCritical, p_agent.transform.localPosition, Quaternion.identity);

        if (_lives == 0)
        {
            onGameOver?.Invoke();
        }
        else
        {
            onRestartPlayerRequested?.Invoke(_lives);
        }
    }

    private void Coin_onCoinCollected()
    {
        _points += 10;
        _totalCoins--;

        onPointsUpdated?.Invoke(_points);
        
        if(_totalCoins == 0)
        {
            onNextLevelRequested?.Invoke();
        }
    }

    private IEnumerator RoutineStageEffect01()
    {
        int __count = 5;
        int __effectSwitch = 0;
        WaitForSeconds __twoSeconds = new WaitForSeconds(2f);

        while(__count > 0)
        {
            for (int __i = 0; __i < _blockTiles.Count; __i++)
            {
                if (__i % 2 == __effectSwitch)
                    _blockTiles[__i].spriteRenderer.color = effectBlockColor;
                else
                    _blockTiles[__i].spriteRenderer.color = defaultBlockColor;
            }

            __count--;
            __effectSwitch = __effectSwitch == 0 ? 1 : 0;

            yield return __twoSeconds;
        }

        for (int __i = 0; __i < _blockTiles.Count; __i++)
        {
             _blockTiles[__i].spriteRenderer.color = defaultBlockColor;
        }

        _effectPlaying = false;
    }

    private IEnumerator RoutineStageEffect02()
    {
        for (int __i = 0; __i < _blockTiles.Count; __i++)
        {
            _blockTiles[__i].spriteRenderer.color = effectBlockColor;

            yield return null;
        }

        yield return new WaitForSeconds(4f);

        for (int __i = 0; __i < _blockTiles.Count; __i++)
        {
            _blockTiles[__i].spriteRenderer.color = defaultBlockColor;

            yield return null;
        }

        _effectPlaying = false;
    }
}
