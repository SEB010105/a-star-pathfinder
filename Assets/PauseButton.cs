using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Actor actor;
    [SerializeField] private PlayButton playButton;
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
    }
    
    public void OnPress()
    {
        if (!actor.MovementOnGoing)
            return;
            
        _button.interactable = false;
        actor.MovementPaused = true;

        playButton.UnPress();
    }

    public void UnPress()
    {
        _button.interactable = true;
        actor.MovementPaused = false;
    }
}
