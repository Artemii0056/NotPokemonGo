using Units;

namespace Battlefields
{
    public class TargetHighlighter
    {
        private Unit _target;
        
        public Unit Target => _target;

        public void Highlight(Unit target)
        {
            if (_target == target)
                return;

            target.HighlightContainer.gameObject.SetActive(true);

            if (_target != null)
                _target.HighlightContainer.gameObject.SetActive(false);

            _target = target;
        }
    }
}