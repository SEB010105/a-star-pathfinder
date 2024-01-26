using UnityEngine;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private Actor actor;
    [SerializeField] private LineRendererUpdater lineRendererUpdater;
    [SerializeField] private Grid grid;
    [SerializeField] private PlayButton playButton;
    [SerializeField] private PauseButton pauseButton;

    public void OnPress()
    {
        playButton.UnPress();
        pauseButton.UnPress();
        actor.Reset();
        lineRendererUpdater.Reset();
        grid.ClearDebugTexts();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OnPress();
    }
}
