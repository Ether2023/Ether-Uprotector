using System;
using System.Collections.Generic;
using System.Text;

namespace OZ_IL2CPP_GUI
{
    class Il2cppLibUtilitys
    {
        /// <summary>
        /// 原理:使用MetadataHeader.h作为版本区分
        /// 原因:无法读取例如24.5小版本的区别
        /// 实现:利用这个文件的MD5来区分版本
        /// 将OZIl2cppCode打包成Zip放在本软件./OZ_Il2cpp_Code下,分别命名为OZ_Il2cpp_<MD5>.zip
        /// 例如 OZ_Il2cpp_00000000DEADBEEF00000000.zip
        /// 注意MD5全部字母大写(ToUpper)
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        public static string GetVersion(string libPath)
        {
            return "00000000DEADBEEF00000000".ToUpper();
        }
    }
}
