using System;
using System.Collections.Generic;
using System.Linq;
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
                    Result.ShouldNotBeNull();
                    Result.Count.ShouldEqual(0);
                };
            }

            public class When_parsing_empty_script : ParseConcern
            {
                private Establish context2 = () => ScriptText = "";
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_no_instructions = () =>
                {
                    Result.ShouldNotBeNull();
                    Result.Count.ShouldEqual(0);
                };
            }

            public class When_parsing_valid_script : ParseConcern
            {
                private Establish context2 = () => ScriptText = "north\nsouth\neast\nwest\nalpha\nbeta\ngamma\ndelta\nalpha north\n";
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_instructions = () =>
                {
                    Result.ShouldNotBeNull();
                    Result.Count.ShouldEqual(9);

                    var resultList = Result.ToList();

                    resultList[0].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[0].Direction.ShouldEqual(Direction.North);

                    resultList[1].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[1].Direction.ShouldEqual(Direction.South);

                    resultList[2].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[2].Direction.ShouldEqual(Direction.East);

                    resultList[3].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[3].Direction.ShouldEqual(Direction.West);

                    resultList[4].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    resultList[4].Direction.ShouldEqual(Direction.None);

                    resultList[5].FiringPattern.ShouldEqual(FiringPattern.Beta);
                    resultList[5].Direction.ShouldEqual(Direction.None);

                    resultList[6].FiringPattern.ShouldEqual(FiringPattern.Gamma);
                    resultList[6].Direction.ShouldEqual(Direction.None);

                    resultList[7].FiringPattern.ShouldEqual(FiringPattern.Delta);
                    resultList[7].Direction.ShouldEqual(Direction.None);

                    resultList[8].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    resultList[8].Direction.ShouldEqual(Direction.North);
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

            public static Queue<Instruction> Result { get; set; }

            public static string ScriptText { get; set; }
        }
    }
}