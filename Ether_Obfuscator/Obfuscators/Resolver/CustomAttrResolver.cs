using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
namespace Ether_Obfuscator.Obfuscators.Resolver
{
    public class CustomAttrResolver
    {
        public List<TypeSig> typeReflist = new List<TypeSig>();
        public CustomAttrResolver(ModuleDefMD module) 
        {
            foreach (var type in module.Types.Where(x => !(x.Name.StartsWith("<"))))
            {
                typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(type.CustomAttributes));
                foreach (var method in type.Methods)
                {
                    typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(method.CustomAttributes));
                    foreach (var param in method.Parameters.Where(x => x.ParamDef != null))
                        typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(param.ParamDef.CustomAttributes));
                }
                foreach (var field in type.Fields)
                {
                    typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(field.CustomAttributes));
                }
                foreach (var property in type.Properties)
                {
                    typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(property.CustomAttributes));
                }
                foreach (var _event in type.Events)
                {
                    typeReflist.AddRange(SearchTypeRefFromCustomAttrCollection(_event.CustomAttributes));
                }
            }
        }
        public List<TypeSig> SearchTypeRefFromCustomAttrCollection(CustomAttributeCollection customAttributes)
        {
            List<TypeSig> list = new List<TypeSig>();
            foreach (CustomAttribute customAttribute in customAttributes) 
            {
                foreach(var customAttrArgs in customAttribute.ConstructorArguments)
                {
                    list.AddRange(GetTypeRefsFromConstructorArguments(customAttrArgs));
                }
            }
            return list;
        }
        public List<TypeSig> GetTypeRefsFromConstructorArguments(CAArgument argument)
        {
            List<TypeSig> list = new List<TypeSig>();
            if(argument.Type.FullName == "System.Type")
                list.Add((TypeSig)argument.Value);
            return list;
        }
    }
}
