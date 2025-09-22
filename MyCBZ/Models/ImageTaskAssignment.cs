using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Extensions;

namespace Win_CBZ.Models
{

    [SupportedOSPlatform("windows")]
    internal class ImageTaskAssignment
    {

        public List<Page> Pages { get; set; }

        public ImageTask ImageTask { get; set; }


        public ImageTaskAssignment(List<Page> pages, ImageTask imageTask)
        {
            Pages = pages;
            ImageTask = imageTask;
        }

        public void AssignTaskToPages()
        {
            foreach (var page in Pages)
            {
                page.ImageTask = ImageTask;
            }
        }

        public string GetAssignedPageNumbers()
        {
            // 
            return Pages
                .Select(p => p.Number)
                .OrderBy(n => n)
                .Aggregate(new List<Tuple<int, int>>(), (sb, number) =>
                {
                    if (sb.Count > 0)
                    {
                        var last = sb.Last();
                        if (last.Item2 + 1 == number)
                        {
                            sb[sb.Count - 1] = new Tuple<int, int>(last.Item1, number);
                            return sb;
                        }
                        else if (last.Item2 == number)
                        {
                            sb.Add(new Tuple<int, int>(number, number));

                            return sb;

                        }
                    } else
                    {
                        sb.Add(new Tuple<int, int>(number, number));
                    }

                    
                    
                    return sb;
                })
                .Aggregate(new StringBuilder(), (sb, range) =>
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    if (range.Item1 == range.Item2)
                    {
                        sb.Append(range.Item1);
                    }
                    else
                    {
                        sb.Append($"{range.Item1}-{range.Item2}");
                    }
                    return sb;
                })
                .ToString();
        }
    }
}
