using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public static class GameEvents
{
    public delegate void EnableSquareSelection();
    public static EnableSquareSelection onEnableSquareSelection;
    public static void EnableSquareSelectionMethod()
    {
        if(onEnableSquareSelection != null)
        {
            onEnableSquareSelection();
        }
    }

    public delegate void DisableSquareSelection();
    public static DisableSquareSelection onDisableSquareSelection;
    public static void DisableSquareSelectionMethod()
    {
        if(onDisableSquareSelection != null)
        {
            onDisableSquareSelection();
        }
    }

    public delegate void SelectSquare(Vector3 position);
    public static SelectSquare onSelectSquare;

    public static void SelectSquareMethod(Vector3 position)
    {
        if(onSelectSquare != null)
        {
            onSelectSquare(position);
        }
    }

    public delegate void CheckSquare(string letter, Vector3 squarePos, int squareIndex);
    public static CheckSquare onCheckSquare;

    public static void CheckSquareMethod(string letter, Vector3 squarePos, int squareIndex)
    {
        if(onCheckSquare != null)
        {
            onCheckSquare( letter,  squarePos, squareIndex);
        }
    }

    public delegate void ClearSelection();
    public static ClearSelection onClearSelection;

    public static void ClearSelectionMethod()
    {
        if(onClearSelection != null)
        {
            onClearSelection();
        }
    }
}
