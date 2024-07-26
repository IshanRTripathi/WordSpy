using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class GridSquare : MonoBehaviour
{
    public int squareIndex { get; set; }
    private AlphabetData.LetterData _normalLetterData;
    private AlphabetData.LetterData _selectedLetterData;
    private AlphabetData.LetterData _correctLetterData;

    private SpriteRenderer displayedImage;

    private bool _selected;
    private bool _clicked;
    private int _index = -1;

    private void SetIndex(int index)
    {
        _index = index;
    }

    private int GetIndex()
    {
        return _index;
    }

    void Start()
    {
        displayedImage = GetComponent<SpriteRenderer>();
        _selected = false;
        _clicked = false;

    }

    private void OnEnable()
    {
        GameEvents.onEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.onDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.onSelectSquare += SelectSquare;
    }

    private void OnDisable()
    {

        GameEvents.onEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.onDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.onSelectSquare -= SelectSquare;
    }

    public void OnEnableSquareSelection()
    {
        _clicked = true;
        _selected = false;
    }

    public void OnDisableSquareSelection()
    {
        _clicked = false;
        _selected = false;
    }

    public void SelectSquare(Vector3 pos)
    {
        if(this.gameObject.transform.position == pos)
        {
            displayedImage.sprite = _selectedLetterData.image;
        }
    }

    public void SetSprite(AlphabetData.LetterData normalLetterData, AlphabetData.LetterData selectedLetterData, AlphabetData.LetterData correctLetterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = _normalLetterData.image;
    }

    private void OnMouseDown()
    {
        onEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();
        CheckSquare();
        displayedImage.sprite = _selectedLetterData.image;
    }

    private void OnMouseEnter()
    {
        CheckSquare();
    }

    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    public void CheckSquare()
    {
        if(_selected == false && _clicked ==true)
        {
            _selected = true;
            GameEvents.CheckSquareMethod(_normalLetterData.letter, gameObject.transform.position, _index);
        }
    }
}
