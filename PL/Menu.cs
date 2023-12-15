using System.Reflection;
using BLL;

namespace PL
{
    public class Menu
    {
        EntityService list = new();
        public void ShowMain()
        {
            Console.Clear();
            Console.WriteLine(
               "1 - view all;\n" +
               "2 - view the database of students;\n" +
               "3 - view the database of pilots;\n" +
               "4 - view the database of musicians;\n" +
               "5 - add students to the database;\n" +
               "6 - add pilots to the database;\n" +
               "7 - add musicians to the database;\n" +
               "8 - search\n" +
               "9 - db settings (name and format)\n" +
               "0 - EXIT");
        }

        public void Search()
        {
            List<Tuple<int, Student>> searchList = list.Search();
            Console.Clear();
            Console.WriteLine("Search: " + searchList.Count + " items ");
            int currentIndex = 0;
            while (currentIndex < searchList.Count)
            {
                Console.WriteLine(1 + searchList[currentIndex].Item1 + ". " + searchList[currentIndex].Item2);
                currentIndex++;
            }
            Console.WriteLine("0 - menu");

            bool done = false;
            do
            {
                var input = Console.ReadLine();
                switch (input)
                {
                    case "0": ShowMain(); return;
                    default:
                        ShowMain(); return;
                }
            }
            while (!done);
        }
        public void ShowPeople(string? typeOfEntity)
        {
            Console.Clear();
            Console.WriteLine(list.Length() + " entities:");
            for (int index = 0; index < list.Length(); index++)
            {
                if (typeOfEntity == null || list[index].GetType().Name == typeOfEntity)
                {
                    Console.WriteLine(1 + index + ". " + list[index]);
                }
            }
            Console.WriteLine("0 - back to menu, entity number - view entity");
            bool done = false;
            do
            {
                var i = Console.ReadLine();
                switch (i)
                {
                    case "0": ShowMain(); return;
                    default:
                        try
                        {
                            int parsed = Convert.ToInt32(i);
                            if (parsed < 1 || parsed > list.Length()) throw new WrongInputException();
                            ViewEntity(parsed - 1);
                            done = true;
                            return;
                        }
                        catch (Exception) { Console.WriteLine("Invalid input!"); }
                        break;
                }

            } while (!done);
        }
        public void ViewEntity(int index)
        {
            Console.Clear();
            Console.WriteLine("Entity: " + (index + 1) + " of " + list.Length() + " items");
            Entity entity = list[index];
            Console.WriteLine(1 + index + ". " + entity);
            Console.WriteLine("This entity can:");
            for (int i = 0; i < entity.Methods.Length; i++)
            {
                Console.WriteLine(entity.Methods[i] + " ");
            }
            Console.WriteLine("0 - go back to menu; 1 - edit; 2 - delete;");
            bool done = false;
            do
            {
                var input = Console.ReadLine();
                if (input != null && input.StartsWith("do "))
                {
                    String MethodName = input.Split(' ')[1]; ;
                    Type type = entity.GetType();
                    MethodInfo? theMethod = type.GetMethod(MethodName);
                    if (theMethod == null)
                    {
                        Console.WriteLine("Error: no such function");
                    }
                    else
                    {
                        Console.WriteLine(theMethod.Invoke(entity, new string[] { }));
                        list.Save();
                    }
                }
                else
                {
                    switch (input)
                    {
                        case "0": ShowMain(); return;
                        case "1": AddOrEdit(entity, index, null); return;
                        case "2":
                            try
                            {
                                list.Delete(index);
                                ShowMain(); return;
                            }
                            catch (Exception) { Console.WriteLine("Failed to delete!"); }
                            break;
                        default: Console.WriteLine("Invalid input!"); break;
                    }
                }
            }
            while (!done);
        }
        public void AddOrEdit(Entity? entity, int? index, string? type)
        {
            Console.Clear();
            bool isEditing = entity != null;
            Console.WriteLine(isEditing ? "Editing " + entity.GetType().Name : "Adding " + type);
            if (isEditing)
            {
                Console.WriteLine(entity.ToString());
                type = entity.GetType().Name;
            }
            Entity? newEntity = null;
            switch (type)
            {
                case "Student":
                    Student? oldStudent = isEditing ? (Student)entity : null;
                    newEntity = new Student(
                        AskNameOrGender(isEditing ? oldStudent.LastName : null, true),
                        AskNameOrGender(isEditing ? oldStudent.Gender : null, false),
                        AskID(isEditing ? oldStudent.StudentID : null),
                        AskCourse(isEditing ? oldStudent.Course : null),
                        AskDormitory(isEditing ? oldStudent.InDorms : false),
                        AskDormitoryNumber(isEditing && oldStudent.InDorms ? oldStudent.DormitoryNumber: null),
                        AskDormitoryRoom(isEditing && oldStudent.InDorms ? oldStudent.DormitoryRoom: null)
                    );
                    ShowMain();
                    break;
                case "Pilot":
                    Pilot? oldTailor = isEditing ? (Pilot)entity : null;
                    newEntity = new Pilot(
                        AskNameOrGender(isEditing ? oldTailor.LastName : null, true)
                    );
                    ShowMain();
                    break;
                case "Musician":
                    Musician? oldSinger = isEditing ? (Musician)entity : null;
                    newEntity = new Musician(
                        AskNameOrGender(isEditing ? oldSinger.LastName : null, true)
                    );
                    ShowMain();
                    break;
                default:
                    Console.WriteLine("I don`t know how to create this entity(\nNew key to go back");
                    Console.ReadKey();
                    break;
            }
            if (newEntity != null)
            {
                if (isEditing)
                {
                    list.Update(newEntity, (int)index);
                }
                else
                {
                    list.Insert(newEntity);
                }
            }
            return;
        }
        public string? AskNameOrGender(string? input, bool isName)
        {
            bool done = false;
            string nameOrGender = "";
            do
            {
                if (isName == true)
                {
                    Console.WriteLine("Enter name: " + (input != null ? "(" + input + ")" : ""));
                }
                else
                {
                    Console.WriteLine("Enter gender: " + (input != null ? "(" + input + ")" : ""));
                }
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "" && input != null) { return input; }
                try
                {
                    EntityService.ValidateNameOrGender(stringFromConsole);
                    nameOrGender = stringFromConsole;
                }
                catch (Exception) { Console.WriteLine("Wrong input!"); continue; }
                done = true;
            }
            while (!done);
            return nameOrGender;
        }
        public string? AskID(string? input)
        {
            bool done = false;
            string id = "";
            do
            {
                Console.WriteLine("Enter id: " + (input != null ? "(" + input + ")" : ""));
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "" && input != null) { return input; }
                try
                {
                    EntityService.ValidateID(stringFromConsole);
                    id = stringFromConsole;
                }
                catch (Exception) { Console.WriteLine("Wrong id!"); continue; }
                done = true;
            }
            while (!done);
            return id;
        }
        public int? AskCourse(int? input)
        {
            bool done = false;
            int? course = 0;
            do
            {
                Console.WriteLine("Enter course: " + (input != null ? "(" + input + ")" : ""));
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "")
                {
                    if (input != null) { return input; }
                    return null;
                }
                try
                {
                    int parsed = Convert.ToInt32(stringFromConsole);
                    EntityService.ValidateCourse(parsed);
                    course = parsed;
                }
                catch (Exception) { Console.WriteLine("Wrong course!"); continue; }
                done = true;
            }
            while (!done);
            return course;
        }
        public bool AskDormitory(bool input)
        {
            bool done = false;
            bool inDorms = false;
            do
            {
                Console.WriteLine("Lives in dormitory (0 - no, 1 - yes): " + (input != null ? "(" + input + ")" : ""));
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "")
                {
                    if (input != null) { return input; }
                    return false;
                }
                try
                {
                    bool parsed = Convert.ToBoolean(stringFromConsole);
                    EntityService.ValidateInDorms(parsed);
                    inDorms = parsed;
                }
                catch (Exception) { Console.WriteLine("Wrong input!"); continue; }
                done = true;
            }
            while (!done);
            return inDorms;
        }

        public int? AskDormitoryNumber(int? input)
        {
            bool done = false;
            int dormitoryNumber = 1;
            do
            {
                Console.WriteLine("Enter dormitory number: " + (input != null ? "(" + input + ")" : ""));
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "")
                {
                    if (input != null) { return input; }
                    return null;
                }
                try
                {
                    int parsed = Convert.ToInt32(stringFromConsole);
                    EntityService.ValidateDormitoryNumber(parsed);
                    dormitoryNumber = parsed;
                }
                catch (Exception) { Console.WriteLine("Wrong input!"); continue; }
                done = true;
            }
            while (!done);
            return dormitoryNumber;
        }
        public int? AskDormitoryRoom(int? input)
        {
            bool done = false;
            int dormitoryRoom = 100;
            do
            {
                Console.WriteLine("Enter dormitory room: " + (input != null ? "(" + input + ")" : ""));
                string? stringFromConsole = Console.ReadLine();
                if (stringFromConsole == null || stringFromConsole == "")
                {
                    if (input != null) { return input; }
                    return null;
                }
                try
                {
                    int parsed = Convert.ToInt32(stringFromConsole);
                    EntityService.ValidateDormitoryRoom(parsed);
                    dormitoryRoom = parsed;
                }
                catch (Exception) { Console.WriteLine("Wrong input!"); continue; }
                done = true;
            }
            while (!done);
            return dormitoryRoom;
        }
        public void ShowDBSettings()
        {
            string CurrentDBName = $"{list.DBName}.{list.DBType}";
            Console.Clear();
            Console.WriteLine("Current DB: " + CurrentDBName);
            Console.WriteLine("Available db types: " + string.Join(", ", list.AvailableDBTypes));
            Console.WriteLine("Enter new db name: ");
            Console.WriteLine("0 - menu");
            bool done = false;
            do
            {
                var input = Console.ReadLine();
                if (input == null || input == "") { continue; }
                switch (input)
                {
                    case "0": ShowMain(); return;
                    default:
                        try
                        {
                            list.SetProvider(input);
                            ShowMain();
                            return;
                        }
                        catch (Exception) { Console.WriteLine("Wrong input"); continue; }
                }
            }
            while (!done);
        }
        public void MainMenu()
        {
            bool flag = false;
            ShowMain();
            do
            {
                Console.WriteLine("Enter command:");
                string? i = Console.ReadLine();
                if (!int.TryParse(i, out int result)) { Console.WriteLine("Invalid input!"); }
                else
                {
                    int command = int.Parse(i);
                    switch (command)
                    {
                        case 1:
                            ShowPeople(null);
                            break;
                        case 2:
                            ShowPeople("Student");
                            break;
                        case 3:
                            ShowPeople("Pilot");
                            break;
                        case 4:
                            ShowPeople("Musician");
                            break;
                        case 5:
                            AddOrEdit(null, null, "Student");
                            break;
                        case 6:
                            AddOrEdit(null, null, "Pilot");
                            break;
                        case 7:
                            AddOrEdit(null, null, "Musician");
                            break;
                        case 8:
                            Search();
                            break;
                        case 9:
                            ShowDBSettings();
                            break;
                        case 0:
                            flag = true;
                            break;
                        default:
                            Console.WriteLine("Invalid number!");
                            break;
                    }
                }
            } while (!flag);
        }
    }
}
