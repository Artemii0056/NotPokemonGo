using Services.Audio;
using UnityEngine;

namespace DefaultNamespace
{
    public sealed class AudioService : IAudioService
    {
        private AudioSource _2dSource;

        public AudioService()
        {
            // Создаём один глобальный 2D источник
            var go = new GameObject("[AudioService_2D]");
            Object.DontDestroyOnLoad(go);

            _2dSource = go.AddComponent<AudioSource>();
            _2dSource.playOnAwake = false;
            _2dSource.spatialBlend = 0f; // 2D
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            Play2D(clip, volume);
        }

        public void Play2D(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            _2dSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void Play3D(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null)
                return;

            AudioSource.PlayClipAtPoint(
                clip,
                position,
                Mathf.Clamp01(volume)
            );
        }
    }
}