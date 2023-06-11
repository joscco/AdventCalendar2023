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

        private SceneReference enumValue;
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
        public static readonly SceneReference MENU_LEVEL = new("LevelChooserScene", false);
        public static readonly SceneReference LEVEL_SHEEP = new("SheepLevel", true);
        public static readonly SceneReference LEVEL_FROG = new("FrogLevel", true);
        public static readonly SceneReference LEVEL_WIZARD = new("WizardLevel", true);

        private static List<SceneReference> LEVEL_ORDER = new() { LEVEL_SHEEP };
        
        public static SceneReference GetReferenceForLevel(LevelReference reference) {
            switch (reference) 
            {
                case LevelReference.FROG_LEVEL:
                    return LEVEL_FROG;
                case LevelReference.SHEEP_LEVEL:
                    return LEVEL_SHEEP;
                case LevelReference.WIZARD_LEVEL:
                    return LEVEL_WIZARD;
            }

            return LEVEL_SHEEP;
        }
    }
}