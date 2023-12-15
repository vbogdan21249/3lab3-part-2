using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using DAL;

namespace BLL
{
    //[JsonDerivedType(typeof(Entity), typeDiscriminator: "base")]
    //[JsonDerivedType(typeof(Student), typeDiscriminator: "student")]
    //[JsonDerivedType(typeof(Pilot), typeDiscriminator: "Pilot")]
    //[JsonDerivedType(typeof(Musician), typeDiscriminator: "Musician")]
    [Serializable]
    [XmlInclude(typeof(Student))]
    [XmlInclude(typeof(Pilot))]
    [XmlInclude(typeof(Musician))]
    public class Entity : ISkate
    {
        [XmlElement]
        public string LastName { get; set; }
        public Entity() { }
        [JsonConstructor]
        public Entity(string LastNameInput)
        {
            LastName = LastNameInput;
        }
        public virtual string[] Methods { get { return new string[] { "Skating" }; } }
        public string Skate()
        {
            return LastName + " is skating";
        }

        public override string ToString() => LastName;
        protected Entity(SerializationInfo info, StreamingContext context)
        {
            LastName = info.GetString("LastName");

        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LastName", LastName);

        }

    }

    [Serializable]
    public class Student : Entity
    {
        private string? studentID;
        private int? course;
        private string? gender;
        private bool inDorms;
        private int? dormitoryNumber;
        private int? dormitoryRoom;

        public string? StudentID { get; set; }
        public int? Course { get; set; }
        public string? Gender { get; set; }
        public bool InDorms { get; set; }
        public int? DormitoryNumber { get; set; }
        public int? DormitoryRoom { get; set; }
        public Student() { }
        public Student(string studentID, int course, string gender, bool inDorms, int dormitoryNumber, int dormitoryRoom)
        {
            StudentID = studentID;
            Course = course;
            Gender = gender;
            InDorms = inDorms;
            DormitoryNumber = dormitoryNumber;
            DormitoryRoom = dormitoryRoom;
        }
        [JsonConstructor]
        public Student(string? LastName, string? Gender, string? StudentID, int? Course, bool InDorms,int? DormitoryNumber, int? DormitoryRoom) : base(LastName)
        {
            (gender, studentID, course, inDorms, dormitoryNumber, dormitoryRoom) = (Gender, StudentID, Course, InDorms, DormitoryNumber, DormitoryRoom);
        }
    }

    [Serializable]
    public class Pilot : Entity
    {
        public Pilot() { }
        public Pilot(string LastName) : base(LastName) { }
        public string Compose()
        {
            return LastName + " is flying.";
        }
    }
    [Serializable]
    public class Musician : Entity
    {
        public Musician() { }
        public Musician(string LastName) : base(LastName) { }
        public string Compose()
        {
            return LastName + " is composing.";
        }

    }
}
