public sealed class CameraBlendGuard
{
    private bool _isBlending;

    public bool TryStart()
    {
        if (_isBlending)
            return false;

        _isBlending = true;
        return true;
    }

    public void Complete()
    {
        _isBlending = false;
    }
}