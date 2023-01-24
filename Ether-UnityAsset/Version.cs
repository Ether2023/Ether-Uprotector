using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset
{
    public enum AssetsFileFormatVersion
    {
        //
        // 摘要:
        //     Added support for stripped objects.
        v5_0_1_AndUp = 0xF,
        //
        // 摘要:
        //     Refactoring of class id.
        v5_5_0a_AndUp,
        //
        // 摘要:
        //     Refactoring type data.
        v5_5_0b_AndUp,
        //
        // 摘要:
        //     Refactoring of shareable type tree data.
        v2019_1a_AndUp,
        //
        // 摘要:
        //     Added flags for type trees nodes.
        v2019_1_AndUp,
        //
        // 摘要:
        //     Refactoring of serialized types.
        v2019_2_AndUp,
        //
        // 摘要:
        //     Added storing of type dependencies.
        v2019_3_AndUp,
        //
        // 摘要:
        //     Added support for large files.
        v2020_1_AndUp
    }
    public interface IVersionAble<TVersionFormat> where TVersionFormat : Enum
    {
        TVersionFormat Version { get; }
    }
}
