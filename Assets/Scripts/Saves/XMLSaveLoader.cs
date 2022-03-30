using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Molodoy.CoreComponents.Saves
{
    public static class XMLSaveLoader
    {
        private static GameObject MainHandWeapon = new GameObject();

        //Save the streamingDataObject to xml
        public static void Save()
        {
            Player_Data oList = new Player_Data { baseDamage = 10f};

            //Create new xml file
            XmlSerializer serializer = new XmlSerializer(typeof(Player_Data));             //Create serializer
            FileStream stream = new FileStream(Application.persistentDataPath + "ObjectData", FileMode.Create); //Create file at this path
            serializer.Serialize(stream, oList);//Write the data in the xml file
            stream.Close();//Close the stream
        }

        //Load xml file
        public static void Load()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(Player_Data));            //Create serializer
            FileStream stream = new FileStream(Application.persistentDataPath + "ObjectData", FileMode.Open); //Load file at this path
            Player_Data oList = serializer.Deserialize(stream) as Player_Data;
            stream.Close();//Close the stream

            Debug.LogWarning(oList.baseDamage);

        }
    }

    public class Player_Data
    {
        public GameObject gameObject;
        public float baseDamage;
        public float baseHealth;
        public float currentHealth;
        public float baseMana;
        public float currentMana;
        public float baseMoveSpeed;
    }
}