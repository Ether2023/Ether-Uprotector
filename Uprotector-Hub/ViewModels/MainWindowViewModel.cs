using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using Uprotector_Hub.Models;
using Uprotector_Hub.Services;

namespace Uprotector_Hub.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IEditorManagementService _editorManagementService;

    private IList<EditorModel> _editorModels;
    public IList<EditorModel> EditorModels
    {
        get => _editorModels;
        set => this.RaiseAndSetIfChanged(ref _editorModels, value);
    }

    private int _page = 0;
    public int Page
    {
        get => _page;
        set => this.RaiseAndSetIfChanged(ref _page, value);
    }
    
    public MainWindowViewModel(IEditorManagementService editorManagementService)
    {
        _editorManagementService = editorManagementService;
        
        PageSelectCommand = ReactiveCommand.Create<int>(PageSelect);
        
        InitializeProperty();
        InitializeCompute();
    }

    private async void InitializeProperty()
    {
        await _editorManagementService.RefreshEditors();
        EditorModels = _editorManagementService.Editors;
    }

    private void InitializeCompute()
    {
        
    }

    public ReactiveCommand<int, Unit> PageSelectCommand { get; }
    private void PageSelect(int obj) => Page = obj;
}