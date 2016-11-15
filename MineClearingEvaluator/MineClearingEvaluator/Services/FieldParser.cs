using System;
using System.Collections.Generic;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IFieldParser
    {
        Field Parse(string fieldText);
    }

    /// <summary>
    /// The field parser takes the field text as input and builds an object representing the field.
    /// The field contains the mines and the ship in a coordinate system.
    /// </summary>
    public class FieldParser : IFieldParser
    {
        private readonly ICharacterDistanceConverter _characterDistanceConverter;

        public FieldParser(ICharacterDistanceConverter characterDistanceConverter)
        {
            _characterDistanceConverter = characterDistanceConverter;
        }

        public Field Parse(string fieldText)
        {
            /* I assume the field text is valid since it is ostensibly provided by me, the Instructor
             * One potential improvement is to validate the field text if it were to be deemed necessary
             * in the future
             */

            var mines = new List<Mine>();

            var lines = fieldText.Trim().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    var character = line[j];
                    if (character == '.')
                    {
                        continue;
                    }
                    var distance = _characterDistanceConverter.ConvertCharacterToDistance(character);
                    var mine = new Mine(j, i, distance);
                    mines.Add(mine);
                }
            }

            var width = lines[0].Length;
            var length = lines.Length;

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