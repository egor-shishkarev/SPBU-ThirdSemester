namespace Reflection;

using System.Reflection;

public class Reflector
{

    public Reflector(string path)
    {

    }


    // Создает файл с именем класса, сохраняя все его методы и поля, но без реализации. Модификаторы такие же, как в изначальном классе. Сохранение генериковости.
    public void PrintStructure(Type someClass)
    {
        var file = File.Create($"../../../../Result/{someClass.FullName}.cs");
        var writer = new StreamWriter(file);
        writer.WriteLine($"{getAttribute(someClass)} class {someClass}");
        writer.WriteLine("{");
        foreach (MemberInfo mi in someClass.GetTypeInfo().DeclaredMembers)
        {
            var typeName = mi switch
            {
                TypeInfo _ => "TypeInfo",
                FieldInfo _ => "FieldInfo",
                MethodInfo _ => "MethodInfo",
                ConstructorInfo _ => "ConstructorInfo",
                PropertyInfo _ => "PropertyInfo",
                EventInfo _ => "EventInfo",
                _ => ""
            };
            if (typeName == "FieldInfo")
            {
                writer.WriteLine($"\t{((mi as FieldInfo).Attributes.ToString().Replace(",", "")).ToString().ToLower()} {(mi as FieldInfo).FieldType} {mi.Name};\n");
            }
            if (typeName == "MethodInfo")
            {
                var _ = mi as MethodInfo;
                writer.WriteLine($"\t{getAttributeOfInfo((mi as MethodInfo).Attributes)} {mi.Name}()" + " {};");
            }
        }
        writer.WriteLine("}");
        writer.Close();
    }

    // Вывод всех полей и методов, различающихся в обоих классах
    public void DiffClasses(Type a, Type b)
    {

    }

    private string getAttribute(Type someClass)
    {
        List<string> attributes = new();
        if (someClass.IsEnum)
        {
            attributes.Add("enum");
        }
        if (someClass.IsInterface)
        {
            attributes.Add("interface");
        }
        if (someClass.IsPublic)
        {
            attributes.Add("public");
        }
        if (someClass.IsNotPublic)
        {
            attributes.Add("private");
        }
        if (someClass.Attributes.ToString().Contains("Static"))
        {
            attributes.Add("static");
        }
        return String.Join(" ", attributes.ToArray());
    }

    private string getAttributeOfInfo(MethodAttributes info)
    {
        Console.WriteLine(info);
        if (info.HasFlag(MethodAttributes.Public))
        {

        }
        return "";
    }
}