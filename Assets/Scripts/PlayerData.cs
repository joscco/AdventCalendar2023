namespace Code
{
    public class PlayerData
    {
        private int _levelUnlocked = 1;

        public void SetLevelUnlocked(int level)
        {
            if (level >= _levelUnlocked)
            {
                _levelUnlocked = level;
            }
        }

        public int GetLevelUnlocked()
        {
            return _levelUnlocked;
        }
    }
}