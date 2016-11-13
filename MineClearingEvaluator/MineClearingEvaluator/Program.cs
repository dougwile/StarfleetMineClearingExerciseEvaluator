using System;
using System.IO;

namespace MineClearingEvaluator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Must provide field file and script file as arguments");
            }

            var fieldFilePath = args[0];
            var scriptFilePath = args[1];

            var field = File.ReadAllText(fieldFilePath);
            var script = File.ReadAllText(scriptFilePath);
            
            var evaluator = new Evaluator();

            var result = evaluator.Evaluate(field, script);

            Console.Write(result);
        }
    }
}
