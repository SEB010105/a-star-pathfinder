using System.Collections;
using System.Collections.Generic;
using Tiles;
using TMPro;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private float moveDelay;
    [SerializeField] private ParticleSystem targetReachedParticleSystem;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject statusText;
    [SerializeField] private TileType defaultSpeedTile;
    [SerializeField] private Grid grid;
    private Movement.Movement _movement;
    private Vector3 _startPosition;
    private Coroutine _ongoingMovement;
    
    public bool MovementPaused { get; set; }
    public bool MovementOnGoing { get; set; }

    private void Start()
    {
        _startPosition = transform.position;
        _movement = new Movement.Smooth(gameObject, 10);
    }

    private void Move(Vector3 to)
    {
        _ongoingMovement = StartCoroutine(_movement.Move(to));
    }

    public IEnumerator MoveAlongPath(List<Tiles.Tile> path)
    {
        MovementOnGoing = true;
        
        foreach (var tile in path)
        {
            yield return new WaitUntil(ActorNotPaused);
            
            Move(tile.GetPositionInWorld());
            
            yield return new WaitUntil(() => !_movement.Moving);
        }

        if (CheckIfTargetReached())
            targetReachedParticleSystem.Play();
        else
        {
            statusText.GetComponent<TextMeshProUGUI>().text =
                "TARGET IS UNREACHABLE!";
            statusText.GetComponent<Animator>().Play("StatusTextFadeOut");
        }
        
        MovementOnGoing = false;
    }

    private bool CheckIfTargetReached()
    {
        var actorPosition = transform.position;
        var targetPosition = target.transform.position;

        return
            Mathf.Abs(actorPosition.x - targetPosition.x) < 0.00001 &&
            Mathf.Abs(actorPosition.z - targetPosition.z) < 0.00001
            ;
    }

    private bool ActorNotPaused()
    {
        return !MovementPaused;
    }

    public void Reset()
    {
        if (_ongoingMovement != null)
            StopCoroutine(_ongoingMovement);
        gameObject.transform.position = _startPosition;
        MovementPaused = false;
        MovementOnGoing = false;
    }

    public float GetMoveDelay()
    {
        return moveDelay;
    }

    public float GetCurrentSpeed()
    {
        var tileUnderActor = grid.GetTile(grid.GetPositionInGrid(transform.position));
        return (float) defaultSpeedTile.GetPassCost() / tileUnderActor.PassCost;
    }
}