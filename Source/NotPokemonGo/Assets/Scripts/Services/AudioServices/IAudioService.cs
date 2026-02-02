using UnityEngine;

namespace Services.AudioServices
{
    public interface IAudioService
    {
        void PlayOneShot(AudioClip clip, float volume = 1f);
        void Play2D(AudioClip clip, float volume = 1f);
        void Play3D(AudioClip clip, Vector3 position, float volume = 1f);
    }
}