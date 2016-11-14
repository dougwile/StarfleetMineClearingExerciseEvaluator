using System;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using Machine.Specifications;
using MineClearingEvaluator;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;
using MineClearingEvaluator.Services;

namespace MineClearingEvaluatorUnitTests.Services
{
    public class ScriptParserSpecs
    {
        [Subject(typeof(ScriptParser))]
        public class Concern : Observes<ScriptParser>
        {
            private Establish context = () =>
            {
            };
        }

        public class ParseConcern : Concern
        {
            private Establish context = () =>
            {
            };


            public class When_parsing_null_script : ParseConcern
            {
                private Establish context2 = () => ScriptText = null;
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_no_instructions = () =>
                {
                    Result.Instructions.ShouldNotBeNull();
                    Result.Instructions.Count.ShouldEqual(0);
                };
            }

            public class When_parsing_empty_script : ParseConcern
            {
                private Establish context2 = () => ScriptText = "";
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_no_instructions = () =>
                {
                    Result.Instructions.ShouldNotBeNull();
                    Result.Instructions.Count.ShouldEqual(0);
                };
            }

            public class When_parsing_valid_script : ParseConcern
            {
                private Establish context2 = () => ScriptText = "north\r\nsouth\neast\nwest\nalpha\nbeta\ngamma\ndelta\r\nalpha north\r\n";
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_instructions = () =>
                {
                    Result.Instructions.ShouldNotBeNull();
                    Result.Instructions.Count.ShouldEqual(9);

                    Result.Instructions[0].FiringPattern.ShouldEqual(FiringPattern.None);
                    Result.Instructions[0].Direction.ShouldEqual(Direction.North);

                    Result.Instructions[1].FiringPattern.ShouldEqual(FiringPattern.None);
                    Result.Instructions[1].Direction.ShouldEqual(Direction.South);

                    Result.Instructions[2].FiringPattern.ShouldEqual(FiringPattern.None);
                    Result.Instructions[2].Direction.ShouldEqual(Direction.East);

                    Result.Instructions[3].FiringPattern.ShouldEqual(FiringPattern.None);
                    Result.Instructions[3].Direction.ShouldEqual(Direction.West);

                    Result.Instructions[4].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    Result.Instructions[4].Direction.ShouldEqual(Direction.None);

                    Result.Instructions[5].FiringPattern.ShouldEqual(FiringPattern.Beta);
                    Result.Instructions[5].Direction.ShouldEqual(Direction.None);

                    Result.Instructions[6].FiringPattern.ShouldEqual(FiringPattern.Gamma);
                    Result.Instructions[6].Direction.ShouldEqual(Direction.None);

                    Result.Instructions[7].FiringPattern.ShouldEqual(FiringPattern.Delta);
                    Result.Instructions[7].Direction.ShouldEqual(Direction.None);

                    Result.Instructions[8].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    Result.Instructions[8].Direction.ShouldEqual(Direction.North);
                };
            }

            public class When_parsing_script_with_invalid_firing_pattern : ParseConcern
            {
                private Establish context2 = () => ScriptText = "foo north";
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.Parse(ScriptText));
                public static Exception ExceptionResult { get; set; }

                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ValidationException));
            }

            public class When_parsing_script_with_invalid_direction : ParseConcern
            {
                private Establish context2 = () => ScriptText = "alpha foo";
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.Parse(ScriptText));
                public static Exception ExceptionResult { get; set; }

                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ValidationException));
            }

            public class When_parsing_script_with_invalid_instruction : ParseConcern
            {
                private Establish context2 = () => ScriptText = "foo";
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.Parse(ScriptText));
                public static Exception ExceptionResult { get; set; }

                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ValidationException));
            }

            public class When_parsing_script_with_invalid_line : ParseConcern
            {
                private Establish context2 = () => ScriptText = "alpha north foo";
                private Because of = () => ExceptionResult = Catch.Exception(() => sut.Parse(ScriptText));
                public static Exception ExceptionResult { get; set; }

                private It should_throw_exception = () => ExceptionResult.ShouldBeAssignableTo(typeof(ValidationException));
            }

            public static Script Result { get; set; }

            public static string ScriptText { get; set; }
        }
    }
}