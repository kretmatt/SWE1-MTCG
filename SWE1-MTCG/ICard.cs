namespace SWE1_MTCG
{
    public interface ICard
    {
        string PrintCard();
        double ReceiveDamage(ICardAction cardAction);
        ICardAction UseCard();
    }
}