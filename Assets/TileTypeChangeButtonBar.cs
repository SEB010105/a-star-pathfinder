using UnityEngine;

public class TileTypeChangeButtonBar : MonoBehaviour
{
    private int _index;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _index++;
            transform.GetChild(_index % transform.childCount).GetComponent<TileTypeChangeButton>().OnPress();
        }
    }
}
