using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineClearingEvaluator;
using MineClearingEvaluator.Services;
using MineClearingEvaluatorIntegrationTests.Properties;

namespace MineClearingEvaluatorIntegrationTests
{
    [TestClass()]
    public class EvaluatorTests
    {
        public EvaluatorTests()
        {
            var container = new MyContainer();
            Evaluator = container.GetInstance<IEvaluator>();
        }

        public IEvaluator Evaluator { get; set; }

        [TestMethod()]
        public void Evaluator_evaluates_example_1()
        {
            var field = Resources.FieldFile1;
            var script = Resources.ScriptFile1;
            var output = Resources.Output1;

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(result, output);
        }
    }
}