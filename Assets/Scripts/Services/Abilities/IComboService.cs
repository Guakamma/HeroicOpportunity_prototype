using Data.Abilities;

namespace Services.Abilities
{
    public interface IComboService
    {
        ComboInfo[] GetAllComboInfos();
        ComboInfo GetRandomComboInfo();
    }
}