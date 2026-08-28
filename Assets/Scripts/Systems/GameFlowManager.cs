using Scripts.Storyboards;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems
{
    public enum GameStep
    {
        SplashScreen,
        MainMenu,
        Cutscene1,
        Gameplay1_Tutorial,
        Cutscene2,
        Gameplay2_CaptureMission,
        Cutscene3,
        Gameplay3_GuildHQ_Dungeon,
        Cutscene4,
        Gameplay4_FinalBetrayalFight,
        Cutscene5,
        Credits,
    }

    public class GameFlowManager : MonoBehaviour
    {
        public static GameFlowManager Instance { get; private set; }

        [Header("Current State")]
        [SerializeField]
        private GameStep currentStep = GameStep.SplashScreen;

        [Header("Storyboard Assets")]
        [SerializeField]
        private StoryboardSO cutscene1;

        [SerializeField]
        private StoryboardSO cutscene2;

        [SerializeField]
        private StoryboardSO cutscene3;

        [SerializeField]
        private StoryboardSO cutscene4;

        [SerializeField]
        private StoryboardSO cutscene5;

        [Header("Scene Names (Match your Build Settings)")]
        [SerializeField]
        private string splashSceneName = "00_Splash";

        [SerializeField]
        private string menuSceneName = "01_MainMenu";

        [SerializeField]
        private string gameplay1SceneName = "02_Gameplay1_Tutorial";

        [SerializeField]
        private string gameplay2SceneName = "03_Gameplay2_Mission";

        [SerializeField]
        private string gameplay3SceneName = "04_Gameplay3_HQ_Dungeon";

        [SerializeField]
        private string gameplay4SceneName = "05_Gameplay4_Boss";

        [SerializeField]
        private string creditsSceneName = "06_Credits";

        public GameStep CurrentStep => currentStep;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AdvanceFlow()
        {
            currentStep++;
            ExecuteCurrentStep();
        }

        public void SetStep(GameStep step)
        {
            currentStep = step;
            ExecuteCurrentStep();
        }

        private void ExecuteCurrentStep()
        {
            Debug.Log($"[GameFlowManager] Advancing to: {currentStep}");

            switch (currentStep)
            {
                case GameStep.SplashScreen:
                    LoadScene(splashSceneName);
                    break;
                case GameStep.MainMenu:
                    LoadScene(menuSceneName);
                    break;
                case GameStep.Cutscene1:
                    PlayCutscene(cutscene1, () => AdvanceFlow());
                    break;
                case GameStep.Gameplay1_Tutorial:
                    LoadScene(gameplay1SceneName);
                    break;
                case GameStep.Cutscene2:
                    PlayCutscene(cutscene2, () => AdvanceFlow());
                    break;
                case GameStep.Gameplay2_CaptureMission:
                    LoadScene(gameplay2SceneName);
                    break;
                case GameStep.Cutscene3:
                    PlayCutscene(cutscene3, () => AdvanceFlow());
                    break;
                case GameStep.Gameplay3_GuildHQ_Dungeon:
                    LoadScene(gameplay3SceneName);
                    break;
                case GameStep.Cutscene4:
                    PlayCutscene(cutscene4, () => AdvanceFlow());
                    break;
                case GameStep.Gameplay4_FinalBetrayalFight:
                    LoadScene(gameplay4SceneName);
                    break;
                case GameStep.Cutscene5:
                    PlayCutscene(cutscene5, () => AdvanceFlow());
                    break;
                case GameStep.Credits:
                    LoadScene(creditsSceneName);
                    break;
            }
        }

        private void PlayCutscene(StoryboardSO storyboard, System.Action onComplete)
        {
            var storyboardUI = FindFirstObjectByType<StoryboardUI>(FindObjectsInactive.Include);
            if (storyboardUI != null)
            {
                storyboardUI.PlayStoryboard(storyboard, onComplete);
            }
            else
            {
                Debug.LogWarning(
                    "[GameFlowManager] No StoryboardUI found in scene! Skipping cutscene."
                );
                onComplete?.Invoke();
            }
        }

        private void LoadScene(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
