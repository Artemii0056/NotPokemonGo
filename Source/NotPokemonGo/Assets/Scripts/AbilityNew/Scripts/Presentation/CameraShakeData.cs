namespace AbilityNew.Scripts.Presentation
{
    public struct CameraShakeData
    {
        public float Amplitude;
        public float Frequency;
        public float Duration;

        public CameraShakeData(float amplitude, float frequency, float duration)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Duration = duration;
        }
    }
}