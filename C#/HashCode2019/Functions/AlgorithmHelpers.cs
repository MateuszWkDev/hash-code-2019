using HashCode2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019.Functions
{
    public static class AlgorithmHelpers
    {
        public static List<Slide> CreateSlidesFromVerticalPhotos(List<Photo> verticalPhotos)
        {
            var slides = new List<Slide>();
            if (verticalPhotos.Count==0)
            {
                return slides;
            }
            for (var i = 0; i<verticalPhotos.Count; i = i + 2)
            {
                slides.Add(new Slide(verticalPhotos[i], verticalPhotos[i + 1]));
            }
            return slides;
        }
        public static int CountScoreOfSlides(Slide slide1, Slide slide2)
        {
            var pointsList = new List<int>();
            pointsList.Add(slide1.Tags.Intersect(slide2.Tags).Count());
            pointsList.Add(slide1.TagsCount - pointsList[0]);
            pointsList.Add(slide2.TagsCount - pointsList[0]);
            return pointsList.Min();
        }
        public static Task<Tuple<int,Slide>> FindBestSildeInChunk(Slide baseSlide, List<Slide> slides, int minScore)
        {
            return Task.Run(() => {
                var bestScore = -1;
                Slide bestSlide = null;
                for (var i = 0; i < slides.Count; i = i + 1)
                {
                    var slide = slides[i];
                    var score = AlgorithmHelpers.CountScoreOfSlides(baseSlide, slide);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestSlide = slide;
                    }
                    if (bestScore >= minScore)
                    {
                        break;
                    }
                }
                return Tuple.Create(bestScore, bestSlide);
            });                  
        }
        public static IEnumerable<List<T>> SplitList<T>(List<T> lists, int nSize = 5000)
        {
            for (int i = 0; i < lists.Count; i += nSize)
            {
                yield return lists.GetRange(i, Math.Min(nSize, lists.Count - i));
            }
        }
    }
}
