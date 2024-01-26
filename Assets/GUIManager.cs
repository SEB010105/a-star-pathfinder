using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private Canvas _canvas;
    private bool _visible = true;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            _visible = !_visible;
            _canvas.enabled = _visible;
        }
    }
}
