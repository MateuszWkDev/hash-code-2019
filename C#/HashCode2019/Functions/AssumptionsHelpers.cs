using HashCode2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019.Functions
{
    public static class AssumptionsHelpers
    {
        public static StatementAssumption ExtractStatementAssumptions(List<string> fileLines)
        {
            var statemenAssumption = new StatementAssumption(int.Parse(fileLines[0]));
            fileLines.RemoveAt(0);
            return statemenAssumption;
        }
        public static List<Photo> ExtractPhotos(List<string> fileLines)
        {
            return fileLines.Select((line, index) =>
            {
                var lineList = line.Split(" ").ToList();
                return new Photo()
                {
                    Id = index,
                    Orientation = lineList[0],
                    NumberOfTags = int.Parse(lineList[1]),
                    Tags = lineList.Skip(2).ToList()
                };
            }).ToList();
        }
    }
}
