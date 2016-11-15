using System.Collections.Generic;
using System.Linq;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface ISimulationFactory
    {
        ISimulation CreateSimulation(Field field, Queue<Instruction> instructions);
    }

    /// <summary>
    /// The simulation factory breaks the dependency between the Evaluator and the Simulation
    /// </summary>
    public class SimulationFactory : ISimulationFactory
    {
        public ISimulation CreateSimulation(Field field, Queue<Instruction> instructions)
        {
            return new Simulation(field, instructions);
        }
    }
}