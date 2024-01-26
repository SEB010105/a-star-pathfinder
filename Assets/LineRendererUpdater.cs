using UnityEngine;

public class LineRendererUpdater : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Actor actor;
    [SerializeField] private float updateFrequency = 0.5f;
    private int _pathLength;
    private float _countdown;

    private void Update()
    {
        if (_countdown <= 0)
        {
            _countdown = updateFrequency;
            AddLineRendererPosition();
        }

        _countdown -= Time.deltaTime;
    }

    public void Reset()
    {
        lineRenderer.positionCount = 0;
        _pathLength = 0;
    }
    
    public void AddLineRendererPosition()
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(_pathLength, actor.transform.position);
        _pathLength++;
    }
    
    public void SetPathActive(bool active)
    {
        lineRenderer.gameObject.SetActive(active);
    }
}