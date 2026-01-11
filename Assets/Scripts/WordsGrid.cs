using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

    void Start()
    {
        SpawnGridSquares();
        SetSquarePositions();
        if (_squareList.Count > 0)
        {
          
        }
        else
        {
            Debug.LogError("Grid kareleri oluþturulamadý. _squareList boþ!");
        }
    }

    private void SetSquarePositions()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].transform;

        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;

        foreach (var square in _squareList)
        {
            if (rowNumber + 1 > currentGameData.SelectedBoardData.Rows)
            {
                columnNumber++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + (offset.x * columnNumber);
            var positionY = startPosition.y - (offset.y * rowNumber);

            square.transform.position = new Vector2(positionX, positionY);
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].transform;

        var squareSize = new Vector2
        {
            x = squareRect.width * squareTransform.localScale.x,
            y = squareRect.height * squareTransform.localScale.y
        };

        var midWidthPosition = ((currentGameData.SelectedBoardData.Columns - 1) * squareSize.x / 2) * 0.01f;
        var midHeightPosition = ((currentGameData.SelectedBoardData.Rows - 1) * squareSize.y / 2) * 0.01f;

        startPosition.x = -midWidthPosition;
        startPosition.y += midHeightPosition; 

        return startPosition;
    }

    private void SpawnGridSquares()
    {
        if (currentGameData == null || currentGameData.SelectedBoardData == null)
        {
            Debug.LogError("GameData veya SelectedBoardData eksik.");
            return;//geriye deðer döndürme
            
        }

        var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));

        foreach (var squares in currentGameData.SelectedBoardData.Board)
        {
            foreach (var squareLetter in squares.Row)
            {
                var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                var selectedLetterData = alphabetData.AlphabetHighligted.Find(data => data.letter == squareLetter); 
                var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);

                if (normalLetterData == null || selectedLetterData?.image == null)
                { 
                    Debug.LogError($"Eksik harf verisi: '{squareLetter}' harfi bulunamadý.");
#if UNITY_EDITOR
                    if (UnityEditor.EditorApplication.isPlaying)
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
#endif
                    return;
                }

                var square = Instantiate(gridSquarePrefab); 
                square.GetComponent<GridSqare>().SetSprite(normalLetterData, correctLetterData, selectedLetterData);
                square.transform.SetParent(this.transform);
                square.transform.position = Vector3.zero;
                square.transform.localScale = squareScale;
                _squareList.Add(square);
                _squareList[_squareList .Count - 1]. GetComponent <GridSqare >().SetIndex(_squareList .Count -1); 
            }

        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        float adjustment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
           finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;

        Vector2 squareSize = new Vector2(
            (squareRect.width * targetScale.x) + squareOffset,
            (squareRect.height * targetScale.y) + squareOffset
        );

        float gridWidth = currentGameData.SelectedBoardData.Columns * squareSize.x;
        float gridHeight = currentGameData.SelectedBoardData.Rows * squareSize.y;

        float halfScreenWidth = GetHalfScreenWith() * 100f;

        return (gridWidth > halfScreenWidth * 2 || gridHeight > topPosition * 100f);
    }

    private float GetHalfScreenWith()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
