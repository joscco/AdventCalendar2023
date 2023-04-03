using System.Reflection.Emit;
using Code.GameScene.Items.Item;

namespace Code.GameScene.Items.Field
{
    public class FieldUpdateData
    {
        public bool shouldEvolve;
        public bool shouldDelete;
        public PlantData newPlantData;

        public FieldUpdateData(bool evolve, bool delete)
        {
            shouldEvolve = evolve;
            shouldDelete = delete;
        }
        
        public FieldUpdateData(PlantData data)
        {
            newPlantData = data;
        }
    }
}