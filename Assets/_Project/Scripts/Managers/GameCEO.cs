using UnityEngine;

public class GameCEO : MonoBehaviour
{
    public static event System.Action<int> onLanguageSelected;
    public static event System.Action onGameStateChanged;

    public static int Language;
    public static GameState State { get; private set; }

    public GUIManager guiManager;
    public InputManager inputManager;
    public StageManager stageManager;
    public CameraManager cameraManager;
    public AgentsManager agentsManager;

    public MapGenerator mapGenerator;

    private void Awake()
    {
        ChangeGameState(GameState.LOADING);

        inputManager.onPauseRequested += InputManager_onPauseRequested;

        guiManager.onLanguageRequested += GuiManager_onLanguageRequested;
        guiManager.onDefaultStageRequested += GuiManager_onDefaultStageRequested;
        guiManager.onBuildStageRequested += GuiManager_onBuildStageRequested;
        guiManager.onRetryRequested += GuiManager_onRetryRequested;
        guiManager.onIntroRequested += GuiManager_onIntroRequested;

        mapGenerator.onStageBuildCompleted += MapGenerator_onStageBuildCompleted;
        mapGenerator.onBuildPhaseChanged += MapGenerator_onBuildPhaseChanged;
        mapGenerator.onBuildWaringRequested += MapGenerator_onBuildWaringRequested;

        stageManager.onPointsUpdated += StageManager_onPointsUpdated;
        stageManager.onRestartPlayerRequested += StageManager_onRestartPlayerRequested;
        stageManager.onNextLevelRequested += StageManager_onNextLevelRequested;
        stageManager.onGameOver += StageManager_onGameOver;

        cameraManager.Initiate();
        guiManager.Initiate();
        stageManager.Initiate();
        agentsManager.Initate();
    }

    private void Start()
    {
        guiManager.ShowDisplay(Displays.LANGUAGE);

        ChangeGameState(GameState.INTRO);
    }

    //-----------------CEO------------------

    private void StartGame()
    {
        agentsManager.ActiveCharacters();

        guiManager.ShowDisplay(Displays.HUD);

        ChangeGameState(GameState.PLAY);
    }

    private void Restart(bool p_resetPoints)
    {
        agentsManager.DeactiveCharacters();
        mapGenerator.Restart();
        stageManager.Restart(p_resetPoints);

        guiManager.UpdateDisplay(Displays.HUD, 0, stageManager.points);
        guiManager.UpdateDisplay(Displays.HUD, 1, stageManager.lives);
    }

    private void ChangeGameState(GameState p_state)
    {
        State = p_state;

        onGameStateChanged?.Invoke();
    }

    //-----------------GUI MANAGER------------------

    private void GuiManager_onLanguageRequested(int p_language)
    {
        Language = p_language;
        onLanguageSelected?.Invoke(p_language);

        guiManager.ShowDisplay(Displays.INTRO);
    }

    private void InputManager_onPauseRequested()
    {
        if(State == GameState.PLAY)
        {
            Time.timeScale = 0;
            ChangeGameState(GameState.PAUSE);

            guiManager.ShowDisplay(Displays.PAUSE);
        }
        else if(State == GameState.PAUSE)
        {
            Time.timeScale = 1;
            ChangeGameState(GameState.PLAY);

            guiManager.ShowDisplay(Displays.HUD);
        }
    }

    //-----------------GUI MANAGER------------------

    private void GuiManager_onDefaultStageRequested()
    {
        mapGenerator.RequestBuildDefaultStage();

        guiManager.HideCurrentDisplay();

        ChangeGameState(GameState.LOADING);
    }

    private void GuiManager_onBuildStageRequested()
    {
        mapGenerator.RequestBuildCustomStage();

        guiManager.ShowDisplay(Displays.BUILD);

        ChangeGameState(GameState.LOADING);
    }

    private void GuiManager_onRetryRequested()
    {
        Restart(true);
        StartGame();
    }

    private void GuiManager_onIntroRequested()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //-----------------MAP GENERATOR------------------

    private void MapGenerator_onStageBuildCompleted(int p_totalCoins)
    {
        AstarPath.active.Scan();
        stageManager.InitializeStage(mapGenerator.blockTiles, p_totalCoins);
        agentsManager.InitializeCharacters();

        StartGame();
    }

    private void MapGenerator_onBuildPhaseChanged(MapGenerator.Phase p_phase)
    {
        guiManager.UpdateDisplay(Displays.BUILD, (int)p_phase);
    }

    private void MapGenerator_onBuildWaringRequested(string p_warning)
    {
        guiManager.UpdateDisplay(Displays.BUILD, p_warning);
    }

    //-----------------STAGE MANAGER----------------

    private void StageManager_onPointsUpdated(int p_points)
    {
        guiManager.UpdateDisplay(Displays.HUD, 0, p_points);
    }

    private void StageManager_onRestartPlayerRequested(int p_lives)
    {
        agentsManager.RestartPlayerCharacter();

        guiManager.UpdateDisplay(Displays.HUD, 1, p_lives);
    }

    private void StageManager_onNextLevelRequested()
    {
        ChangeGameState(GameState.VICTORY);

        Restart(false);

        stageManager.ChangeLevel(stageManager.level + 1);
        guiManager.UpdateDisplay(Displays.HUD, 2, stageManager.level);
        StartGame();
    }

    private void StageManager_onGameOver()
    {
        ChangeGameState(GameState.GAME_OVER);

        agentsManager.DeactiveCharacters();
        

        guiManager.ShowDisplay(Displays.GAME_OVER);
    }
}
