using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddTileOnLeftClick : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private Grid _gridComponent;
    [SerializeField] private Actor actor;
    [SerializeField] private GameObject statusText;
    [SerializeField] private Tiles.TileType lmbTileType;
    [SerializeField] private Tiles.TileType rmbTileType;
    [SerializeField] private float drawActivationDuration = 0.2f;
    [SerializeField] private List<GameObject> irreplaceables;
    private float _mouseDownDuration;

    private void Start()
    {
        _gridComponent = grid.GetComponent<Grid>();
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            _mouseDownDuration = 0;
        }
        else
        {
            _mouseDownDuration += Time.deltaTime;
            if (_mouseDownDuration < drawActivationDuration && _mouseDownDuration - Time.deltaTime > 0)
                return;

            PlaceTile(Input.GetMouseButton(0) ? lmbTileType : rmbTileType);
        }
    }

    public void SetLMBTileType(Tiles.TileType tileType)
    {
        lmbTileType = tileType;
    }

    private void PlaceTile(Tiles.TileType tileType)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit))
        {
            if (actor.MovementOnGoing)
            {
                statusText.GetComponent<TextMeshProUGUI>().text =
                    "YOU CANNOT EDIT THE GRID WHILE THE SIMULATION IS RUNNING!";
                statusText.GetComponent<Animator>().Play("StatusTextFadeOut");
                return;
            }
            
            var hitPoint = ray.GetPoint(hit.distance);
            
            if (hit.transform.IsChildOf(grid.transform))
                hitPoint = hit.transform.position;
            
            var positionInGrid = _gridComponent.GetPositionInGrid(hitPoint);
            
            if (IsExcludedPosition(positionInGrid))
                return;

            _gridComponent.SetTileTypeAtPosition(tileType, positionInGrid);
        }
    }

    private bool IsExcludedPosition(Vector2 position)
    {
        foreach (var irreplaceable in irreplaceables)
        {
            if (position.Equals(_gridComponent.GetPositionInGrid(irreplaceable.transform.position)))
                return true;
        }
        return false;
    }
}