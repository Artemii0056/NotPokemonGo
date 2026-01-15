using UnityEngine;

namespace Services.Audio
{
    public interface IAudioService
    {
        void PlayOneShot(AudioClip clip, float volume = 1f);
    }
}