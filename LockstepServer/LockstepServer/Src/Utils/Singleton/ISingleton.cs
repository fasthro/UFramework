using System;

namespace LockstepServer
{
    public interface ISingleton : IDisposable
    {
        void Initialize();
    }
}
