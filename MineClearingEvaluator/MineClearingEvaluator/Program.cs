using System;
using System.IO;
using System.Linq;
using MineClearingEvaluator.Services;
using SimpleInjector;

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

            var container = new MyContainer();

            var fieldFilePath = args[0];
            var scriptFilePath = args[1];

            var field = File.ReadAllText(fieldFilePath);
            var script = File.ReadAllText(scriptFilePath);

            var evaluator = container.GetInstance<IEvaluator>();

            var result = evaluator.Evaluate(field, script);

            Console.Write(result);
        }

        public static void SetUpIoC()
        {
            

        }

    }

    public class MyContainer : Container
    {
        public MyContainer()
        {
            // Configure the container (register)
            this.Register<IEvaluator, Evaluator>();
            this.Register<ICharacterDistanceConverter, CharacterDistanceConverter>();
            this.Register<IFieldParser, FieldParser>();
            this.Register<IScriptParser, ScriptParser>();

            // Verify the container's configuration.
            this.Verify();
        }
    }

    
}
