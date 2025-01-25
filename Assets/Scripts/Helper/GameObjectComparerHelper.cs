using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectComparerHelper
{
    /// <summary>
    /// 比較兩個List中相同及不同的元素
    /// </summary>
    /// <returns>傳回ComparisonResult這個型態的資料</returns>
    public static ComparisonResult CompareAndExecuteMethods(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count < 1 || list2.Count < 1) return null;

        var commonElements = ExecuteMethodForCommonElements(list1, list2);
        var differentElements = ExecuteMethodForDifferentElements(list1, list2);

        return new ComparisonResult(commonElements, differentElements);
    }

    /// <summary>
    /// 比較兩個List中相同的元素
    /// </summary>
    /// <returns>傳回所有相同元素的List</returns>
    private static List<GameObject> ExecuteMethodForCommonElements(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count < 1 || list2.Count < 1) return null;

        var commonElements = list1.Intersect(list2);
        List<GameObject> commonElementsList = new List<GameObject>();

        foreach (var commonElement in commonElements)
        {
            commonElementsList.Add(commonElement);
        }

        return commonElementsList;
    }

    /// <summary>
    /// 比較兩個List中不同的元素
    /// </summary>
    /// <returns>傳回所有不同元素的List</returns>
    private static List<GameObject> ExecuteMethodForDifferentElements(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count < 1 || list2.Count < 1) return null;

        var differentElements = list1.Except(list2).Union(list2.Except(list1));
        List<GameObject> differentElementsList = new List<GameObject>();

        foreach (var differentElement in differentElements)
        {
            differentElementsList.Add(differentElement);
        }

        return differentElementsList;
    }

    /// <summary>
    /// 自定義型態，用於儲存相同與不同元素的List
    /// </summary>
    public class ComparisonResult
    {
        /// <summary>
        /// 相同的元素List
        /// </summary>
        public List<GameObject> commonElementsList { get; private set; }

        /// <summary>
        /// 不同的元素List
        /// </summary>
        public List<GameObject> differentElementsList { get; private set; }

        public ComparisonResult(List<GameObject> commonElementsList, List<GameObject> differentElementsList)
        {
            this.commonElementsList = commonElementsList;
            this.differentElementsList = differentElementsList;
        }
    }
}
