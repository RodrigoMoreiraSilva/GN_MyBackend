using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Models;
using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Services.Implementation
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private const string _baseUrlLog = "Namespace: GestaoUnica_backend.Services.Implementation | ";

        private readonly IConfiguration _config;
        private readonly ILogService _logService;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;

        private string _baseUrl = string.Empty;
        private string _endpointLogin = string.Empty;

        public ActiveDirectoryService(IConfiguration config, ILogService logService, IRoleBusiness roleBusiness, IUserBusiness userBusiness)
        {
            _config = config;
            _logService = logService;
            _roleBusiness = roleBusiness;
            _userBusiness = userBusiness;
            _baseUrl = _config.GetSection("ActiveDirectory:API_Base_AD").Value.ToString();
        }
        
        public async Task<bool> IsADAuthenticated(User user)
        {
            _endpointLogin = _config.GetSection("ActiveDirectory:LoginEndpoint").Value.ToString();
            
            var credentials = new
            {
                user.UserName,
                user.Password,
                user.LdapSection
            };
            var uri = string.Format("{0}{1}", this._baseUrl, this._endpointLogin);
            
            var authenticated = false;

            var jsonString = JsonConvert.SerializeObject(credentials);

            using(HttpClient client = new HttpClient())
            {
                var result = await client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                
                if (result.StatusCode == HttpStatusCode.OK)
                    authenticated = true;

                #region Registro de Log
                var logUser = _userBusiness.FindByUsername(user.UserName);
                _logService.SalvarLog
                    (
                        "Services.Implementation.ActiveDirectoryService.IsADAuthenticated", //Url
                        "Chamada à API Active Directory",                                   //Acao
                        _roleBusiness.FindByRoleName(logUser.Role),                         //Regra
                        logUser,                                                            //Usuario
                        result.Content.ReadAsStringAsync().Result                           //Erro
                    );
                #endregion
            }

            return authenticated;
        }
    }
}
