using Marbles.Enums;

namespace Marbles.Systems.Contracts
{
    public interface IBattleManager
    {
        BattleTurn BattleTurn { get; }
        void SetTurn(BattleTurn battleTurn);
    }
}
