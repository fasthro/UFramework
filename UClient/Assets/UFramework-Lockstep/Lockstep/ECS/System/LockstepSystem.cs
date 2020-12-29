using Entitas;

namespace Lockstep
{
    public class LockstepSystem : Feature
    {
        public LockstepSystem(Contexts contexts) : base("Lockstep System")
        {
            Add(new MoveSystem(contexts));
        }
    }
}