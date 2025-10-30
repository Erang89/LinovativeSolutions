using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Accountings
{

    [LocalizerKey(nameof(AccountDto))]
    public class AccountDto : EntityDtoBase
    {

        [LocalizedRequired, UniqueField(EntityTypes.Account)]
        public string? AccountCode { get; set; }

        [LocalizedRequired, UniqueField(EntityTypes.Account)]
        public string? Name { get; set; } = default!;

        [LocalizedRequired, EntityID(EntityTypes.COAGroup)]
        public Guid? GroupId { get; set; }


        [EntityID(EntityTypes.Account)]
        public Guid? ParentAccountId { get; set; }
    }


    public class AccountViewDto : AccountDto
    {
        public AccountParentDto? Parent { get; set; }
        public COAGroupDto? Group { get; set; }
    }


    public class AccountParentDto : AccountDto
    {
        public bool IsParent => true;
    }

}
