namespace Marbles.Systems.Contracts
{
    public interface ITimeController : ISystem
    {
        void SetTimeScale(float scale);
        void SetTimeScale(float scale, float refreshTime);
        void ResetTimeScale();
    }
}
