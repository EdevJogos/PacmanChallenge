using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum Phase
    {
        BUILD,
        PLAYER_POSITION,
        FOE_POSITION,
        IDLE = 99,
    }

    public event System.Action<Phase> onBuildPhaseChanged;
    public event System.Action<int> onStageBuildCompleted;
    public event System.Action<string> onBuildWaringRequested;

    public Camera mainCamera;
    public Wall wallPrefab;
    public Block blockPrefab;
    public Floor floorPrefab;
    public Coin coinPrefab;
    public Transform playerStartPos, foeStartPos;
    public Transform defaultPlayerStart, defaultFoeStart;
    public Transform defaultBlockTilesHolder;

    public List<BuildObject> blockTiles
    {
        get { return _blockTiles; }
    }

    private bool _playerStartSet, _foeStartSet;
    private Phase _currentPhase = Phase.IDLE;
    private Vector2 _lastPressCenter;
    private List<Coin> _coins = new List<Coin>(594);
    private List<BuildObject> _floorTiles = new List<BuildObject>(594);
    private List<BuildObject> _blockTiles = new List<BuildObject>(500);
    private List<BuildObject> _wallTiles = new List<BuildObject>(41);
    private List<List<BuildObject>> _buildObjects = new List<List<BuildObject>>(3);

    private void Awake()
    {
        _buildObjects.Add(_floorTiles);
        _buildObjects.Add(_wallTiles);
        _buildObjects.Add(_blockTiles);
    }

    private void Start()
    {
        Vector2 __currentPosition = Vector3.zero;
        Vector2 __offset = new Vector2(1, 0);

        for (int __i = 0; __i <= 23; __i++)
        {
            for (int __j = 0; __j <= 18; __j++)
            {
                __currentPosition.Set(__j, __i);

                bool __placeWall = (__j == 0 || __j == 18 || __i == 0 || __i == 23);

                if(__placeWall)
                {
                    Wall __wall = Instantiate(wallPrefab, __currentPosition, Quaternion.identity);
                    __wall.Initiate(__currentPosition);

                    _wallTiles.Add(__wall);
                }
                else
                {
                    Floor __floor = Instantiate(floorPrefab, __currentPosition, Quaternion.identity);
                    __floor.Initiate(__currentPosition);

                    _floorTiles.Add(__floor);
                }
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentPhase = Phase.BUILD;
            onBuildPhaseChanged?.Invoke(_currentPhase);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentPhase = Phase.PLAYER_POSITION;
            playerStartPos.gameObject.SetActive(true);
            onBuildPhaseChanged?.Invoke(_currentPhase);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentPhase = Phase.FOE_POSITION;
            foeStartPos.gameObject.SetActive(true);
            onBuildPhaseChanged?.Invoke(_currentPhase);
        }

        switch (_currentPhase)
        {
            case Phase.BUILD:
                BuildPhase();
                break;
            case Phase.PLAYER_POSITION:
                PlayerPhase();
                break;
            case Phase.FOE_POSITION:
                FoePhase();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_playerStartSet && _foeStartSet)
            {
                StartCoroutine(RoutineBuildStage(3, 3));

                _currentPhase = Phase.IDLE;
            }
            else
            {
                if(GameCEO.Language == 0)
                    onBuildWaringRequested?.Invoke("Você tem que definir a posição inicial " + (_playerStartSet ? "dos fanstasmas." : "do jogador."));
                else
                    onBuildWaringRequested?.Invoke("You've to set the " + (_playerStartSet ? "ghosts" : "player") + " start position.");
            }
        }
    }

    public void RequestBuildCustomStage()
    {
        StartCoroutine(RoutineBuildStage(0, 2));
    }

    public void RequestBuildDefaultStage()
    {
        LoadDefaultStage();
        StartCoroutine(RoutineBuildStage(0, 3));
    }

    public void Restart()
    {
        for (int __i = 0; __i < _coins.Count; __i++)
        {
            _coins[__i].gameObject.SetActive(true);
        }
    }

    private void BuildPhase()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _lastPressCenter = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(_lastPressCenter, CameraManager.MouseWorldPosition) >= 0.7f)
            {
                Transform __lookResult = LookForClickedObject();

                if (__lookResult == null)
                    return;

                if (__lookResult.tag == "Floor")
                {
                    Floor __pressedFloor = __lookResult.GetComponent<Floor>();

                    Block __block = Instantiate(blockPrefab, __lookResult.localPosition, Quaternion.identity);
                    __block.Initiate(__lookResult.localPosition);
                    __block.Active();

                    _blockTiles.Add(__block);
                    _lastPressCenter = __lookResult.localPosition;
                }
                else if (__lookResult.tag == "Block")
                {
                    _blockTiles.Remove(__lookResult.GetComponent<Block>());

                    Destroy(__lookResult.gameObject);

                    _lastPressCenter = __lookResult.localPosition;
                }
            }
        }
    }

    private Transform LookForClickedObject()
    {
        RaycastHit2D __hit = Physics2D.Raycast(CameraManager.MouseWorldPosition, Vector2.zero);

        if (__hit.transform != null)
        {
            return __hit.transform;
        }

        return null;
    }

    private void PlayerPhase()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform __lookResult = LookForClickedObject();

            if(__lookResult != null && __lookResult.tag == "Floor")
            {
                playerStartPos.position = __lookResult.position;

                _playerStartSet = true;
            }
        }
    }

    private void FoePhase()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform __lookResult = LookForClickedObject();

            if (__lookResult != null && __lookResult.tag == "Floor")
            {
                foeStartPos.position = __lookResult.position;

                _foeStartSet = true;
            }
        }
    }

    private void LoadDefaultStage()
    {
        playerStartPos.localPosition = defaultPlayerStart.localPosition;
        foeStartPos.localPosition = defaultFoeStart.localPosition;

        foreach (Transform __transfom in defaultBlockTilesHolder)
        {
            BuildObject __buildObject = __transfom.GetComponent<BuildObject>();
            __buildObject.Initiate(__buildObject.transform.localPosition);

            _blockTiles.Add(__buildObject);
        }
    }

    private IEnumerator RoutineBuildStage(int p_startPhase, int p_endPhase)
    {
        for (int __j = p_startPhase; __j < p_endPhase; __j++)
        {
            List<BuildObject> __buildObjectList = _buildObjects[__j];

            for (int __i = 0; __i < __buildObjectList.Count; __i++)
            {
                __buildObjectList[__i].transform.position = new Vector2(HelpExtensions.RandomPickOne(-4f, 23f), Random.Range(-3f, 35f));
                __buildObjectList[__i].gameObject.SetActive(true);
            }

            bool __allCompleted = false;

            AudioDatabase.Instance.buildSource.Play();

            while (!__allCompleted)
            {
                __allCompleted = true;

                for (int __i = 0; __i < __buildObjectList.Count; __i++)
                {
                    if (Vector2.Distance(__buildObjectList[__i].transform.localPosition, __buildObjectList[__i].myPosition) > 0.1f)
                    {
                        __buildObjectList[__i].transform.localPosition = Vector2.MoveTowards(__buildObjectList[__i].transform.localPosition, __buildObjectList[__i].myPosition, 40f * Time.deltaTime);
                        __allCompleted = false;
                    }
                    else
                    {
                        __buildObjectList[__i].transform.localPosition = __buildObjectList[__i].myPosition;
                    }
                }

                yield return null;
            }

            if (p_endPhase == 2) _currentPhase = Phase.BUILD;
        }

        if(p_endPhase >= 3)
        {
            playerStartPos.gameObject.SetActive(false);
            foeStartPos.gameObject.SetActive(false);

            AudioDatabase.Instance.buildSource.Play();

            int __totalCoins = 0;

            for (int __i = 0; __i < _floorTiles.Count;)
            {
                for (int __j = 0; __j < 5 && __i < _floorTiles.Count; __j++, __i++)
                {
                    Vector2 __floorPosition = _floorTiles[__i].transform.localPosition;
                    RaycastHit2D[] __hited = Physics2D.RaycastAll(__floorPosition, Vector2.zero);

                    if (__hited.Length == 1)
                    {
                        _coins.Add(Instantiate(coinPrefab, __floorPosition, Quaternion.identity));

                        __totalCoins++;
                    }
                }

                yield return null;
            }
            
            onStageBuildCompleted?.Invoke(__totalCoins);
        }
    }
}
