using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEvents;

public class WordChecker : MonoBehaviour
{

    public GameData CurrentGameData;
    public GameLevelData gameLevelData;
    private string _word;

    private int _assignedPoints = 0;
    private int _completedWords = 0;
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();



    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }
    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
        GameEvents.OnLoadNextLevel -= LoadNextGameLevel;
    }

    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
    void Start()
    {
        CurrentGameData.SelectedBoardData.ClearData();
          
        _assignedPoints = 0;
        _completedWords = 0; 

    }

    void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);
            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);
        }
    }

    private void SquareSelected(string letter, Vector3 squareposition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = squareposition;
            _correctSquareList.Add(squareIndex);
            _word += letter;

            _rayUp = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(0f, 1));
            _rayDown = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(0f, -1));
            _rayLeft = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(-1, 0f));
            _rayRight = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(1, 0f));
            _rayDiagonalLeftUp = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(-1, 1));
            _rayDiagonalLeftDown = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(-1, -1));
            _rayDiagonalRightUp = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(1, 1));
            _rayDiagonalRightDown = new Ray(new Vector2(squareposition.x, squareposition.y), new Vector2(1, -1));

        }
        else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = SelectRay(_rayStartPosition, squareposition);
            GameEvents.SelectSquareMethod(squareposition);
            _word += letter;
            CheckWord();
        }
        else
        {
            if (IsPointOnTheRay(_currentRay, squareposition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squareposition);
                _word += letter;
                CheckWord();
            }
        }
        _assignedPoints++;
    }
    private void CheckWord()
    {
        foreach (var searchinWord in CurrentGameData.SelectedBoardData.SearchWords)
        {
            if (_word == searchinWord.Word && searchinWord .Found ==false  )
            {
                searchinWord .Found = true;
                GameEvents.CorrectWordMethod(_word, _correctSquareList);
                _completedWords ++;
                _word = string.Empty;
                _correctSquareList.Clear();
                CheckBoardCompleted();
                return;
            }
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }
        return false;
    }
    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolorance = 0.01f;

        if (Math.Abs(direction.x) < tolorance && Math.Abs(direction.y - 1f) < tolorance)
        {
            return _rayUp;
        }

        if (Math.Abs(direction.x) < tolorance && Math.Abs(direction.y - (-1f)) < tolorance)
        {
            return _rayDown;
        }

        if (Math.Abs(direction.x - (-1f)) < tolorance && Math.Abs(direction.y) < tolorance)
        {
            return _rayLeft;
        }
        if (Math.Abs(direction.x - 1f) < tolorance && Math.Abs(direction.y) < tolorance)
        {
            return _rayRight;
        }

        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }

        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }

        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }
        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }
        return _rayDown;
    }
    private void ClearSelection()
    {
        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }

    private void CheckBoardCompleted()
    {
        bool loadNextCategory = false;
        if (CurrentGameData.SelectedBoardData.SearchWords.Count == _completedWords)
        {
            var categoryName = CurrentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;
            for (int index = 0; index < gameLevelData.data.Count; index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(gameLevelData.data[index].catagoryName);
                    readNextLevelName = false;
                }
                if (gameLevelData.data[index].catagoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;

                }
            }
            var currentLevelSize = gameLevelData.data[currentCategoryIndex].boardData.Count;
            if (currentBoardIndex < currentLevelSize)
                currentBoardIndex += 1;

            DataSaver.SaveCategoryData(categoryName, currentBoardIndex);

            if (currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;
                if (currentCategoryIndex < gameLevelData.data.Count)
                {
                    categoryName = gameLevelData.data[currentCategoryIndex].catagoryName;
                    currentBoardIndex = 0;
                    loadNextCategory = true;

                    if (nextBoardIndex <= 0)
                    {
                        DataSaver.SaveCategoryData(categoryName, currentBoardIndex); 
                    } 
                }
                else
                {
                    SceneManager.LoadScene("SelectCatagory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod(); 
            }
            if(loadNextCategory )
            {
                GameEvents.UnlockNextCategoryMethod();
            }
        }
    }
}
