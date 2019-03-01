using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019.Models
{
    public class Slide
    {
        public Slide(Photo photo1, Photo photo2 =null)
        {
            Tags = photo1.Tags;
            Photos = new List<Photo>() { photo1 };
            
            if(photo2 != null)
            {
                Photos.Add(photo2);
                Tags.AddRange(photo2.Tags);
            }
            TagsCount = Tags.Count;
        }
        public List<Photo> Photos { get; set; }
        public List<string> Tags { get; set; }
        public int TagsCount { get; set; }

    }
}
