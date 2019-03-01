using HashCode2019.Functions;
using HashCode2019.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HashCode2019
{
    class Program
    {
        public const int FileIndex = 1;
        public const int MinSlideScore =3;
        static void Main(string[] args)
        {
            var allFilesLines = FileHelpers.GetFilesLines();

            var wholeFileWatch = Stopwatch.StartNew();
            var fileLines = allFilesLines[FileIndex];
            var assumption = AssumptionsHelpers.ExtractStatementAssumptions(fileLines);
            var photos = AssumptionsHelpers.ExtractPhotos(fileLines);
            var slidesWithHorizontalPhotos = photos.Where(photo => photo.Orientation == "H").Select(photo=> new Slide(photo)).ToList();
            var slidesWithVerticalPhotos = AlgorithmHelpers.CreateSlidesFromVerticalPhotos(photos.Where(photo => photo.Orientation == "V").ToList());
            var sortedSlides = slidesWithHorizontalPhotos.Concat(slidesWithVerticalPhotos).OrderByDescending(slide => slide.TagsCount).ToList();

            var slideShow = new List<Slide>(){
               sortedSlides[0]
            };
            sortedSlides.RemoveAt(0);
            var iterationWatch = Stopwatch.StartNew();
            while (true)
            {
                var bestScore = -1;
                Slide bestSlide = null;
                var sortedSlidesLen = sortedSlides.Count;
                var baseSlide = slideShow.Last();
                if (sortedSlidesLen == 0)
                {
                    break;
                }
                for (var i = 0; i < sortedSlidesLen; i = i + 1)
                {
                    var slide = sortedSlides[i];
                    var score = AlgorithmHelpers.CountScoreOfSlides(baseSlide, slide);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestSlide = slide;
                    }
                    if(bestScore>= MinSlideScore)
                    {
                        break;
                    }
                }
                slideShow.Add(bestSlide);
                sortedSlides.Remove(bestSlide);
                if (sortedSlidesLen%100 == 0)
                {
                    Console.WriteLine($"Slides left: {sortedSlidesLen} elapsed seconds: {(float)iterationWatch.ElapsedMilliseconds / 1000}");
                    iterationWatch.Restart();
                }
            }
            FileHelpers.PrepareResults(slideShow, FileIndex);
            Console.WriteLine($"Finished file {FileIndex} in {(float)wholeFileWatch.ElapsedMilliseconds / 1000}");
            Console.ReadLine();
        }
    }
}
