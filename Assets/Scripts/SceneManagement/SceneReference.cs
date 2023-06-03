using System;
using System.Collections.Generic;

namespace SceneManagement
{
    public class SceneReference
    {
        private SceneReference(String name, bool level)
        {
            this.name = name;
            this.level = level;
        }

        private String name;
        private bool level;

        public String GetName()
        {
            return name;
        }

        public bool IsLevel()
        {
            return level;
        }

        public SceneReference GetNextLevel()
        {
            if (level)
            {
                var index = LEVEL_ORDER.IndexOf(this);
                if (-1 != index && index < LEVEL_ORDER.Count - 1)
                {
                    return LEVEL_ORDER[index + 1];
                }
            }

            return null;
        }

        public static readonly SceneReference FOCUS = new("FocusScene", false);
        public static readonly SceneReference START = new("StartScene", false);
        public static readonly SceneReference MENU_LEVEL = new("LevelChoosingScene", false);
        public static readonly SceneReference LEVEL_SHEEP = new("LevelSheepScene", false);

        private static List<SceneReference> LEVEL_ORDER = new List<SceneReference>{ LEVEL_SHEEP };
    }
}