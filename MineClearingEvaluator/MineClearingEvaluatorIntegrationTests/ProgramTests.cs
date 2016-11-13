using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineClearingEvaluator;

namespace MineClearingEvaluatorIntegrationTests
{
    [TestClass()]
    public class ProgramTests
    {
        public ProgramTests()
        {
            Program = new Program();
        }

        public Program Program { get; set; }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void Main_Throws_Exception_When_No_Args_Provided()
        {
            Program.Main(new string[0]);
        }

        [ExpectedException(typeof(FileNotFoundException))]
        [TestMethod()]
        public void Main_Outputs_Foo()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                Program.Main(new string[2] { "../", "bar" });

                string expected = string.Format("Ploeh{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }


    }
}