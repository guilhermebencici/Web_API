﻿namespace APICatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacaco(string nome)
        {
            return $"Bem vindo, {nome} \n\n{DateTime.Now}";
        }
    }
}
