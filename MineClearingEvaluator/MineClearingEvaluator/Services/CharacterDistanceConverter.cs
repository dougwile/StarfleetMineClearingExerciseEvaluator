using System;
using System.Collections.Generic;

namespace MineClearingEvaluator.Services
{
    public interface ICharacterDistanceConverter
    {
        char ConvertDistanceToCharacter(int distance);
        int ConvertCharacterToDistance(char character);
    }

    public class CharacterDistanceConverter : ICharacterDistanceConverter
    {
        private readonly IDictionary<char, int> _characterToDistanceMap;
        private readonly IDictionary<int, char> _distanceToCharacterMap;

        public CharacterDistanceConverter()
        {
            _characterToDistanceMap = new Dictionary<char, int>();
            _distanceToCharacterMap = new Dictionary<int, char>();

            const string charcters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < charcters.Length; i++)
            {
                _characterToDistanceMap.Add(charcters[i], i + 1);
                _distanceToCharacterMap.Add(i + 1, charcters[i]);
            }
        }

        public char ConvertDistanceToCharacter(int distance)
        {
            if (distance <= 0)
            {
                return '*';
            }

            if (!_distanceToCharacterMap.ContainsKey(distance))
            {
                throw new ArgumentException("Invalid distance");
            }

            return _distanceToCharacterMap[distance];
        }

        public int ConvertCharacterToDistance(char character)
        {
            if (!_characterToDistanceMap.ContainsKey(character))
            {
                throw new ArgumentException("Invalid character");
            }

            return _characterToDistanceMap[character];
        }
    }
}