using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Settings;
using LinoVative.Shared.Dto;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace LinoVative.Service.Backend.AuthServices
{
    public class ChangeDefaultCompanyCommand : IRequest<Result>
    {
        public bool SetAsDefaultCompany { get; set; }
        public Guid CompanyId { get; set; }
    }


    public class ChangeDefaultCompanyCommandService : IRequestHandler<ChangeDefaultCompanyCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ILangueageService _lang;
        private readonly IActor _actor;
        private readonly JwtSettings _jwtSettings;

        public ChangeDefaultCompanyCommandService(IOptions<AppSettings> appSettings, IAppDbContext dbCtx, ILangueageService langService, IActor actor)
        {
            _dbContext = dbCtx;
            _lang = langService;
            _actor = actor;
            _jwtSettings = appSettings.Value.JwtSettings;
        }


        public async Task<Result> Handle(ChangeDefaultCompanyCommand request, CancellationToken ct)
        {
            var validate = await Validate(request, ct);
            if (!validate) return validate;

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == _actor.UserId);
            user!.DefaultCompanyId = request.CompanyId;

            var jwt = JwtTokeProvider.Generate(user, _jwtSettings, request.CompanyId);

            await _dbContext.SaveAsync(_actor, ct);

            return Result.OK(jwt);
        }

        async Task<Result> Validate(ChangeDefaultCompanyCommand request, CancellationToken ct)
        {
            var userCompanyExist = _dbContext.UserCompanies.Any(x => x.UserId == _actor.UserId && x.CompanyId == request.CompanyId);
            var isOwner = _dbContext.Companies.Where(x => !x.IsDeleted && x.OwnByUserId == _actor.UserId && x.Id == request.CompanyId).Any();

            if (!userCompanyExist && !isOwner)
                return Result.Failed(_lang[$"{nameof(ChangeDefaultCompanyCommand)}.CompanyId.NotFound"]);


            await Task.CompletedTask;
            return Result.OK();
        }
    }
}
