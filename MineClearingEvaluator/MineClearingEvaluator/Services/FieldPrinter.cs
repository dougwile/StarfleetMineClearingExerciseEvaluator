using System;
using System.Collections.Generic;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IFieldPrinter
    {
        string Print(Field field);
    }

    public class FieldPrinter: IFieldPrinter
    {
        private readonly ICharacterDistanceConverter _characterDistanceConverter;

        public FieldPrinter(
            ICharacterDistanceConverter characterDistanceConverter)
        {
            _characterDistanceConverter = characterDistanceConverter;
        }

        public string Print(Field field)
        {
            var shipCoords = field.Ship.Coordinates;

            // find greatest X and Y distance between ship and a mine
            // length and width will be equal to twice this distance plus one to center on the ship

            var maxX = 0;
            var maxY = 0;

            foreach (var mine in field.Mines)
            {
                maxX = Math.Max(maxX, Math.Abs(shipCoords.X - mine.Coordinates.X));
                maxY = Math.Max(maxY, Math.Abs(shipCoords.Y - mine.Coordinates.Y));
            }

            var width = 2*maxX + 1;
            var length = 2*maxY + 1;

            var grid = new char[width, length];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    grid[i,j] = '.';
                }
            }

            var centerX = (int) width/2;
            var centerY = (int) length/2;
            var gridOffset = new Offset(shipCoords.X - centerX, shipCoords.Y - centerY);

            foreach (var mine in field.Mines)
            {
                var mineX = mine.Coordinates.X - gridOffset.X;
                var mineY = mine.Coordinates.Y - gridOffset.Y;
                var mineZ = mine.Coordinates.Z - shipCoords.Z;
                var mineCharacter = _characterDistanceConverter.ConvertDistanceToCharacter(mineZ);
                grid[mineX, mineY] = mineCharacter;
            }

            var output = "";
            for (int j = 0; j < length; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    output += grid[i, j];
                }
                output += '\n';
            }

            return output.Trim();
        }
    }
}