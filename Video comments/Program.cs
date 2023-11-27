using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using static Solution;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine());
        var comments = new List<Comment>();
        for (int i = 0; i < n; i++)
        {
            string comment = Console.ReadLine();
            var split = comment.Split('|');
            var c = new Comment(split[0], DateTime.Parse(split[1]), long.Parse(split[2]), Enum.Parse<Priority>(split[3]), i);
            if (split[0].StartsWith("    "))
            {
                comments.Last().Replys.Add(c);
            }
            else
            {
                comments.Add(c);
            }
        }

        PrintCommentsPrio(comments);
    }

    private static void PrintCommentsPrio(List<Comment> comments)
    {
        if (comments.Count == 0) return;
        var pinned = comments.FirstOrDefault(x => x.Priority == Priority.Pinned);
        if (pinned is not null)
        {
            Console.WriteLine(pinned);
            PrintCommentsPrio(pinned.Replys);
        }
        var followed = comments.Where(x => x.Priority == Priority.Followed).OrderByDescending(x => x.Likes).ThenByDescending(x => x.Time).ThenBy(x => x.InputIndex);
        foreach (var comment in followed)
        {
            Console.WriteLine(comment);
            PrintCommentsPrio(comment.Replys);
        }
        var nonePrioComments = comments.Where(x => x.Priority == Priority.none).ToList();
        if (nonePrioComments.Count == 0) return;
        var maxLikes = nonePrioComments.Max(x => x.Likes);
        foreach (var comment in nonePrioComments.Where(x => x.Likes == maxLikes).OrderByDescending(x => x.Time).ThenBy(x => x.InputIndex))
        {
            Console.WriteLine(comment);
            PrintCommentsPrio(comment.Replys);
        }
        foreach (var comment in nonePrioComments.Where(x => x.Likes != maxLikes).OrderByDescending(x => x.Time).ThenBy(x => x.InputIndex))
        {
            Console.WriteLine(comment);
            PrintCommentsPrio(comment.Replys);
        }
    }

    public enum Priority { Pinned, Followed, none }

    public class Comment
    {
        public string User { get; set; }
        public DateTime Time { get; set; }
        public long Likes { get; set; }
        public Priority Priority { get; set; }
        public int InputIndex { get; set; }
        public bool IsReply => User.StartsWith("    ");
        public List<Comment> Replys { get; set; } = new List<Comment>();

        public Comment(string user, DateTime time, long likes, Priority priority, int inputIndex)
        {
            User = user;
            Time = time;
            Likes = likes;
            Priority = priority;
            InputIndex = inputIndex;
        }

        public override string ToString()
        {
            var time = Time.ToString("HH:mm");
            return $"{User}|{time}|{Likes}|{Priority}";
        }
    }
}