using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Newtonsoft.Json.Linq;
using Uprotector_Hub.Models;

namespace Uprotector_Hub.Services;

public class WindowsEditorManagementService : IEditorManagementService
{
    private EditorModel[] _editorModels;
    public async Task RefreshEditors()
    {
        var path = $"C:/Users/{Environment.UserName}/AppData/Roaming/UnityHub/editors-v2.json";
        var jsonRoot = JObject.Parse(await File.ReadAllTextAsync(path));

        if (!jsonRoot.TryGetValue("data", out var dataProperty))
            throw new Exception("Failed to read editor list");

        _editorModels = dataProperty.ToObject<EditorModel[]>()!;
    }

    public IList<EditorModel> Editors => _editorModels;
}