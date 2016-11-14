using System;
using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;
using MineClearingEvaluator.Services;

namespace MineClearingEvaluatorUnitTests.Services
{
    public class CharacterDistanceConverterSpecs
    {
        [Subject(typeof(CharacterDistanceConverter))]
        public class Concern : Observes<CharacterDistanceConverter>
        {
            private Establish context = () =>
            {
            };
        }

        public class ConvertDistanceToCharacterConcern : Concern
        {
            private Establish context = () =>
            {
            };
            public static char Result { get; set; }
            public static Exception ExceptionResult { get; set; }

            public class When_converting_1 : ConvertDistanceToCharacterConcern
            {
                private Because of = () => Result = sut.ConvertDistanceToCharacter(1);
                private It should_return_a = () => Result.ShouldEqual('a');
            }

            public class When_converting_52 : ConvertDistanceToCharacterConcern
            {
                private Because of = () => Result = sut.ConvertDistanceToCharacter(52);
                private It should_return_Z = () => Result.ShouldEqual('Z');
            }

            public class When_converting_negative : ConvertDistanceToCharacterConcern
            {
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.ConvertDistanceToCharacter(-1));
                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ArgumentException));
            }

            public class When_converting_large : ConvertDistanceToCharacterConcern
            {
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.ConvertDistanceToCharacter(100));
                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ArgumentException));
            }
        }

        public class ConvertCharacterToDistanceConcern : Concern
        {
            private Establish context = () =>
            {
            };
            public static int Result { get; set; }
            public static Exception ExceptionResult { get; set; }

            public class When_converting_a : ConvertCharacterToDistanceConcern
            {
                private Because of = () => Result = sut.ConvertCharacterToDistance('a');
                private It should_return_1 = () => Result.ShouldEqual(1);
            }

            public class When_converting_Z : ConvertCharacterToDistanceConcern
            {
                private Because of = () => Result = sut.ConvertCharacterToDistance('Z');
                private It should_return_52 = () => Result.ShouldEqual(52);
            }

            public class When_converting_invalid : ConvertCharacterToDistanceConcern
            {
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.ConvertCharacterToDistance('@'));
                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ArgumentException));
            }
        }
    }
}