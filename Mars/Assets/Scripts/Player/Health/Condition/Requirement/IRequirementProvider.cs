namespace Scripts.Player.Health.Condition
{
    public interface IRequirementProvider
    {
        bool TryGetValue(Player player, out float requirementValue);
    }
}