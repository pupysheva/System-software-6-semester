using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyTypes.Tree
{
    public static class TreeTools
    {
        private static int maxDeepCount = 1000;

        /// <summary>
        /// Возвращает количество всех потомков, включая текущий узел.
        /// Прошу заметить, что в случае, если потомков 0, то вернётся 1.
        /// </summary>
        /// <exception cref="StackOverflowException">Найдена рекурсия или достигнут лимит размера дерева.</exception>
        public static long GetCountAll<T>(this ITreeNode<T> tree)
        {
            if(--maxDeepCount <= 0)
            {
                maxDeepCount = 1000;
                throw new StackOverflowException();
            }
            long count = 1;
            foreach(ITreeNode<T> child in tree.GetEnumerableOnlyNeighbors())
            {
                count += child.GetCountAll();
            }
            maxDeepCount++;
            return count;
        }

        private readonly static ISet<object> nodesWrited = new MyHashSet<object>();

        public static string ChildrenToString<T>(this ITreeNode<T> root, StringFormat sf = StringFormat.Default, string separator = ", ")
        {
            StringBuilder sb = new StringBuilder();
            foreach(ITreeNode<T> n in root.GetEnumerableOnlyNeighbors())
            {
                sb.Append(n.ToString(sf));
                sb.Append(separator);
            }
            if (sb.Length >= separator.Length)
                sb.Length -= separator.Length;
            return sb.ToString();
        }

        public static string ToString<T>(this ITreeNode<T> root, StringFormat sf)
        {
            if (root == null)
                return null;
            if (!nodesWrited.Add(root))
                return $"cur: {root.Current}, deep: ...";
            string output;
            try
            {
                output = root.ToStringUnsafe(sf);
            }
            catch
            {
                nodesWrited.Remove(root);
                throw;
            }
            nodesWrited.Remove(root);
            return output;
        }

        private static string ToStringUnsafe<T>(this ITreeNode<T> root, StringFormat sf)
        {
            switch (sf)
            {
                case StringFormat.Default:
                    return $"{root.GetType()}: cur: {root.Current}, deep: [{root.ChildrenToString(sf, ", ")}]";
                case StringFormat.NewLine:
                    {
                        string[] lines = $"{root.Current}{(root.Count > 0 ? ":" : "")}\n<NEEDTAB_TreeNode>{root.ChildrenToString(sf, "\n")}\n</NEEDTAB_TreeNode>".Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        int count = 0, countOpen, countClose;
                        Regex regOpen = new Regex("<NEEDTAB_TreeNode>");
                        Regex regClose = new Regex("</NEEDTAB_TreeNode>");
                        for (long i = 0; i < lines.LongLength; i++)
                        {
                            countOpen = regOpen.Matches(lines[i]).Count;
                            countClose = regClose.Matches(lines[i]).Count;
                            count += countOpen - countClose;
                            if (count > 0)
                                lines[i] = lines[i].Insert(0, new string('\t', count));
                            if (countOpen != 0)
                                lines[i] = lines[i].Replace("<NEEDTAB_TreeNode>", "");
                            if (countClose != 0)
                                lines[i] = lines[i].Replace("</NEEDTAB_TreeNode>", "");
                        }
                        Regex needRemove = new Regex("^[\\t| ]+$");
                        List<string> LinesWithoutSpace = new List<string>(lines);
                        LinesWithoutSpace.RemoveAll(str => needRemove.Match(str).Success || str.Length == 0);
                        return string.Join("\n", LinesWithoutSpace);
                    }
                case StringFormat.Base:
                    return root.GetType().ToString();
                default:
                    throw new NotSupportedException($"Формат {sf} не поддерживается.");
            }
        }
    }
}
