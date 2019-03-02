using HashCode2019.Functions;
using HashCode2019.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HashCode2019
{
    class Program
    {
        public const int FileIndex = 3;
        public const int MaxTasks = 40;
        public const int SlidesPerTask = 500;
        public static readonly int? MinSlideScore = null;
        public const bool UseMultipleThreads = false;
        public static async Task Main(string[] args)
        {
            var allFilesLines = FileHelpers.GetFilesLines();

            var wholeFileWatch = Stopwatch.StartNew();
            var fileLines = allFilesLines[FileIndex];
            var assumption = AssumptionsHelpers.ExtractStatementAssumptions(fileLines);
            var photos = AssumptionsHelpers.ExtractPhotos(fileLines);
            var slidesWithHorizontalPhotos = photos.Where(photo => photo.Orientation == "H").Select(photo => new Slide(photo)).ToList();
            var slidesWithVerticalPhotos = AlgorithmHelpers.CreateSlidesFromVerticalPhotos(photos.Where(photo => photo.Orientation == "V").ToList());
            var sortedSlides = slidesWithHorizontalPhotos.Concat(slidesWithVerticalPhotos).OrderByDescending(slide => slide.TagsCount).ToList();
            //var sortedSlides = slidesWithHorizontalPhotos.Concat(slidesWithVerticalPhotos).ToList();
            var slideShow = new List<Slide>(){
               sortedSlides[0]
            };
            sortedSlides.RemoveAt(0);
            var iterationWatch = Stopwatch.StartNew();
            while (true)
            {
                var sortedSlidesLen = sortedSlides.Count;
                var baseSlide = slideShow.Last();
                Slide bestSlide = null;
                if (sortedSlidesLen == 0)
                {
                    break;
                }
                if (UseMultipleThreads && sortedSlidesLen > SlidesPerTask)
                {
                    var chunks = AlgorithmHelpers.SplitList(sortedSlides, SlidesPerTask).ToList();
                    var chunksToCount = chunks.Take(chunks.Count > MaxTasks ? MaxTasks : chunks.Count).ToList();
                    if (sortedSlidesLen % 1000 == 0)
                    {
                        Console.WriteLine($"Chunks: {chunks.Count}");
                    }
                    var tasks = new List<Task<Tuple<int, Slide>>>();
                    chunksToCount.ForEach(chunk => tasks.Add(AlgorithmHelpers.FindBestSildeInChunk(baseSlide, chunk, MinSlideScore)));
                    var tupleList = await Task.WhenAll(tasks.ToArray());
                    bestSlide = tupleList.OrderByDescending(slideTuple => slideTuple.Item1).FirstOrDefault().Item2;
                }
                else
                {
                    //bestSlide = (await AlgorithmHelpers.FindBestSildeInChunk(baseSlide, sortedSlides, MinSlideScore)).Item2;
                    bestSlide = (await AlgorithmHelpers.FindBestSildeInChunk(baseSlide, sortedSlides.Take(sortedSlidesLen > SlidesPerTask * MaxTasks ? SlidesPerTask * MaxTasks : sortedSlidesLen).ToList(), MinSlideScore)).Item2;
                }
                slideShow.Add(bestSlide);
                sortedSlides.Remove(bestSlide);
                if (sortedSlidesLen % 100 == 0)
                {
                    Console.WriteLine($"Slides left: {sortedSlidesLen} elapsed seconds: {(float)iterationWatch.ElapsedMilliseconds / 1000}");
                    iterationWatch.Restart();
                }
            }
            FileHelpers.PrepareResults(slideShow, FileIndex);
            Console.WriteLine($"Finished file {FileIndex} in seconds: {(float)wholeFileWatch.ElapsedMilliseconds / 1000}");
            Console.WriteLine($"Finished file {FileIndex} in minutes: {(float)wholeFileWatch.ElapsedMilliseconds / 60000}");
            Console.ReadLine();
        }
    }
}
