using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.test;
AssemblyLoader loader = new AssemblyLoader("C:\\Users\\22864\\Desktop\\2019Testbuild\\O&Z_2019_4_32_f1_Data\\Managed\\Assembly-CSharp.bak.dll");
NumObfus obfus = new NumObfus(loader.Module);
obfus.Execute();
loader.Save();