using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public enum ButtonType
    {
        BackgraundMusic,
        SoundFx
    };

    public ButtonType type;

    public Sprite onSprite;
    public Sprite offSprite;


    public GameObject button;
    public Vector3 offButtonPosition;
    private Vector3 _onButtonPosition;
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = onSprite;
        _onButtonPosition = button.GetComponent<RectTransform>().anchoredPosition;
        ToggleButoon();

    }
    public void ToggleButoon()
    {
        var muted = false;

        if (type == ButtonType.BackgraundMusic)
            muted = SoundManager.instance.IsBackgraundMusicMuted();
        else
            muted = SoundManager.instance.isSoundFXMuted();

        if (muted)
        {
            _image.sprite = offSprite;
            button.GetComponent<RectTransform>().anchoredPosition = offButtonPosition;
        }
        else
        {
            _image.sprite = onSprite;
            button.GetComponent<RectTransform>().anchoredPosition = _onButtonPosition;
        }
    }
}
