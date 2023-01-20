using System.Collections.Generic;
using System.Threading.Tasks;
using Uprotector_Hub.Models;

namespace Uprotector_Hub.Services;

public interface IEditorManagementService
{
    public Task RefreshEditors();
    public IList<EditorModel> Editors { get; }
}