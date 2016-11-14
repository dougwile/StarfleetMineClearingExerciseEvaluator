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
        public IDictionary<char, int> CharacterToDistanceMap { get; set; }
        public IDictionary<int, char> DistanceToCharacterMap { get; set; }

        public CharacterDistanceConverter()
        {
            CharacterToDistanceMap = new Dictionary<char, int>();
            DistanceToCharacterMap = new Dictionary<int, char>();

            const string charcters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < charcters.Length; i++)
            {
                CharacterToDistanceMap.Add(charcters[i], i + 1);
                DistanceToCharacterMap.Add(i + 1, charcters[i]);
            }
        }

        public char ConvertDistanceToCharacter(int distance)
        {
            return DistanceToCharacterMap[distance];
        }

        public int ConvertCharacterToDistance(char character)
        {
            return CharacterToDistanceMap[character];
        }
    }
}