using UnityEngine;
using System;
using System.Collections.Generic;

public class QteGestureHandler : MonoBehaviour
{
    public RectTransform swipeZone;
    public LineRenderer swipeLine;
    public float pointRecordSpacing = 10f;
    public float recognitionThreshold = 0.8f;
    public float matchRadius = 0.1f;
    public Color lineColor = Color.cyan;
    public bool useEditorDrawnTemplate = false;

    public List<Vector2> editorTemplatePoints = new();
    public GestureTemplateSO externalTemplate;

    public Action<bool> OnQteComplete;

    private List<Vector2> inputPoints = new();
    private List<Vector2> template = new();
    private Vector2 lastRecordedPoint;
    private bool tracking = false;
    private Camera _uiCamera;

    void Start()
    {
        _uiCamera = Camera.main;
        template = useEditorDrawnTemplate ? editorTemplatePoints :
            externalTemplate ? externalTemplate.points : GetDefaultTemplate();

        if (swipeLine)
        {
            swipeLine.enabled = false;
            swipeLine.positionCount = 0;
            swipeLine.startColor = lineColor;
            swipeLine.endColor = lineColor;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsInsideZone(Input.mousePosition))
        {
            inputPoints.Clear();
            tracking = true;
            lastRecordedPoint = Input.mousePosition;
            inputPoints.Add(NormalizeToZone(lastRecordedPoint));

            if (swipeLine)
            {
                swipeLine.enabled = true;
                swipeLine.positionCount = 1;
                swipeLine.SetPosition(0, ScreenToWorld(lastRecordedPoint));
            }
        }

        if (tracking && Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            if (Vector2.Distance(pos, lastRecordedPoint) > pointRecordSpacing)
            {
                lastRecordedPoint = pos;
                Vector2 norm = NormalizeToZone(pos);
                inputPoints.Add(norm);

                if (swipeLine)
                {
                    swipeLine.positionCount++;
                    swipeLine.SetPosition(swipeLine.positionCount - 1, ScreenToWorld(pos));
                }
            }
        }

        if (tracking && Input.GetMouseButtonUp(0))
        {
            tracking = false;
            if (swipeLine) swipeLine.enabled = false;

            bool match = MatchGesture(template, inputPoints, matchRadius);
            OnQteComplete?.Invoke(match);
        }
    }

    List<Vector2> GetDefaultTemplate()
    {
        return new List<Vector2>
        {
            new Vector2(0.1f, 0.1f),
            new Vector2(0.2f, 0.3f),
            new Vector2(0.35f, 0.5f),
            new Vector2(0.55f, 0.7f),
            new Vector2(0.75f, 0.8f),
            new Vector2(0.9f, 0.9f)
        };
    }

    bool IsInsideZone(Vector2 screenPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(swipeZone, screenPos);
    }

    Vector2 NormalizeToZone(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(swipeZone, screenPos, _uiCamera, out var local);
        var size = swipeZone.rect.size;
        return new Vector2((local.x + size.x * 0.5f) / size.x, (local.y + size.y * 0.5f) / size.y);
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        if (_uiCamera)
            return _uiCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1));
        return screenPos;
    }

    bool MatchGesture(List<Vector2> template, List<Vector2> input, float radius)
    {
        if (template.Count == 0 || input.Count == 0) return false;

        int matched = 0;
        int i = 0;

        foreach (var t in template)
        {
            for (; i < input.Count; i++)
            {
                if (Vector2.Distance(t, input[i]) < radius)
                {
                    matched++;
                    break;
                }
            }
        }

        return matched >= template.Count * recognitionThreshold;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        var source = useEditorDrawnTemplate ? editorTemplatePoints :
            externalTemplate ? externalTemplate.points : template;

        if (swipeZone == null || source.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < source.Count - 1; i++)
        {
            Gizmos.DrawLine(Denormalize(source[i]), Denormalize(source[i + 1]));
        }
    }

    Vector3 Denormalize(Vector2 norm)
    {
        var size = swipeZone.rect.size;
        Vector2 local = new Vector2(norm.x * size.x, norm.y * size.y) - size * 0.5f;
        return swipeZone.transform.TransformPoint(local);
    }
#endif
}