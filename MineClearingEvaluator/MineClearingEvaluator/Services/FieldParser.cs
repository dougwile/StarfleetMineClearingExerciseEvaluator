using System.Collections.Generic;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IFieldParser
    {
        Field Parse(string fieldText);
    }

    public class FieldParser : IFieldParser
    {
        private readonly ICharacterDistanceConverter _characterDistanceConverter;

        public FieldParser(ICharacterDistanceConverter characterDistanceConverter)
        {
            _characterDistanceConverter = characterDistanceConverter;
        }

        public Field Parse(string fieldText)
        {
            // assume the field text is valid since it is provided by the instructor
            //TODO: add validation if it becomes an issue in the future

            var mines = new List<Mine>();

            var lines = fieldText.Trim().Split(new[] { '\r', '\n' });

            

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    var character = line[j];
                    if (character == '.')
                    {
                        continue;
                    }
                    var distance = _characterDistanceConverter.ConvertCharacterToDistance(character);
                    var mine = new Mine(i, j, distance);
                    mines.Add(mine);
                }
            }

            var width = lines.Length;
            var length = lines[0].Length;

            /* The ship starts in the center of the grid. This means that a valid grid has an odd 
             * number of rows and columns. Because the grid is zero indexed, we can get the center
             * by dividing the length and width by two and removing the remainder.
             */
            var shipX = (int) (width/2); 
            var shipY = (int)(length / 2);
            var shipZ = 0;

            var ship = new Ship(shipX, shipY, shipZ);

            return new Field(mines, ship);
        }
    }
}