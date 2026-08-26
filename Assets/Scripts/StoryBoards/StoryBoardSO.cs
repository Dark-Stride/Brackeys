using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Storyboards
{
    [System.Serializable]
    public struct StoryboardSlide
    {
        public Sprite slideImage;
        [TextArea(2, 4)] public string captionText;
        public AudioClip voiceOrSfx;
    }

    [CreateAssetMenu(fileName = "NewStoryboard", menuName = "Story/Storyboard Cutscene")]
    public class StoryboardSO : ScriptableObject
    {
        public string cutsceneTitle;
        public List<StoryboardSlide> slides = new();
    }
}
