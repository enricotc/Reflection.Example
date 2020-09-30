using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Reflection.Example.Patcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (patchFile())
            { Console.WriteLine("File patchato"); }
            else
            { Console.WriteLine("Impossibile patchare"); }
            
            if (filesToDesktop())
            { Console.WriteLine("Files copiati sul desktop"); }
            else
            { Console.WriteLine("Impossibile copiare sul desktop"); }

            Console.ReadLine();
        }

        /// <summary>
        /// Questa funzione modifica la chiave di licenza nella DLL
        /// </summary>
        /// <returns></returns>
        private static bool patchFile()
        {
            bool result = false;

            try
            {
                //Carico la dll da patchare e in particolare l'oggetto Class1
                using ModuleDefinition module = ModuleDefinition.ReadModule(@$"{Directory.GetCurrentDirectory()}\..\..\..\..\Reflection.Example\bin\Debug\netcoreapp3.1\Reflection.Example.Shared.dll");
                TypeDefinition myType = module.Types.First(type => type.Name == "Class1");

                //Mi posiziono sul costruttore... te la spiego la prossima volta.. intanto ricordati che è sdove viene impostato il valore "licenza"
                MethodDefinition ctor = myType.Methods.First(prop => prop.Name == ".ctor");

                //foreach (Instruction i in ctor.Body.Instructions)
                //{
                //    Console.WriteLine($"offset:{i.Offset}, opcode:{i.OpCode}, operand:{i.Operand}, previous:{i.Previous}, next:{i.Next}");
                //}
                //Console.ReadLine();

                //Modifico il valore di "licenza" in "nuova licenza"
                ctor.Body.Instructions.SingleOrDefault(e => e.OpCode.Equals(Mono.Cecil.Cil.OpCodes.Ldstr)).Operand = "nuova licenza";

                //Riscrico la dll con un nome diverso
                module.Write(@$"{Directory.GetCurrentDirectory()}\..\..\..\..\Reflection.Example\bin\Debug\netcoreapp3.1\Reflection.Example.Shared-patched.dll");
                module.Dispose();

                //Lo copio sovrascrivendolo
                File.Copy(@$"{Directory.GetCurrentDirectory()}\..\..\..\..\Reflection.Example\bin\Debug\netcoreapp3.1\Reflection.Example.Shared-patched.dll",
                            @$"{Directory.GetCurrentDirectory()}\..\..\..\..\Reflection.Example\bin\Debug\netcoreapp3.1\Reflection.Example.Shared.dll", true);
                //Cancello il file nominato diversamente
                File.Delete(@$"{Directory.GetCurrentDirectory()}\..\..\..\..\Reflection.Example\bin\Debug\netcoreapp3.1\Reflection.Example.Shared-patched.dll");

                result = true;
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Copio i file sul desktop
        /// </summary>
        /// <returns></returns>
        private static bool filesToDesktop()
        {
            bool result = false;

            try
            {
                //carico la risorsa desiderata dall'exe
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "Reflection.Example.Patcher.Prova.txt";

                //Queste righe prendile cosi come sono.. essenzialmente viene aperto uno stream di lettura per leggere il contenuto del file
                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                using StreamReader reader = new StreamReader(stream);

                // Stessa cosa del precedente, ma in scrittuta sul desktop
                StreamWriter sw = File.CreateText(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Prova.txt");
                sw.Write(reader.ReadToEnd());
                sw.Flush();
                sw.Close();

                result = true;
            }
            catch (Exception ex) { }

            return result;
        }
    }
}
