using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class SortGameObjectsByName
{
    [MenuItem("GameObject/Sort By Name (Natural Order)", false, 0)]
    private static void SortSelectedObjectsByName()
    {
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("請先選取一些物件！");
            return;
        }

        // 確保所有選取物件有相同的父物件
        Transform parent = selectedObjects[0].transform.parent;
        if (selectedObjects.Any(obj => obj.transform.parent != parent))
        {
            Debug.LogWarning("選取的物件必須有相同的父物件才能排序！");
            return;
        }

        // 自然排序 (解析名稱中的數字部分和非數字部分)
        var sortedObjects = selectedObjects
            .OrderBy(obj => ParseName(obj.name), new NaturalOrderComparer())
            .ToArray();

        for (int i = 0; i < sortedObjects.Length; i++)
        {
            sortedObjects[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("已按自然順序排序！");
    }

    // 將名稱拆分為數字和非數字部分進行比較
    private static List<object> ParseName(string name)
    {
        var matches = Regex.Matches(name, @"\D+|\d+");
        return matches
            .Cast<Match>()
            .Select(m => int.TryParse(m.Value, out var num) ? (object)num : m.Value)
            .ToList();
    }

    // 自然順序比較器
    private class NaturalOrderComparer : IComparer<List<object>>
    {
        public int Compare(List<object> x, List<object> y)
        {
            int minLength = Mathf.Min(x.Count, y.Count);

            for (int i = 0; i < minLength; i++)
            {
                var xElement = x[i];
                var yElement = y[i];

                int result = 0;

                if (xElement is int xInt && yElement is int yInt)
                {
                    result = xInt.CompareTo(yInt);
                }
                else if (xElement is string xStr && yElement is string yStr)
                {
                    result = string.Compare(xStr, yStr, StringComparison.Ordinal);
                }
                else
                {
                    // 如果類型不同，數字優先於字串
                    result = xElement is int ? -1 : 1;
                }

                if (result != 0)
                {
                    return result;
                }
            }

            // 如果前部分完全相同，長度較短的優先
            return x.Count.CompareTo(y.Count);
        }
    }

    [MenuItem("GameObject/Sort By Name (Natural Order)", true)]
    private static bool ValidateSortSelectedObjectsByName()
    {
        return Selection.gameObjects.Length > 0;
    }
}
