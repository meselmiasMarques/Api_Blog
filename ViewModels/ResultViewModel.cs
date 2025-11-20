namespace Blog.ViewModels;

public class ResultViewModel<T> 
{
    public ResultViewModel(T data, List<string> errors)
        =>  Erros = errors;
    
    public ResultViewModel(T data)
        => Data = data;
    
    public ResultViewModel(string error)
        => Erros.Add(error);
    public ResultViewModel(List<string> errors)
        => Erros = errors;
    public T Data { get; private set; }
    public List<string> Erros { get; private set; } = new ();
    
    
}