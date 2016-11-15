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
            var output = Resources.Output1.Replace("\r", ""); // get rid of carriage returns because Windows is annoying

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(output, result);
        }

        [TestMethod()]
        public void Evaluator_evaluates_example_2()
        {
            var field = Resources.FieldFile2;
            var script = Resources.ScriptFile2;
            var output = Resources.Output2.Replace("\r", "");

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(output, result);
        }

        [TestMethod()]
        public void Evaluator_evaluates_example_3()
        {
            var field = Resources.FieldFile3;
            var script = Resources.ScriptFile3;
            var output = Resources.Output3.Replace("\r", "");

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(output, result);
        }

        [TestMethod()]
        public void Evaluator_evaluates_example_4()
        {
            var field = Resources.FieldFile4;
            var script = Resources.ScriptFile4;
            var output = Resources.Output4.Replace("\r", "");

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(output, result);
        }

        [TestMethod()]
        public void Evaluator_evaluates_example_5()
        {
            var field = Resources.FieldFile5;
            var script = Resources.ScriptFile5;
            var output = Resources.Output5.Replace("\r", "");

            var result = Evaluator.Evaluate(field, script);
            Assert.AreEqual(output, result);
        }
    }
}