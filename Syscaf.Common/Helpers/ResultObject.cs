namespace Syscaf.Service.Helpers
{
    public class ResultObject
    {
        public bool Exitoso { get; set; }

        public object Data { get; set; } 

        public string Mensaje { get; set; } = String.Empty; 
        public void error()
        {
            Exitoso = false;
        }
        public void error(string message = null)
        {
            Exitoso = false;
            Mensaje = message;
        }
        public void error(string message = null, object data = null)
        {
            Exitoso = false;
            Mensaje = message;
            Data = data;
        }
        public void success()
        {
            Exitoso = true;
        }
        public void success(object data = null)
        {
            Exitoso = true;
            Data = data;
        }
        public void success(string message = null, object data = null)
        {
            Exitoso = true;
            Mensaje = message;
            Data = data;
        }

        public void success(string message = null)
        {
            Exitoso = true;
            Mensaje = message;

        }
    }
}
