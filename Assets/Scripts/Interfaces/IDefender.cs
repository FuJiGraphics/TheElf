
public interface IDefender
{
    public bool IsDie { get; }
    public void TakeDamage(int damage, string effectName = "");

} // interface IDefender
