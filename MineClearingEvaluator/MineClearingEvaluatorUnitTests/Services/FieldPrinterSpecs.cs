using System;
using System.Collections.Generic;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using Machine.Specifications;
using MineClearingEvaluator.Models;
using MineClearingEvaluator.Services;

namespace MineClearingEvaluatorUnitTests.Services
{
    public class FieldPrinterSpecs
    {
        [Subject(typeof(FieldPrinter))]
        public class Concern : Observes<FieldPrinter>
        {
            private Establish context = () =>
            {
                CharacterDistanceConverter = depends.@on<ICharacterDistanceConverter>();
            };

            public static ICharacterDistanceConverter CharacterDistanceConverter { get; set; }
        }

        public class ConvertDistanceToCharacterConcern : Concern
        {
            private Establish context = () =>
            {
                CharacterDistanceConverter.WhenToldTo(x => x.ConvertDistanceToCharacter(1)).Return('a');
                CharacterDistanceConverter.WhenToldTo(x => x.ConvertDistanceToCharacter(2)).Return('b');
            };
            public static string Result { get; set; }
            public static Exception ExceptionResult { get; set; }
            public static Field Field { get; set; }

            public class When_converting_simple_field : ConvertDistanceToCharacterConcern
            {
                private Establish context = () => Field = new Field(new List<Mine>()
                {
                    new Mine(0, 0, 1),
                }, new Ship(0, 0, 0));
                private Because of = () => Result = sut.Print(Field);
                private It should_return_a = () => Result.ShouldEqual("a");
            }

            public class When_converting_complex_field : ConvertDistanceToCharacterConcern
            {
                private Establish context = () => Field = new Field(new List<Mine>()
                {
                    new Mine(0, 0, 1),
                    new Mine(2, 1, 2),
                    new Mine(1, 2, 1),
                }, new Ship(0, 2, 0));
                private Because of = () => Result = sut.Print(Field);
                private It should_return_a = () => Result.ShouldEqual("..a..\n....b\n...a.\n.....\n.....");
            }

        }
    }
}