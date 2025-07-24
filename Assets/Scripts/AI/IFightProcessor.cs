public interface IFightProcessor
{
    void AddEnemy(CharacterFacade c);
    void AddFriend(CharacterFacade c);
    void ProcessFight();
    void Dispose();
}
