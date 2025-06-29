using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject searchinWordPrefab;
    public float offset = 0.0f;
    public int maxColumns = 5;
    public int maxRows = 4;

    private int _columns = 2;
    private int _rows;
    private int _wordsNumber;
    private List<GameObject> _words = new List<GameObject>();

    private void Start()
    {
        // Null kontrolü
        if (currentGameData == null || currentGameData.SelectedBoardData == null || currentGameData.SelectedBoardData.SearchWords == null)
        {
            Debug.LogError("GameData or SearchWords is not assigned!");
            return;
        }

        if (searchinWordPrefab == null)
        {
            Debug.LogError("Search word prefab is not assigned!");
            return;
        }

        _wordsNumber = currentGameData.SelectedBoardData.SearchWords.Count;

        if (_wordsNumber == 0)
        {
            Debug.LogWarning("No words to create!");
            return;
        }

        if (_wordsNumber < _columns)
            _rows = 1;
        else
            CalculateColumnsAndRows();

        CreateWordObjects();
        SetWordPositions();
    }

    private void CalculateColumnsAndRows()
    {
        do
        {
            _columns++;
            _rows = _wordsNumber / _columns;
        }
        while (_rows >= maxRows);

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        _columns++;
        _rows = _wordsNumber / _columns;

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;
            return false;
        }

        if (_wordsNumber % _columns > 0)
            _rows++;

        return true;
    }

    private void CreateWordObjects()
    {
        var squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (var index = 0; index < _wordsNumber; index++)
        {
            _words.Add(Instantiate(searchinWordPrefab) as GameObject);
            _words[index].transform.SetParent(this.transform);
            _words[index].GetComponent<RectTransform>().localScale = squareScale;
            _words[index].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            _words[index].GetComponent<SearchingWord>().SetWord(currentGameData.SelectedBoardData.SearchWords[index].Word);

        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        Vector3 scale = defaultScale;
        float adjustment = 0.01f;

        while (ShouldScaleDown(scale))
        {
            scale.x -= adjustment;
            scale.y -= adjustment;

            if (scale.x <= 0f || scale.y <= 0f)
            {
                scale.x = adjustment;
                scale.y = adjustment;
                break;
            }
        }

        return scale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        RectTransform prefabRect = searchinWordPrefab.GetComponent<RectTransform>();
        RectTransform parentRect = GetComponent<RectTransform>();

        float width = prefabRect.rect.width * targetScale.x + offset;
        float height = prefabRect.rect.height * targetScale.y + offset;

        float totalHeight = height * _rows;

        while (totalHeight > parentRect.rect.height)
        {
            if (TryIncreaseColumnNumber())
            {
                totalHeight = height * _rows;
            }
            else
            {
                return true;
            }
        }

        float totalWidth = width * _columns;
        return totalWidth > parentRect.rect.width;
    }

    private void SetWordPositions()
    {
        if (_words.Count == 0)
        {
            Debug.LogWarning("No word objects to position.");
            return;
        }

        RectTransform squareRect = _words[0].GetComponent<RectTransform>();
        Vector2 offsetPerWord = new Vector2(
            squareRect.rect.width * squareRect.localScale.x + offset,
            squareRect.rect.height * squareRect.localScale.y + offset
        );

        Vector2 startPos = GetFirstSquarePosition();

        int col = 0;
        int row = 0;

        foreach (var word in _words)
        {
            if (col >= _columns)
            {
                col = 0;
                row++;
            }

            float posX = startPos.x + offsetPerWord.x * col;
            float posY = startPos.y - offsetPerWord.y * row;

            word.GetComponent<RectTransform>().localPosition = new Vector2(posX, posY);
            col++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        RectTransform wordRect = _words[0].GetComponent<RectTransform>();
        RectTransform parentRect = GetComponent<RectTransform>();

        float wordWidth = wordRect.rect.width * wordRect.localScale.x + offset;
        float wordHeight = wordRect.rect.height * wordRect.localScale.y + offset;

        float totalWidth = wordWidth * _columns;
        float totalHeight = wordHeight * _rows;

        float startX = -(totalWidth / 2f) + (wordWidth / 2f);
        float startY = (parentRect.rect.height / 2f) - (wordHeight / 2f);

        return new Vector2(startX, startY);
    }
}
    









































