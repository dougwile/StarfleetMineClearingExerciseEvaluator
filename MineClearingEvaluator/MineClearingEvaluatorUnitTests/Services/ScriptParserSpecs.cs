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
                private Establish context2 = () => ScriptText = "north\nsouth\neast\nwest\nalpha\nbeta\ngamma\ndelta\nalpha north\nsouth beta\n";
                private Because of = () => Result = sut.Parse(ScriptText);
                private It should_return_script = () => Result.ShouldNotBeNull();
                private It should_return_instructions = () =>
                {
                    Result.ShouldNotBeNull();
                    Result.Count.ShouldEqual(10);

                    var resultList = Result.ToList();

                    resultList[0].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[0].Direction.ShouldEqual(Direction.North);
                    resultList[0].ShootFirst.ShouldEqual(false);
                    resultList[0].Text.ShouldEqual("north");

                    resultList[1].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[1].Direction.ShouldEqual(Direction.South);
                    resultList[0].ShootFirst.ShouldEqual(false);
                    resultList[1].Text.ShouldEqual("south");

                    resultList[2].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[2].Direction.ShouldEqual(Direction.East);
                    resultList[2].ShootFirst.ShouldEqual(false);
                    resultList[2].Text.ShouldEqual("east");

                    resultList[3].FiringPattern.ShouldEqual(FiringPattern.None);
                    resultList[3].Direction.ShouldEqual(Direction.West);
                    resultList[3].ShootFirst.ShouldEqual(false);
                    resultList[3].Text.ShouldEqual("west");

                    resultList[4].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    resultList[4].Direction.ShouldEqual(Direction.None);
                    resultList[4].ShootFirst.ShouldEqual(true);
                    resultList[4].Text.ShouldEqual("alpha");

                    resultList[5].FiringPattern.ShouldEqual(FiringPattern.Beta);
                    resultList[5].Direction.ShouldEqual(Direction.None);
                    resultList[5].ShootFirst.ShouldEqual(true);
                    resultList[5].Text.ShouldEqual("beta");

                    resultList[6].FiringPattern.ShouldEqual(FiringPattern.Gamma);
                    resultList[6].Direction.ShouldEqual(Direction.None);
                    resultList[6].ShootFirst.ShouldEqual(true);
                    resultList[6].Text.ShouldEqual("gamma");

                    resultList[7].FiringPattern.ShouldEqual(FiringPattern.Delta);
                    resultList[7].Direction.ShouldEqual(Direction.None);
                    resultList[7].ShootFirst.ShouldEqual(true);
                    resultList[7].Text.ShouldEqual("delta");

                    resultList[8].FiringPattern.ShouldEqual(FiringPattern.Alpha);
                    resultList[8].Direction.ShouldEqual(Direction.North);
                    resultList[8].ShootFirst.ShouldEqual(true);
                    resultList[8].Text.ShouldEqual("alpha north");

                    resultList[9].FiringPattern.ShouldEqual(FiringPattern.Beta);
                    resultList[9].Direction.ShouldEqual(Direction.South);
                    resultList[9].ShootFirst.ShouldEqual(false);
                    resultList[9].Text.ShouldEqual("south beta");
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