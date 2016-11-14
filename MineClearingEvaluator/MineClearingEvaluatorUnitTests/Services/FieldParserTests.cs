using System;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using Machine.Specifications;
using MineClearingEvaluator.Models;
using MineClearingEvaluator.Services;

namespace MineClearingEvaluatorUnitTests.Services
{
    public class FieldParserSpecs
    {
        [Subject(typeof(FieldParser))]
        public class Concern : Observes<FieldParser>
        {
            private Establish context = () =>
            {
                CharacterDistanceConverter = depends.@on<ICharacterDistanceConverter>();
                
            };

            public static ICharacterDistanceConverter CharacterDistanceConverter { get; set; }
        }

        public class ParseConcern : Concern
        {
            private Establish context = () =>
            {
                CharacterDistanceConverter.WhenToldTo(x => x.ConvertCharacterToDistance('a')).Return(1);
                CharacterDistanceConverter.WhenToldTo(x => x.ConvertCharacterToDistance('b')).Return(2);
                CharacterDistanceConverter.WhenToldTo(x => x.ConvertCharacterToDistance('c')).Return(3);
            };


            public class When_parsing_null_field : ParseConcern
            {
                private Establish context2 = () => FieldText = null;
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.Parse(FieldText));
                public static Exception ExceptionResult { get; set; }

                private It should_throw_exception_when_empty = () => ExceptionResult.ShouldBeAssignableTo(typeof(NullReferenceException));
            }

            public class When_parsing_simple_field : ParseConcern
            {
                private Establish context2 = () => FieldText = "a";
                private Because of = () => Result = sut.Parse(FieldText);
                private It should_return_field = () => Result.ShouldNotBeNull();
                private It should_return_field_with_ship = () =>
                {
                    Result.Ship.ShouldNotBeNull();
                    Result.Ship.Coordinates.X.ShouldEqual(0);
                    Result.Ship.Coordinates.Y.ShouldEqual(0);
                    Result.Ship.Coordinates.Z.ShouldEqual(0);
                };
                private It should_return_field_with_mine = () =>
                {
                    Result.Mines.ShouldNotBeNull();
                    Result.Mines.Count.ShouldEqual(1);
                    Result.Mines[0].Coordinates.X.ShouldEqual(0);
                    Result.Mines[0].Coordinates.Y.ShouldEqual(0);
                    Result.Mines[0].Coordinates.Z.ShouldEqual(1);
                };
            }

            public class When_parsing_field_with_no_mines : ParseConcern
            {
                private Establish context2 = () => FieldText = ".";
                private Because of = () => Result = sut.Parse(FieldText);
                private It should_return_field = () => Result.ShouldNotBeNull();
                private It should_return_field_with_ship = () =>
                {
                    Result.Ship.ShouldNotBeNull();
                    Result.Ship.Coordinates.X.ShouldEqual(0);
                    Result.Ship.Coordinates.Y.ShouldEqual(0);
                    Result.Ship.Coordinates.Z.ShouldEqual(0);
                };
                private It should_return_field_with_zero_mines = () =>
                {
                    Result.Mines.ShouldNotBeNull();
                    Result.Mines.Count.ShouldEqual(0);
                };
            }

            public class When_parsing_complex_field : ParseConcern
            {
                private Establish context2 = () => FieldText = ".a.\nb..\n...\n...\n..c";
                private Because of = () => Result = sut.Parse(FieldText);
                private It should_return_field = () => Result.ShouldNotBeNull();
                private It should_return_field_with_ship = () =>
                {
                    Result.Ship.ShouldNotBeNull();
                    Result.Ship.Coordinates.X.ShouldEqual(1);
                    Result.Ship.Coordinates.Y.ShouldEqual(2);
                    Result.Ship.Coordinates.Z.ShouldEqual(0);
                };
                private It should_return_field_with_three_mines = () =>
                {
                    Result.Mines.ShouldNotBeNull();
                    Result.Mines.Count.ShouldEqual(3);

                    Result.Mines[0].Coordinates.X.ShouldEqual(1);
                    Result.Mines[0].Coordinates.Y.ShouldEqual(0);
                    Result.Mines[0].Coordinates.Z.ShouldEqual(1);

                    Result.Mines[1].Coordinates.X.ShouldEqual(0);
                    Result.Mines[1].Coordinates.Y.ShouldEqual(1);
                    Result.Mines[1].Coordinates.Z.ShouldEqual(2);

                    Result.Mines[2].Coordinates.X.ShouldEqual(2);
                    Result.Mines[2].Coordinates.Y.ShouldEqual(4);
                    Result.Mines[2].Coordinates.Z.ShouldEqual(3);
                };
            }

            public static Field Result { get; set; }

            public static string FieldText { get; set; }
        }
    }
}