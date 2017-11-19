namespace Marbles.Systems.Contracts
{
    public interface ITimeController : ISystem
    {
        void SetTimeScale(float scale);
        void ResetTimeScale();
    }
}
