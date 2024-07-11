using UnityEngine;
using UnityEngine.Serialization;

using EmpireCafe.UI.Hud;
using EmpireCafe.UI.Information;
using EmpireCafe.UI.MainMenu;

namespace EmpireCafe.UI.StaticData
{
    [CreateAssetMenu(fileName = "UIData", menuName = "Static data/UI")]
    public class UIStaticData : ScriptableObject
    {
        [Header("Core")]
        public float curtainDissolutionStep;
        public float curtainDissolutionDelay;
        
        [FormerlySerializedAs("mainMenuViewPrefab")] [Header("Main menu")]
        public MainMenuHandler mainMenuHandlerPrefab;
        public InformationView informationViewPrefab;
        
        [Header("Gameplay")]
        public Canvas gameplayCanvas;
        public HudView hudViewPrefab;
    }
}