namespace Units.AnimationControllers
{
    public sealed class AbilityRuntimeState
    {
        public bool QteSuccess;
        public bool QtePerfect;

        public void Clear()
        {
            QteSuccess = false;
            QtePerfect = false;
        }
    }
}