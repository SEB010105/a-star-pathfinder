using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPathButton : MonoBehaviour
{
    [SerializeField] private LineRendererUpdater lineRendererUpdater;
    [SerializeField] private List<Sprite> checkBoxSprites;
    private Image _buttonImage;
    private bool _checked = true;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
    }

    public void OnPress()
    {
        _checked = !_checked;
        lineRendererUpdater.SetPathActive(_checked);
        _buttonImage.sprite = _checked ? checkBoxSprites[0] : checkBoxSprites[1];
    }
}
