using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PollyMVVM.Models;

namespace PollyMVVM.Services.Abstractions
{
    public interface IStatesService
    {
        Task<List<State>> GetStates();
    }
}
