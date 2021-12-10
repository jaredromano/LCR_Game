namespace LCR_Game.Services
{
    public interface IGamePlayer
    {
        int Id { get; set; }
        IGamePlayer GamePlayerLeft { get; }
        IGamePlayer GamePlayerRight { get; }
    }
}