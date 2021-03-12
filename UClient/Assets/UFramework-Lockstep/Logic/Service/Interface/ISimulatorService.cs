﻿// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Logic.Simulator;

namespace Lockstep.Logic
{
    public interface ISimulatorService : IService
    {
        SimulatorPing ping { get; }
    }
}