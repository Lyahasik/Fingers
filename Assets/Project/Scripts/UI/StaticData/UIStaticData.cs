using Fingers.UI.Gameplay;
using Fingers.UI.Hud;
using Fingers.UI.Information;
using Fingers.UI.MainMenu;
using UnityEngine;

namespace Fingers.UI.StaticData
{
    [CreateAssetMenu(fileName = "UIData", menuName = "Static data/UI")]
    public class UIStaticData : ScriptableObject
    {
        [Header("Core")]
        public float curtainDissolutionStep;
        public float curtainDissolutionDelay;
        
        [Header("Main menu")]
        public MainMenuHandler mainMenuHandlerPrefab;
        public InformationView informationViewPrefab;
        
        [Header("Gameplay")]
        public GameplayHandler gameplayHandler;
        public HudView hudViewPrefab;
    }
}