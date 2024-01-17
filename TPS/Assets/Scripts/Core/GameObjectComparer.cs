using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectComparer
{
    /// <summary>
    /// ������List���ۦP�Τ��P������
    /// </summary>
    /// <returns>�Ǧ^ComparisonResult�o�ӫ��A�����</returns>
    public static ComparisonResult CompareAndExecuteMethods(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count < 1 || list2.Count < 1) return null;

        var commonElements = ExecuteMethodForCommonElements(list1, list2);
        var differentElements = ExecuteMethodForDifferentElements(list1, list2);

        return new ComparisonResult(commonElements, differentElements);
    }

    /// <summary>
    /// ������List���ۦP������
    /// </summary>
    /// <returns>�Ǧ^�Ҧ��ۦP������List</returns>
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
    /// ������List�����P������
    /// </summary>
    /// <returns>�Ǧ^�Ҧ����P������List</returns>
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
    /// �۩w�q���A�A�Ω��x�s�ۦP�P���P������List
    /// </summary>
    public class ComparisonResult
    {
        /// <summary>
        /// �ۦP������List
        /// </summary>
        public List<GameObject> commonElementsList { get; private set; }

        /// <summary>
        /// ���P������List
        /// </summary>
        public List<GameObject> differentElementsList { get; private set; }

        public ComparisonResult(List<GameObject> commonElementsList, List<GameObject> differentElementsList)
        {
            this.commonElementsList = commonElementsList;
            this.differentElementsList = differentElementsList;
        }
    }
}
