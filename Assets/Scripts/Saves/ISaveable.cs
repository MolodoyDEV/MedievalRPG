namespace Molodoy.CoreComponents.Saves
{
    public interface ISaveable
    {
        public void LoadData(SavedData.ISaveData savedData);

        public SavedData.ISaveData GetDataToSave();
    }
}
