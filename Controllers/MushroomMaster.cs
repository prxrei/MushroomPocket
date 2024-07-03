namespace MushroomPocket.Controllers;
    public class MushroomMaster
    {
        //This code is provided by IT2154 Advanced Programming Assignment
        public string Name { get; set; }
        public int NoToTransform { get; set; }
        public string TransformTo { get; set; }

        public MushroomMaster(string name, int noToTransform, string transformTo)
        {
            this.Name = name;
            this.NoToTransform = noToTransform;
            this.TransformTo = transformTo;
        }
    }