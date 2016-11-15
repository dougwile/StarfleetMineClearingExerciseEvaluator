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
            /* I added light validation at the entry point to ensure the program gets both input files. 
             * However, I made the assumption that I, the instructor, am using the program and thus will
             * trust myself enough to not do anything too stupid with the input. 
            */
            if (args.Length != 2)
            {
                throw new ArgumentException("Must provide field file and script file as arguments");
            }
            
            /* I included an inversion of control container for a number of reasons. First, IoC and dependency injectio
             * helps eliminate nasty depedency chains (Dependency inversion principle from SOLID). Second, by
             * breaking dependency chains, my code gets much more testable since I can mock out the dependencies.
             * Third, I have never personally set up an IoC framework for a project myself and thought this would be a
             * nice learning experience. It turns out that SimpleInjector is super easy to use, although I found it
             * frustrating that the only way to set up automatic registration of interfaces with a single implementation
             * is with reflection and LINQ. I opted to just manually register my interfaces and implementations instead of
             * bothering with automatic registration since this keeps it simpler for a small project.
             */
            var container = new MyContainer();

            var fieldFilePath = args[0];
            var scriptFilePath = args[1];

            var field = File.ReadAllText(fieldFilePath);
            var script = File.ReadAllText(scriptFilePath);

            /* This is the only time I call container.GetInstance<T>(). I let the automatic dependency injection instantiate
             * all of my other services with complex depenedencies.
             */
            var evaluator = container.GetInstance<IEvaluator>();

            var result = evaluator.Evaluate(field, script);

            Console.Write(result);
            Console.ReadLine();
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
            this.Register<IFieldPrinter, FieldPrinter>();
            this.Register<ISimulationFactory, SimulationFactory>();

            // Verify the container's configuration.
            this.Verify();
        }
    }

    
}
