namespace GameScene
{
    public enum GameMode
    {
        Planting,
        Crafting
    }
    
    public class ModeManager
    {
        public GameMode Mode = GameMode.Planting;

        public void SetMode(GameMode newMode)
        {
            Mode = newMode;
        }
    }
}