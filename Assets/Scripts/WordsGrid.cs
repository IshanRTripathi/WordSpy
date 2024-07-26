using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSqaurePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition = 0.0f;

    private List<GameObject> squareList = new List<GameObject>();

    private void Start()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }

    private void SetSquarePosition()
    {
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPos = GetFirstSquarePosition();
        int colNum = 0;
        int rowNum = 0;

        foreach(var square in squareList)
        {
            if(rowNum + 1 > currentGameData.selectedBoardData.Rows)
            {
                colNum++;
                rowNum = 0;
            }

            var posx = startPos.x + offset.x * colNum;
            var posy = startPos.y + offset.y * rowNum;

            square.GetComponent<Transform>().position = new Vector2(posx, posy);
            rowNum++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPos = new Vector2(0f, transform.position.y);
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midPosWidth = (((currentGameData.selectedBoardData.Columns - 1) * squareSize.x) / 2) * 0.01f;
        var midPosHeight = (((currentGameData.selectedBoardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

        startPos.x = midPosWidth != 0 ? midPosWidth * -1 : midPosWidth;
        startPos.y = midPosHeight != 0 ? midPosHeight * -1 : midPosHeight;

        return startPos;
    }

    private void SpawnGridSquares()
    {
        if(currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach(var sqaures in currentGameData.selectedBoardData.board)
            {
                foreach(var sqaureLetter in sqaures.Row)
                {
                    var normalLetter = alphabetData.AlphabetNormal.Find(data => data.letter == sqaureLetter);
                    var selectedLetter = alphabetData.AlphabetHighlighted.Find(data => data.letter == sqaureLetter);
                    var correctLetter = alphabetData.AlphabetWrong.Find(data => data.letter == sqaureLetter);

                    if(normalLetter.image == null || selectedLetter.image == null)
                    {
                        Debug.Log("normalLetter.image == null || selectedLetter.image == null for " + sqaureLetter);

#if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                            UnityEditor.EditorApplication.isPlaying = false;
#endif
                    }
                    else
                    {
                        squareList.Add(Instantiate(gridSqaurePrefab));
                        squareList[squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetter, correctLetter, selectedLetter);
                        squareList[squareList.Count - 1].transform.SetParent(this.transform);
                        squareList[squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        squareList[squareList.Count - 1].transform.localScale = squareScale;
                    }
                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (shouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;
            if(finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool shouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSqaurePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidth = ((currentGameData.selectedBoardData.Columns * squareSize.x) / 2) * 0.01f;
        var midHeight = ((currentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidth != 0 ? midWidth * -1 : midWidth);
        startPosition.y = (midHeight != 0 ? midHeight * -1 : midHeight);

        return (startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition);

    }

    private float GetHalfScreenWidth()
    {
        return (1.7f * Camera.main.orthographicSize * 2) * Screen.width / Screen.height;
    }


}
