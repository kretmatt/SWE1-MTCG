namespace SWE1_MTCG
{
    public interface IArena
    {
        ICard DetermineVictor(ICard attacker, ICard defender);
    }
}