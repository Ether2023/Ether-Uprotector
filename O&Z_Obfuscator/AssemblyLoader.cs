using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OZ_Obfus
{
    public class AssemblyLoader
    {
        public Assembly OriginAssembly;
        public AssemblyDef Assembly;
        public ModuleDefMD Module{ get; set; }
        public string Path;
        public AssemblyLoader(string Assembly)
        {
            Path = Assembly.Replace("\"", "");
            LoadAssembly();
            LoadModuleDefMD();
            LoadDependencies();
            Tools.Importer =  new Importer(Module);
        }
        public void LoadAssembly()
        {
            Console.Write("Loading assembly...\n");
            OriginAssembly = System.Reflection.Assembly.UnsafeLoadFrom(Path);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" Assembly: ");
            Console.WriteLine(OriginAssembly.FullName);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LoadAssemblyFromBytes(byte[] array)
        {
            Console.Write("Loading assembly...");
            OriginAssembly = System.Reflection.Assembly.Load(array);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" Loaded: ");
            Console.WriteLine(OriginAssembly.FullName);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LoadModuleDefMD()
        {
            Console.Write("Loading Mono ModuleDefMD...\n");
            ModuleCreationOptions modOpts = new ModuleCreationOptions(CLRRuntimeReaderKind.Mono);
            Module = ModuleDefMD.Load(Path, modOpts);
            Assembly = Module.Assembly;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" Module: ");
            Console.WriteLine(Module.FullName);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LoadModuleDefMDFromBytes(byte[] array)
        {
            Console.Write("Loading ModuleDefMD...");
            Module = ModuleDefMD.Load(array);
            Assembly = Module.Assembly;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" Loaded: ");
            Console.WriteLine(Module.FullName);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LoadDependencies()
        {
            Console.WriteLine("Analysing dependencies...");
            var asmResolver = new AssemblyResolver();
            ModuleContext modCtx = new ModuleContext(asmResolver);

            asmResolver.DefaultModuleContext = modCtx;

            asmResolver.EnableTypeDefCache = true;

            asmResolver.DefaultModuleContext = new ModuleContext(asmResolver);
            asmResolver.PostSearchPaths.Insert(0, Path);
            if (IsCosturaPresent(Module))
            {
                foreach (var asm in ExtractCosturaEmbeddedAssemblies(GetEmbeddedCosturaAssemblies(Module), Module))
                    asmResolver.AddToCache(asm);
            }

            int i = 0;
            foreach (var dependency in Module.GetAssemblyRefs())
            {
                AssemblyDef assembly = asmResolver.ResolveThrow(dependency, Module);
                Console.WriteLine("  dependency["+i+"]:" + dependency.Name);
                i++;
            }
            Module.Context = modCtx;
        }

        public void LoadDependenciesFromBytes(List<byte[]> files)
        {
            Console.WriteLine("Resolving dependencies...");
            var asmResolver = new AssemblyResolver();
            ModuleContext modCtx = new ModuleContext(asmResolver);

            asmResolver.DefaultModuleContext = modCtx;

            asmResolver.EnableTypeDefCache = true;

            asmResolver.DefaultModuleContext = new ModuleContext(asmResolver);
            asmResolver.PostSearchPaths.Insert(0, Path);

            foreach (var item in files)
            {
                AssemblyDef assembly = AssemblyDef.Load(item);
                asmResolver.AddToCache(assembly);
                Console.WriteLine("Resolved " + assembly.Name);
            }

            Module.Context = modCtx;
        }

        public bool IsCosturaPresent(ModuleDef module) =>
            module.Types.FirstOrDefault(t => t.Name == "AssemblyLoader" && t.Namespace == "Costura") != null;

        public string[] GetEmbeddedCosturaAssemblies(ModuleDef module)
        {
            var list = new List<string>();

            var ctor = module.Types.Single(t => t.Name == "AssemblyLoader" && t.Namespace == "Costura").FindStaticConstructor();
            var instructions = ctor.Body.Instructions;
            for (var i = 1; i < instructions.Count; i++)
            {
                var curr = instructions[i];
                if (curr.OpCode != OpCodes.Ldstr || instructions[i - 1].OpCode != OpCodes.Ldstr)
                    continue;

                var resName = ((string)curr.Operand).ToLowerInvariant();
                if (resName.EndsWith(".pdb") || resName.EndsWith(".pdb.compressed"))
                {
                    i++;
                    continue;
                }

                list.Add((string)curr.Operand);
            }

            return list.ToArray();
        }

        public List<AssemblyDef> ExtractCosturaEmbeddedAssemblies(string[] assemblies, ModuleDef module)
        {
            var list = new List<AssemblyDef>();

            foreach (var assembly in assemblies)
            {
                EmbeddedResource resource = module.Resources.FindEmbeddedResource(assembly.ToLowerInvariant());
                if (resource == null)
                    throw new Exception("Couldn't find Costura embedded assembly: " + assembly);

                if (resource.Name.EndsWith(".compressed"))
                {
                    resource.CreateReader().CopyTo(new MemoryStream());
                    list.Add(DecompressCosturaAssembly(resource.CreateReader().AsStream()));
                    continue;
                }

                list.Add(AssemblyDef.Load(resource.CreateReader().AsStream()));
            }

            return list;
        }

        public AssemblyDef DecompressCosturaAssembly(Stream resource)
        {
            using (var def = new DeflateStream(resource, CompressionMode.Decompress))
            {
                var ms = new MemoryStream();
                def.CopyTo(ms);
                ms.Position = 0;
                return AssemblyDef.Load(ms);
            }
        }
        public void Save()
        {
            ModuleWriterOptions opts = new ModuleWriterOptions(Module);
            opts.Logger = DummyLogger.NoThrowInstance;
            Assembly.Write(Path + ".obfuscated", opts);
            Console.WriteLine("Saved.");
        }
    }
}
