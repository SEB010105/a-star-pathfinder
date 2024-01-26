using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDebugTextButton : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private List<Sprite> checkBoxSprites;
    private Image _buttonImage;
    private bool _checked = true;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        StartCoroutine(UncheckButtonAtStart());
    }

    public void OnPress()
    {
        _checked = !_checked;
        grid.SetDebugTextsActive(_checked);
        _buttonImage.sprite = _checked ? checkBoxSprites[0] : checkBoxSprites[1];
    }

    private IEnumerator UncheckButtonAtStart()
    {
        yield return new WaitUntil(() => grid.GetTiles().Count > 0);
        OnPress();
    }
}
