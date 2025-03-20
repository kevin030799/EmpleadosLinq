using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmpleadosLinq
{
    class Program
    {
        static string xmlRuta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\LINQApp\Empleados.xml");
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= Menú de Empleados =======");
                Console.WriteLine("1. Consultar empleados mayores de 30 años");
                Console.WriteLine("2. Consultar empleados con salario mayor a 50,000");
                Console.WriteLine("3. Ver el promedio de salarios por departamento");
                Console.WriteLine("4. Ver el empleado con el salario más alto");
                Console.WriteLine("5. Actualizar el salario de un empleado");
                Console.WriteLine("6. Eliminar un empleado por Id");
                Console.WriteLine("7. Agregar un nuevo empleado");
                Console.WriteLine("8. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ConsultarMayores30();
                        break;
                    case "2":
                        ConsultarSalarioMayor50000();
                        break;
                    case "3":
                        PromedioSalarioPorDepartamento();
                        break;
                    case "4":
                        EmpleadoSalarioMasAlto();
                        break;
                    case "5":
                        ActualizarSalario();
                        break;
                    case "6":
                        EliminarEmpleado();
                        break;
                    case "7":
                        AgregarEmpleado();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }

                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void ConsultarMayores30()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            var empleados = xmlDoc.Descendants("Empleado")
                .Where(x => (int)x.Element("Edad") > 30)
                .Select(x => x.Element("Nombre")?.Value);

            Console.WriteLine("\nEmpleados mayores de 30 años:");
            foreach (var nombre in empleados)
            {
                Console.WriteLine(nombre);
            }
        }

        static void ConsultarSalarioMayor50000()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            var empleados = xmlDoc.Descendants("Empleado")
                .Where(x => (decimal)x.Element("Salario") > 50000)
                .Select(x => x.Element("Nombre")?.Value);

            Console.WriteLine("\nEmpleados con salario mayor a 50,000:");
            foreach (var nombre in empleados)
            {
                Console.WriteLine(nombre);
            }
        }

        static void PromedioSalarioPorDepartamento()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            var promedio = xmlDoc.Descendants("Empleado")
                .GroupBy(x => x.Element("Departamento")?.Value)
                .Select(g => new
                {
                    Departamento = g.Key,
                    PromedioSalario = g.Average(x => (decimal)x.Element("Salario"))
                });

            Console.WriteLine("\nPromedio de salario por departamento:");
            foreach (var item in promedio)
            {
                Console.WriteLine($"{item.Departamento}: {item.PromedioSalario:C}");
            }
        }

        static void EmpleadoSalarioMasAlto()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            var empleado = xmlDoc.Descendants("Empleado")
                .OrderByDescending(x => (decimal)x.Element("Salario"))
                .FirstOrDefault();

            if (empleado != null)
            {
                Console.WriteLine("\nEmpleado con el salario más alto:");
                Console.WriteLine($"Nombre: {empleado.Element("Nombre")?.Value}");
                Console.WriteLine($"Salario: {empleado.Element("Salario")?.Value}");
            }
        }

        static void ActualizarSalario()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            Console.Write("\nIngrese el ID del empleado: ");
            string id = Console.ReadLine();
            var empleado = xmlDoc.Descendants("Empleado")
                .FirstOrDefault(x => x.Element("Id")?.Value == id);

            if (empleado != null)
            {
                Console.Write("Ingrese el nuevo salario: ");
                string nuevoSalario = Console.ReadLine();
                empleado.Element("Salario")?.SetValue(nuevoSalario);
                xmlDoc.Save(xmlRuta);
                Console.WriteLine("Salario actualizado con éxito.");
            }
            else
            {
                Console.WriteLine("Empleado no encontrado.");
            }
        }

        static void EliminarEmpleado()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            Console.Write("\nIngrese el ID del empleado a eliminar: ");
            string id = Console.ReadLine();
            var empleado = xmlDoc.Descendants("Empleado")
                .FirstOrDefault(x => x.Element("Id")?.Value == id);

            if (empleado != null)
            {
                empleado.Remove();
                xmlDoc.Save(xmlRuta);
                Console.WriteLine("Empleado eliminado con éxito.");
            }
            else
            {
                Console.WriteLine("Empleado no encontrado.");
            }
        }

        static void AgregarEmpleado()
        {
            XDocument xmlDoc = XDocument.Load(xmlRuta);

            Console.Write("\nIngrese el ID del nuevo empleado: ");
            string id = Console.ReadLine();
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese la edad: ");
            string edad = Console.ReadLine();
            Console.Write("Ingrese el departamento: ");
            string departamento = Console.ReadLine();
            Console.Write("Ingrese el salario: ");
            string salario = Console.ReadLine();

            XElement nuevoEmpleado = new XElement("Empleado",
                new XElement("Id", id),
                new XElement("Nombre", nombre),
                new XElement("Edad", edad),
                new XElement("Departamento", departamento),
                new XElement("Salario", salario)
            );

            xmlDoc.Element("Empleados")?.Add(nuevoEmpleado);
            xmlDoc.Save(xmlRuta);
            Console.WriteLine("Empleado agregado con éxito.");
        }


    }
}

