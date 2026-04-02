using System;
using UnityEngine;

namespace AbilityNew.Scripts.Presentation
{
    [Serializable]
    public sealed class PlayAudioStep : PresentationStep
    {
        public PresentationAnchor Anchor;
        public AudioClip Clip;
        public float Volume = 1f;
    }
}