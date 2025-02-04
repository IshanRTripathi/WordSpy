using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    private BoardData GameDataInstance => target as BoardData;
    private ReorderableList dataList;
    private void OnEnable()
    {
        InitialiseReorderableList(ref dataList, "SearchWords", "Searching Words");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawColumnsRowsInputFields();
        EditorGUILayout.Space();
        ConvertToUpperCase();
        

        if (GameDataInstance.board != null && GameDataInstance.Columns >0 && GameDataInstance.Rows > 0)
        {
            DrawBoardTable();
        }

        GUILayout.BeginHorizontal();
        ClearBoard();
        FillWithRandomLetters();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if(GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    private void DrawColumnsRowsInputFields()
    {
        var colsTemp = GameDataInstance.Columns;
        var rowsTemp = GameDataInstance.Rows;

        GameDataInstance.Columns = EditorGUILayout.IntField("Columns", GameDataInstance.Columns);
        GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

        if((GameDataInstance.Columns != colsTemp || GameDataInstance.Rows != rowsTemp) && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
        {
            GameDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        var rowStyle = new GUIStyle();
        rowStyle.fixedWidth = 40;
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;

        var textFieldStyle = new GUIStyle();
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (var x = 0; x < GameDataInstance.Columns; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for (var y = 0; y < GameDataInstance.Rows; y++)
            {
                if (x >= 0 && y >= 0 && x<20 && y<20)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.board[x].Row[y], textFieldStyle);
                    if (GameDataInstance.board[x].Row[y].Length > 1)
                    {
                        character = GameDataInstance.board[x].Row[y].Substring(0, 1);
                    }
                    GameDataInstance.board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void InitialiseReorderableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };
        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Word"), GUIContent.none);
        };
    }

    private void ConvertToUpperCase()
    {
        if(GUILayout.Button("ToUpperCase"))
        {
            for(var i = 0; i<GameDataInstance.Columns; i++)
            {
                for(var j=0; j<GameDataInstance.Rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.board[i].Row[j], @"[a-z]").Count;

                    if(errorCounter > 0)
                    {
                        GameDataInstance.board[i].Row[j] = GameDataInstance.board[i].Row[j].ToUpper();
                    }
                }
            }

            foreach(var searchWord in GameDataInstance.SearchWords)
            {
                var errorCounter = Regex.Matches(searchWord.Word, @"[a-z]").Count;
                if(errorCounter > 0)
                {
                    searchWord.Word = searchWord.Word.ToUpper();
                }
            }
        }
    }
    private void ClearBoard()
    {
        if(GUILayout.Button("ClearBoard"))
        {
            for (var i = 0; i < GameDataInstance.Columns; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.board[i].Row[j] = "";
                }
            }
        }
    }

    private void FillWithRandomLetters()
    {
        if (GUILayout.Button("RandomFill"))
        {
            for (var i = 0; i < GameDataInstance.Columns; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    //if(GameDataInstance.board[i].Row[j] != "")
                    // {
                    // random lowercase letter
                    //   int a = UnityEngine.Random.Range(0, 26);
                    //  char ch = (char)('A' + a);
                    //GameDataInstance.board[i].Row[j] = ch.ToString();
                    //}

                    var errorCounter = Regex.Matches(GameDataInstance.board[i].Row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    if (errorCounter == 0)
                    {
                        GameDataInstance.board[i].Row[j] = letters[UnityEngine.Random.Range(0, letters.Length)].ToString();
                    }
                }
            }
        }
    }
}
