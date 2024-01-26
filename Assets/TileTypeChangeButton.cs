using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.UI;

public class TileTypeChangeButton : MonoBehaviour
{
    [SerializeField] private AddTileOnLeftClick addTileOnLeftClickScript;
    [SerializeField] private TileType tileType;
    [SerializeField] private bool defaultActive;
    [SerializeField] private List<TileTypeChangeButton> otherButtons;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        if (defaultActive)
            OnPress();
    }
    
    public void OnPress()
    {
        _button.interactable = false;
        
        addTileOnLeftClickScript.SetLMBTileType(tileType);

        foreach (var otherButton in otherButtons)
        {
            otherButton.UnPress();
        }
    }

    public void UnPress()
    {
        _button.interactable = true;
    }
}
