namespace Api_Senior.ViewModel
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data, List<string> errors) //Dado e lista de erros
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data) //Recebe só sucesso
        {
            Data = data;
        }

        public ResultViewModel(List<string> errors) //Recebe uma lista de erros
        {
            Errors = errors;
        }

        public ResultViewModel(string error) //Apenas um erro
        {
            Errors.Add(error);
        }

        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new List<string>();
    }
}
