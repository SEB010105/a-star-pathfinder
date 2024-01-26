using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Actor actor;
    [SerializeField] private GameObject target;
    [SerializeField] private Grid grid;
    [SerializeField] private PauseButton pauseButton;
    [SerializeField] private ResetButton resetButton;
    private Button _button;
    private Coroutine _onGoingCoroutine;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnPress()
    {
        if (ActorAtTarget())
            resetButton.OnPress();

        _button.interactable = false;
        pauseButton.UnPress();
        
        grid.ClearDebugTexts();
        
        if (!actor.MovementOnGoing)
            _onGoingCoroutine = StartCoroutine(actor.MoveAlongPath(
                grid.GetPath(
                    grid.GetPositionInGrid(actor.transform.position),
                    grid.GetPositionInGrid(target.transform.position)
                )));
    }

    private bool ActorAtTarget()
    {
        var actorPositionInGrid = grid.GetPositionInGrid(actor.transform.position);
        var targetPositionInGrid = grid.GetPositionInGrid(target.transform.position);
        return actorPositionInGrid.Equals(targetPositionInGrid);
    }

    public void UnPress()
    {
        _button.interactable = true;
        if (_onGoingCoroutine != null)
        {
            actor.MovementOnGoing = false;
            StopCoroutine(_onGoingCoroutine);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_button.interactable)
                OnPress();
            else 
                pauseButton.OnPress();
        }

        if (!actor.MovementOnGoing)
            _button.interactable = true;
    }
}
