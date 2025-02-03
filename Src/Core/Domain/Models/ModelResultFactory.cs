using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models
{
    /// <summary>
    /// Fábrica de commads result
    /// </summary>
    public static class ModelResultFactory
    {
        /// <summary>
        /// Retorna um commad result com as mensagemns de sucesso do insert
        /// </summary>
        public static ModelResult InsertSucessResult<T>(object model)
        {
            ModelResult result = new ModelResult(model);
            result.AddMessage(BusinessMessages.InsertSucess<T>());
            return result;
        }

        /// <summary>
        /// Retorna um commad result com as mensagens de sucesso do update
        /// </summary>
        public static ModelResult UpdateSucessResult<T>(object model)
        {
            ModelResult result = new ModelResult(model);
            result.AddMessage(BusinessMessages.UpdateSucess<T>());
            return result;
        }

        /// <summary>
        /// Retorna um result com as mensagens de sucesso do delete
        /// </summary>
        public static ModelResult DeleteSucessResult<T>(object? model = null)
        {
            ModelResult result = new ModelResult(model);
            result.AddMessage(BusinessMessages.DeleteSucess<T>());
            return result;
        }

        /// <summary>
        /// Retorna um commad result com o erro de duplicidade
        /// </summary>
        public static ModelResult DuplicatedResult<T>()
        {
            ModelResult result = new ModelResult();
            result.AddError(BusinessMessages.DuplicatedError<T>());
            return result;
        }

        /// <summary>
        /// Retorna um commad result com o erro de não encontrato
        /// </summary>
        public static ModelResult NotFoundResult<T>()
        {
            ModelResult result = new ModelResult();
            result.AddError(BusinessMessages.NotFoundError<T>());
            return result;
        }

        /// <summary>
        /// Retorna um commad result com a mensagem de sucesso generico
        /// </summary>
        public static ModelResult SucessResult()
        {
            ModelResult result = new ModelResult();
            result.AddMessage(BusinessMessages.GenericSucess());
            return result;
        }

        /// <summary>
        /// Retorna um commad result com a mensagem de sucesso generico
        /// </summary>
        public static ModelResult SucessResult(object model)
        {
            ModelResult result = new ModelResult(model);
            result.AddMessage(BusinessMessages.GenericSucess());
            return result;
        }

        /// <summary>
        /// Retorna um commad resut novo
        /// </summary>
        public static ModelResult None()
        {
            return new ModelResult();
        }

        /// <summary>
        /// Retorna um result com as mensagens
        /// </summary>
        public static ModelResult Message(string message)
        {
            ModelResult result = new ModelResult();
            result.AddMessage(message);
            return result;
        }

        /// <summary>
        /// Retorna um result com os erros informados
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ModelResult Error(params string[] errors)
        {
            ModelResult result = new ModelResult();
            foreach (string error in errors) result.AddError(error);
            return result;
        }

        internal static ModelResult DeleteFailResult<T>(params string[] errors)
        {
            ModelResult result = new ModelResult();
            result.AddMessage(ErrorMessages.DeleteDatabaseError<T>());
            foreach (string error in errors) result.AddError(error);
            return result;
        }
    }
}